using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ApiMagalu.Settings;
using AutoMapper;
using DTOs;
using Entities;
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
        private readonly MagaluDbContext _context;
        private readonly UserManager<User> _userManager;
        private IMapper _mapper;

        public AdminController(
            MagaluDbContext context,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        } 
        
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Post(
            [FromBody]UserDTO userDto,
            [FromServices]UserManager<User> userManager,
            [FromServices]SignInManager<User> signInManager,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;
            if (userDto != null)
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = userManager.Users.FirstOrDefault(_ => _.Email == userDto.Email);
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = signInManager
                        .CheckPasswordSignInAsync(userIdentity, userDto.Password, false)
                        .Result;
                    userDto.Id = userIdentity.Id;
                    credenciaisValidas = resultadoLogin.Succeeded;
                }
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(userDto.Id, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Id)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = DateTime.UtcNow.AddDays(7);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });

                var token = handler.WriteToken(securityToken);

                return Ok(new
                {
                    Id = userDto.Id,
                    Username = userDto.UserName,
                    Email = userDto.Email,
                    Token = token,
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    message = "OK"
                });
            }
            else
            {
                return Ok(new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                });
            }
        }    

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDTO userDto)
        {            
            var user = _mapper.Map<User>(userDto);

            try
            {
                CreateUser(user, userDto.Password);
                return NoContent();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        private void CreateUser(User user, string password)
        {
            if (_userManager.FindByEmailAsync(user.Email).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (!resultado.Succeeded)
                {
                    string erro = "";
                    foreach (var item in resultado.Errors)
                    {
                        erro += item.Description + " - ";
                    }
                    throw new Exception(erro);
                }                    
            }
        }
    }
}

