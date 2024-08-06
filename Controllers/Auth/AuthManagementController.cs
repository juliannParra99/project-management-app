using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementApp.Configurations;
using ProjectManagementApp.Models.DTOs;

namespace ProjectManagementApp.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<AuthManagementController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;


        public AuthManagementController(ILogger<AuthManagementController> logger, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        // user registration
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            //validate request
            if (ModelState.IsValid)
            {
                //check if email exist 
                var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);

                if (emailExist != null)
                {
                    return BadRequest("Email already exist;");
                }

                // Create new user
                var newUser = new IdentityUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);

                ////return success respones if was succeded
                if (isCreated.Succeeded)
                {
                    //generate token
                    var token = GenerateJwtToken(newUser);

                    return Ok(new RegistrationRequestResponse()
                    {
                        Result = true,
                        Token = token

                    });
                }

                return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());

            }

            return BadRequest("Invalid request payload");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);

                if (existingUser == null)
                {
                    return BadRequest("Invalid authentication");
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);

                if (isPasswordValid)
                {
                    var Token = GenerateJwtToken(existingUser);
                    return Ok(new LoginRequestResponse()
                    {
                        Token = Token,
                        Result = true

                    });

                }

                return BadRequest("Invalid authentication");
            }

            return BadRequest("Invalid request payload");
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            //setting the generation of the token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }


    }
}