﻿
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class AppLoginDto
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AppRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,20}$", ErrorMessage = "Password must be complex ~!")]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }
    }

    public class UserDto
    {
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }

    }
}