using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using ChoETL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using NeurekaApi.Dtos;
using NeurekaApi.Hubs;
using NeurekaDAL.Models;
using NeurekaService.Services;
using Syncfusion.XlsIO;

namespace NeurekaApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        public VisitController(IVisitService VisitService, IMapper mapper, IHubContext<NotificationHub> hubcontext, IUserService userService)
        {
            _visitService = VisitService;
            _mapper = mapper;
            _hubContext = hubcontext;
            _userService = userService;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException();
            using (var ms = file.OpenReadStream())
            {
                return Ok(await _visitService.UploadFromStreamAsync(file.FileName, ms));
            }
        }

        [Route("download")]
        [HttpGet]
        public async Task<GridFSDownloadStream<ObjectId>> Download(string fileName)
        {
            return await _visitService.DownloadFromStream(fileName);
        }

        [Route("reportinglabels")]
        [HttpGet]
        public async Task<IActionResult> GetReportingLabels(string patientId)
        {
            return Ok(await _visitService.GetReportingLabels(patientId));
        }

        [Route("getreportingdata")]
        [HttpPost]
        public async Task<IActionResult> GetReportingLabels(ReportingDataQueryDto dto)
        {
            return Ok(await _visitService.GetReportingData(dto.patientId,dto.label));
        }


        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _visitService.Get());

        [HttpGet("export/csv")]
        public async Task ExportCSV()
        {
            using (var w = new ChoCSVWriter("visits.csv").WithFirstLineHeader())
            {
                var json = await _visitService.Get();
                w.WriteFields(json);
            }
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel(string patientId)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Excel2016;

                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Name = "visit";
                worksheet.IsGridLinesVisible = true;

                var visits = await _visitService.GetByPatient(patientId);
                if (visits == null)
                    return NotFound();
                var i = 0;
                foreach (var visit in visits)
                {
                    i = i + 1;
                    worksheet.Range[i, 1].Text = visit.Title;
                    worksheet.Range[i, 2].Text = $"{visit.StartDate} {visit.StartTime}";
                    worksheet.Range[i, 3].Text = $"{visit.EndDate} {visit.EndTime}";

                    var j = 0;
                    foreach (var form in visit.Fields)
                    {
                        j = j + 1;
                        worksheet.Range[i + 1, j + 4].Text = !string.IsNullOrEmpty(form.Title) ? form.Title.ToUpper() : "FORM " + (j - 1);

                        var k = 0;
                        foreach (var field in form.Fields)
                        {
                            k = k + 1;
                            worksheet.Range[i + 2, j + 4 + k].Text = !string.IsNullOrEmpty(field.Title) ? field.Title.ToUpper() : "FIELD " + (k - 1);
                            worksheet.Range[i + 3, j + 4 + k].Text = field.Model;
                            if (field.Models != null && field.Selects != null)
                            {
                                var data = "";
                                foreach (var m in field.Models)
                                {
                                    foreach (var s in field.Selects)
                                    {
                                        //if (s.Id.ToString() == m)
                                            data = data + "," + s;
                                    }

                                }
                                data = data.Substring(1, data.Length - 1);
                                worksheet.Range[i + 3, j + 4 + k].Text = data;
                            }
                            else if (field.Selects != null && field.Models == null)
                            {
                                var data = "";
                                foreach (var s in field.Selects)
                                {
                                    //if (s.Id.ToString() == field.Model)
                                        data = s;
                                }
                                worksheet.Range[i + 3, j + 4 + k].Text = data;
                            }
                            else if (field.FileInfos != null)
                            {
                                var data = "";
                                foreach (var info in field.FileInfos)
                                {

                                    data = data + "," + info.Title;
                                }
                                data = data.Substring(1, data.Length - 1);
                                worksheet.Range[i + 3, j + 4 + k].Text = data;
                            }
                        }
                        j = j + k;
                    }
                    i = i + 5;
                }
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                fileStreamResult.FileDownloadName = "visits.xlsx";

                return fileStreamResult;
            }
        }

        [Route("{id:length(24)}", Name = "GetVisit")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var p = await _visitService.Get(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [Route("patients/{id:length(24)}", Name = "GetByPatient")]
        [HttpGet]
        public async Task<IActionResult> GetByPatient(string id)
        {
            var p = await _visitService.GetByPatient(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VisitDto visitDto)
        {
            var visit = _mapper.Map<Visit>(visitDto);
            await _visitService.Create(visit);
            _ = _hubContext.Clients.All.SendAsync("ReceiveAddedVisit", visitDto);
            return Ok(CreatedAtRoute("Visit", new { id = visitDto.Id.ToString(), visitDto }));
        }

        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, Visit visit)
        {
            var p = _visitService.Get(id);
            if (p == null)
                return NotFound();

            await _visitService.Update(id, visit);
            var userId = visit.DoctorId;
            var user = await _userService.Get(userId);
            var notification = new Notification();
            notification.FromFirstName = user.FirstName;
            notification.FromLastName = user.LastName;
            notification.FromId = userId;
            notification.VisitId = visit.Id;
            notification.VisitTitle = visit.Title;
            notification.PatientId = visit.PatientId;
            notification.FromRole = "admin";

            _ = _hubContext.Clients.All.SendAsync("ReceiveNewUpdatedVisit", notification);
            return Ok();
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = _visitService.Get(id);
            if (p == null)
                return NotFound();

            await _visitService.Remove(id);

            return Ok();
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> DeleteFile(DeleteFileDto deleteFileDto) => Ok(await _visitService.DeleteFile(deleteFileDto.FileName));
    }
}
