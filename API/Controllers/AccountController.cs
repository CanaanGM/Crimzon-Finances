using API.Services;

using Application.DTOs;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(AppLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            return result.Succeeded
            ? CreateUserObject(user)
            : Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(AppRegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email)) return BadRequest("Email Taken!");
            if (await _userManager.Users.AnyAsync(x => x.DisplayName == registerDto.DisplayName)) return BadRequest("Display Name Taken!");
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName)) return BadRequest("User Name Taken!");

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            return result.Succeeded
            ? CreateUserObject(user)
            : BadRequest("Problem registering user . . . ");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        private UserDto CreateUserObject(AppUser user) => new UserDto
        {
            DisplayName = user.DisplayName,
            UserName = user.UserName,
            Token = _token.CreateToken(user),
            Image = string.Empty
        };

    }
}
