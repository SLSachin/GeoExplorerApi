using GeoExplorerApi.Dtos;
using GeoExplorerApi.Interfaces;
using GeoExplorerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GeoExplorerApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly GeoExplorerContext _context;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration config, GeoExplorerContext context, IAuthService authService)
        {
            _configuration = config;
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login(UserDto _userDto)
        {
            if (_userDto != null && _userDto.Username != null && _userDto.Password != null)
            {
                var user = await _authService.GetUser(_userDto.Username, _userDto.Password);

                if (user != null)
                {
                    var token = _authService.GenerateJwtToken(user);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register(UserDto newUserDto)
        {
            if (newUserDto != null && !string.IsNullOrEmpty(newUserDto.Username) && !string.IsNullOrEmpty(newUserDto.Password))
            {
                // Check if the user already exists
                var existingUser = await _authService.GetUser(newUserDto.Username, newUserDto.Password);
                if (existingUser != null)
                {
                    return BadRequest("User already exists");
                }

                // Create a new user
                var newUser = new User
                {
                    Username = newUserDto.Username,
                    Password = newUserDto.Password,
                    Role = "user"
                };

                // Add the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Return a success message or token, depending on your requirements
                var token = _authService.GenerateJwtToken(newUser);
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid registration data");
            }
        }
    }
}