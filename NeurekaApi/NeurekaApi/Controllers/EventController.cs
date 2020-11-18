using System;
using System.Collections.Generic;
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
    public class EventController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public EventController(IVisitService visitService, IPatientService patientService, IUserService userService, IMapper mapper)
        {
            _visitService = visitService;
            _patientService = patientService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost("GetEventsByDate")]
        public async Task<IActionResult> Get(EventQueryDto eventQueryDto)
        {
            var visits = await _visitService.GetEventsByDate(eventQueryDto.Date, eventQueryDto.DoctorId, eventQueryDto.Type);

            var events = _mapper.Map<IEnumerable<Event>>(visits);

            foreach (var e in events)
            {
                if (e.DoctorId != null)
                {
                    var user = await _userService.Get(e.DoctorId);
                    e.Doctor = user.FirstName + " " + user.LastName;
                }

                if (e.PatientId != null)
                {
                    var partient = await _patientService.Get(e.PatientId);
                    e.Patient = partient.FirstName + " " + partient.LastName;
                }

            }
            return Ok(events);
        }

    }
}
