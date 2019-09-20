using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDB;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagaluApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<MongoIdentityUser> _userManager;
        private readonly SignInManager<MongoIdentityUser> _signInManager;

        public AdminController(UserManager<MongoIdentityUser> userManager, SignInManager<MongoIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("App is running");
        }

        [AllowAnonymous]
        [HttpPost("register")]       
        public async Task<IActionResult> Register([FromBody]UserDTO userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new MongoIdentityUser(userDto.Username, userDto.Username);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }

            // If we got this far, something failed, redisplay form
            return Ok("Registrado com sucesso");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDTO userDto)
        {
            //var user = ""; //_adminService.Login(userDto.Username, userDto.Password);           

            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });
            //else
            //    await _signInManager.SignInAsync(null, isPersistent: false);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.Id.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);

            //// return basic user info (without password) and token to store client side
            //return Ok(new
            //{
            //    Id = user.Id,
            //    Username = user.Username,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Token = tokenString
            //});
            return Ok("");
        }
    }
}
