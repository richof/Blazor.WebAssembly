using AutoMapper;
using Blazor.WebAsembly.Services.Configurations;
using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blazor.WebAsembly.Services.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenSettings _tokenSettings;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IOptions<TokenSettings> tokenSettings,
                                    IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenSettings = tokenSettings.Value;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //TODO: Add user to Role
                await _userManager.AddToRoleAsync(user, "visitor");
                var token = await GenerateToken(user);
                return Ok(token);
            }

            return BadRequest(result);
        }
        private async Task<LoginResponseModel> GenerateToken(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            IList<Claim> roleClaims = new List<Claim>();
            roleClaims = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(userRoles.FirstOrDefault()));
            foreach (var roleClaim in roleClaims)
            {
                claims.Add(roleClaim);
            }
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            var identityClaims = new ClaimsIdentity(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            var exp=DateTime.UtcNow.AddHours(_tokenSettings.ExpiresIn);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Subject = identityClaims,
                Expires = exp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            var encodedToken = tokenHandler.WriteToken(token);
            var result = new LoginResponseModel
            {
                Token = encodedToken,
                ExpiresIn = TimeSpan.FromMinutes(_tokenSettings.ExpiresIn).TotalSeconds,
                Claims = _mapper.Map<IEnumerable<Claim>, IEnumerable<ClaimModel>>(claims).ToList()
            };
            return result;
        }
        private object ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
