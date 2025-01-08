
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmplyoeeContext _context;
        private readonly IConfiguration configuration;


        public AuthController(EmplyoeeContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginDto)
        {
            var user = await _context.TblEmployee.FirstOrDefaultAsync(x => x.Email == loginDto.email);
       

            if (user==null || !BCrypt.Net.BCrypt.Verify(loginDto.password, user.password))
            {
                return Unauthorized(new { success = false, message = "Invalid email or password." });
            }


      
            //Calling method for creating token if authenticated up!!
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                success = true,
                token,
                user
            });
        }

        [HttpPost("generate-jwt-token")]  //keep this in mind we have to apply HttpPost  to that method also
        public string GenerateJwtToken(TblEmployee user)
        {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("userId", user.Id.ToString()),
        new Claim("userName", user.Name),
        new Claim("email", user.Email),
       
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
