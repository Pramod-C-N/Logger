using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleCRUDwebAPI.DAL;
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

        private readonly MyAppDbContext _appDbContext;
        public LoginController(MyAppDbContext appDbContext,IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;

        }


        private Users AuthenticateUser(Users user)
        {
            Users _user = null;

           
            var dbUser = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (dbUser != null && dbUser.Password == user.Password)
            {
                // Username exists in the database and passwords match
                _user = new Users { Username = dbUser.Username };
            }

            return _user;
        }

        //private Users AuthenticateUser(Users user)
        //{
        //    Users _user = null;
        //    if (user.Username == "admin" && user.Password=="12345")
        //    {   
        //        _user= new Users { Username = "Pramod"};
        //    }
        //    return _user;
        //}

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
