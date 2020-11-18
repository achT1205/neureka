using System;
namespace NeurekaApi.Dtos
{
    public class AuthUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ChangePassDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
