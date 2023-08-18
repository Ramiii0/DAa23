using DreamApp.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAUth.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        /*private readonly JwtConfig _jwtConfig;*/
        public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _configuration = config;

            /*_jwtConfig = jwtConfig;*/ 
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var userexist = await _userManager.FindByEmailAsync(registerDto.Email);
                if(userexist != null)
                {
                    return BadRequest("Email Already Exist");
                }
                var new_user = new IdentityUser()
                {
                    Email = registerDto.Email,
                    UserName=registerDto.UserName,
                };
                var create= await _userManager.CreateAsync(new_user,registerDto.Password);
                if (create.Succeeded)
                {
                    var toekn= gneratetoken(new_user);
                    return Ok(toekn);
                }
                return BadRequest("Server Error");

            }
            return BadRequest("ifos not valid");
        }
        private string gneratetoken(IdentityUser user)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var toekndecs = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id",user.Id), 
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                }),
                Expires= DateTime.Now.AddDays(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256 )
                
            }; var token = jwttokenhandler.CreateToken(toekndecs);
            var jwttoken= jwttokenhandler.WriteToken(token);
            return jwttoken;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginrequest)
        {
            if (ModelState.IsValid)
            {
                var finduser = await _userManager.FindByEmailAsync(loginrequest.Email);
                if(finduser == null)
                {
                    return BadRequest("user doesnt exist");
                     }
                var iscorect = await _userManager.CheckPasswordAsync(finduser, loginrequest.Password);
                if (!iscorect)
                {
                    return BadRequest("pass not valid");
                }
                var jwttoken = gneratetoken(finduser);
                return Ok(jwttoken);

            }
            return BadRequest("not valid");

        }

        
    }
}
