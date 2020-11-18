using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeurekaApi.Dtos;
using NeurekaDAL.Models;
using NeurekaService.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace NeurekaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly INeurekaAppSettings _settings;
        public AuthController(IUserService userService, IMapper mapper, INeurekaAppSettings settings)
        {
            _userService = userService;
            _mapper = mapper;
            _settings = settings;
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthUserDto authUser)
        {
            var authenticated = await _userService.Authenticate(authUser.Email, authUser.Password);
            if (authenticated)
            {
                var user = await _userService.GetUserByEmail(authUser.Email);
                if (user != null)
                {
                    var claims = new[]{
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim("firstName", user.FirstName),
                            new Claim("lastName", user.LastName),
                            new Claim("jobTitle", user.JobTitle),
                            new Claim("email", user.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                        };

                    var secreBytes = Encoding.UTF8.GetBytes(_settings.Secret);
                    var key = new SymmetricSecurityKey(secreBytes);
                    var algorithm = SecurityAlgorithms.HmacSha256;
                    var signingCredentials = new SigningCredentials(key, algorithm);
                    var token = new JwtSecurityToken(
                       _settings.Issuer,
                        _settings.Audience,
                        claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddDays(_settings.AccessExpiration),
                        signingCredentials: signingCredentials
                        );

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    user.Password = null;
                    return Ok(new { user = user, AccessToken = jwtToken });
                }
                return null;
            }
            return null;
        }


        [AllowAnonymous]
        [Route("reset-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] AuthUserDto authUser)
        {
            return Ok(await _userService.ResetPassword(authUser.Email));
        }


        [Authorize]
        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> RestPassword([FromBody] ChangePassDto changePassDto)
        {
            var reseted = await _userService.ChangePassword(changePassDto.Email, changePassDto.OldPassword, changePassDto.Password);
            if (reseted)
            {
                var user = await _userService.GetUserByEmail(changePassDto.Email);
                if (user != null)
                {
                    var claims = new[]{
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim("firstName", user.FirstName),
                            new Claim("lastName", user.LastName),
                            new Claim("jobTitle", user.JobTitle),
                            new Claim("email", user.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                        };

                    var secreBytes = Encoding.UTF8.GetBytes(_settings.Secret);
                    var key = new SymmetricSecurityKey(secreBytes);
                    var algorithm = SecurityAlgorithms.HmacSha256;
                    var signingCredentials = new SigningCredentials(key, algorithm);
                    var token = new JwtSecurityToken(
                       _settings.Issuer,
                        _settings.Audience,
                        claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddDays(_settings.AccessExpiration),
                        signingCredentials: signingCredentials
                        );

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    user.Password = null;
                    return Ok(new { user = user, AccessToken = jwtToken });
                }
                return null;
            }
            return null;
        }

    }
}
