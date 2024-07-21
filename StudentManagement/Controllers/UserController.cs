using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Models;
using StudentManagement.Repository.IRepository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserController(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }
                if (!_userRepository.IsUniqueUser(admin.Email))
                {
                    return BadRequest("User is already in use.");
                }
                var registeredAdmin = _userRepository.Register(admin);
                if (registeredAdmin != null)
                {
                    return Ok(registeredAdmin);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to register user.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering user.");
            }
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginVM loginVM)
        {
            try
            {
                var user = _userRepository.Authenticate(loginVM.Email, loginVM.Password);
                if (user == null)
                {
                    var teacher = _userRepository.AuthenticateTeacher(loginVM.Email, loginVM.Password);
                    if (teacher == null)
                    {
                        return BadRequest("Authentication failed.");
                    }

                    return Ok(GenerateJwtToken(teacher.TeacherId.ToString(), teacher.Role));
                }

                return Ok(GenerateJwtToken(user.AdminId.ToString(), user.Role));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during authentication.");
            }
        }

        private object GenerateJwtToken(string userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new { Token = tokenString };
        }
    }
}
