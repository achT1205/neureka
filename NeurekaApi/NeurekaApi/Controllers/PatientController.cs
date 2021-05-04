using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NeurekaApi.Dtos;
using NeurekaApi.Hubs;
using NeurekaDAL.Models;
using NeurekaService.Services;

namespace NeurekaApi.Controllers
{
    //[Authorize(Roles ="admin")]
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IVisitService _visitService;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        public PatientController(IPatientService patientService, IVisitService visitService, IMapper mapper, IHubContext<NotificationHub> hubcontext)
        {
            _patientService = patientService;
            _visitService = visitService;
            _mapper = mapper;
            _hubContext = hubcontext;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _patientService.Get());

        [Route("{id:length(24)}", Name = "GetPatient")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var p = await _patientService.Get(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            await _patientService.Create(patient);

            return Ok(CreatedAtRoute("Patient", new { id = patient.Id.ToString(), patient }));
        }

        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, Patient patient)
        {
            var p = _patientService.Get(id);
            if (p == null)
                return NotFound();

            await _patientService.Update(id, patient);

            return Ok();
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = _patientService.Get(id);
            if (p == null)
                return NotFound();

            await _patientService.Remove(id);

            return Ok();
        }


        [Route("{clientid:length(24)}/openvisit")]
        [HttpGet]
        public async Task<IActionResult> GetOpenVisit(string clientid)
        {
            var p = await _patientService.GetPatientOpenVisitByPatientId(clientid);
            if (p == null)
                return NotFound();

            return Ok(p);
        }


        [HttpPut("updatevisit")]
        public async Task<IActionResult> UpdatePatientVisit(Visit visit)
        {
            await _patientService.UpdatePatientOpenVisit(visit);
            _ = _hubContext.Clients.All.SendAsync("ReceiveNewUpdatedVisit", await _patientService.GetPatientOpenVisitByPatientId(visit.PatientId));


            return Ok();
        }
    }
}
