using DAL.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LogInDto logInDto)
        {
            var result = await _account.Login(logInDto);

            if (result == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(result);

        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDto registerInDto)
        {
            var result = await _account.Register(registerInDto);

            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);

        }

        public async Task<string> Login(LogInDto logInDto)
        {
            var user = await _userManager.FindByNameAsync(logInDto.UserName);

            if (user == null)
            {
                return null;
            }

            var check = await _userManager.CheckPasswordAsync(user, logInDto.Password);

            if (check == false)
            {
                return null;
            }

            var claims = (await _userManager.GetClaimsAsync(user)).ToList();
            return GenerateToken(claims);
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            ApplicationUser user = new ApplicationUser();
            /*
            Data
            */
            var result = await _userManager.CreateAsync(user, registerDto.Passowrd);
            if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Role", "Adim"));
                claims.Add(new Claim("Name", registerDto.UserName));

                await _userManager.AddClaimsAsync(user, claims);

                string token = GenerateToken(claims);
                return token;
            }
            return null;

        }

        private string GenerateToken(List<Claim> claims)
        {
            var secretKey = _configuration.GetSection("SecretKey").Value;

            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);

            SecurityKey securityKey = new SymmetricSecurityKey(secretKeyByte);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expire = DateTime.UtcNow.AddDays(30);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);
            return token;

        }

        evaluate this code
            }
}
