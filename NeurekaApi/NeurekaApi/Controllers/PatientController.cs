using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeurekaApi.Dtos;
using NeurekaDAL.Models;
using NeurekaService.Services;

namespace NeurekaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IVisitService _visitService;
        private readonly IMapper _mapper;
        public PatientController(IPatientService patientService, IVisitService visitService, IMapper mapper)
        {
            _patientService = patientService;
            _visitService = visitService;
            _mapper = mapper;
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

        [Route("visits")]
        [HttpPost]
        public async Task<IActionResult> CreatePatientVisit(VisitDto  visitDto)
        {

            var visit = _mapper.Map<Visit>(visitDto);
            await _patientService.CreatePatientVisit(visit);
            return Ok(CreatedAtRoute("Visit", new { id = visitDto.Id.ToString(), visitDto }));
        }

        [HttpPut("visits/{id}")]
        public async Task<IActionResult> UpdatePatientVisit(string id, Visit Visit)
        {
            var p = _patientService.GetPatientVisit(id);
            if (p == null)
                return NotFound();

            await _patientService.UpdatePatientVisit(id, Visit);
            return Ok();
        }

        [HttpDelete("visits/{id}")]
        public async Task<IActionResult> DeletePatientVisit(string id)
        {
            var p = _patientService.GetPatientVisit(id);
            if (p == null)
                return NotFound();

            await _patientService.RemovePatientVisit(id);

            return Ok();
        }

        [HttpGet("{d}/visits")]
        public async Task<IActionResult> GetVisits(string id)
        {
            var p = await _patientService.GetPatientVisitByPatientId(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [HttpGet("visits/{id}")]
        public async Task<IActionResult> GetVisit(string visitid)
        {
            var p = await _patientService.GetPatientVisit(visitid);
            if (p == null)
                return NotFound();

            return Ok(p);
        }
    }
}
