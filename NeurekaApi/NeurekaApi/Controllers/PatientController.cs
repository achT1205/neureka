using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
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
    }
}
