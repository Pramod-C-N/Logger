using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCRUDwebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SimpleCRUDwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        private Users AuthenticateUser(Users user)
        {
            Users _user = null;
            if (user.Username == "admin" && user.Password=="12345")
            {   
                _user= new Users { Username = "Pramod"};
            }
            return _user;
        }

        private string GenerateToken(Users users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],_configuration["Jwt:Audience"],null,
                expires : DateTime.Now.AddMinutes(1),
                signingCredentials : credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users users)
        {
            IActionResult response = Unauthorized();
            var user_  = AuthenticateUser(users);
            if (user_ != null)
            {
                var token = GenerateToken(users);
                response = Ok(new {token = token});
            }
            return response;
        }
    }
}
