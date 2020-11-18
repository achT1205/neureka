using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeurekaApi.Dtos;
using NeurekaDAL.Models;
using NeurekaService.Services;

namespace NeurekaApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.Get();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [Route("{role}", Name = "GetUsersByRole")]
        [HttpGet]
        public async Task<IActionResult> GetUserByRole(string role)
        {
            var users = await _userService.GetUsersByRole(role);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [Route("{id:length(24)}", Name = "GetUser")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var p = await _userService.Get(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _userService.Create(user);

            return Ok(CreatedAtRoute("user", new { id = user.Id.ToString(), user }));
        }

        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var p = _userService.Get(id);
            if (p == null)
                return NotFound();

            await _userService.Update(id, user);

            return Ok();
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = _userService.Get(id);
            if (p == null)
                return NotFound();

            await _userService.Remove(id);

            return Ok();
        }


    }
}
