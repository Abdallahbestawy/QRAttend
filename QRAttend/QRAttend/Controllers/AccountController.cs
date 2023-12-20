using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QRAttend.Dto;
using QRAttend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;


        public AccountController(UserManager<ApplicationUser> _userManager,IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }
        //[Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterUserDto userDto ) 
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userDto.UserName;
                user.Email = userDto.Email;
                IdentityResult result=await userManager.CreateAsync(user,userDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Add Succeeded");
                }
                return BadRequest(result.Errors.FirstOrDefault());  
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user=await userManager.FindByNameAsync(userDto.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, userDto.Password);
                    if (found)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                        SigningCredentials signing =
                            new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken Token = new JwtSecurityToken(
                            issuer: configuration["JWT:ValidIssuer"],
                            audience: configuration["JWT:ValidAudience"],
                            claims: claims,
                            expires:DateTime.Now.AddHours(3),
                            signingCredentials: signing
                            );
                        return Ok(new 
                        {
                            token=new JwtSecurityTokenHandler().WriteToken(Token),
                            expires= Token.ValidTo

                        }); 
                    }
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }
}
