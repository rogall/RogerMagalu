using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using Helpers;
using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace MagaluApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private IAdminService _adminService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(
            IAdminService adminService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _adminService = adminService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("App is running");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]UserDTO userDto)
        {
            var user = _adminService.Login(userDto.UserName, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDTO userDto)
        {            
            var user = _mapper.Map<User>(userDto);

            try
            {
                //var result = await _userManager.CreateAsync(user, userDto.Password);
                //if (result.Succeeded)
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //}

                _adminService.CreateUser(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _adminService.GetAllUsers();
            var userDtos = _mapper.Map<IList<UserDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _adminService.GetUserById(id);
            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]UserDTO userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save 
                _adminService.UpdateUser(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _adminService.DeleteUser(id);
            return Ok();
        }
    }
}