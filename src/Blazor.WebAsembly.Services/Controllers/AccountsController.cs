using AutoMapper;
using Blazor.WebAsembly.Services.Configurations;
using Blazor.WebAsembly.Services.IdentityData;
using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.Models;
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
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenSettings _tokenSettings;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IOptions<TokenSettings> tokenSettings,
                                    IMapper mapper,
                                    SignInManager<ApplicationUser> signInManager,
                                    ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenSettings = tokenSettings.Value;
            _mapper = mapper;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("register")]
        
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
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
                var token = await GenerateTokenAsync(user.UserName);
                return CustomResponse(token);
            }
            else
            {
                foreach (var error in result.Errors)
                    AddError(error.Code,error.Description);
            }
            
            return CustomResponse(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            //if (!ModelState.IsValid) return BadRequest(model);
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user is null) {
                AddError("UserName/Password", "Invalid User Name or Password");
                return CustomResponse();
            }
            var login = await _signInManager.PasswordSignInAsync(user, model.Password,false,false);
            if(login.Succeeded)
            {
                var token = await GenerateTokenAsync(user);
                return CustomResponse(token);
            }
            AddError("UserName/Password", "Invalid User Name or Password");
            return CustomResponse();

        }
        private async Task<LoginResponseModel> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles =  await _userManager.GetRolesAsync(user);
            IList<Claim> roleClaims = new List<Claim>();
            roleClaims = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(userRoles.FirstOrDefault()));
            //foreach (var roleClaim in roleClaims)
            //{
            //    claims.Add(roleClaim);
            //}
            var identityOptions = new IdentityOptions();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(identityOptions.ClaimsIdentity.RoleClaimType, role));
            }
            var identityClaims = new ClaimsIdentity(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var exp=DateTime.UtcNow.AddHours(_tokenSettings.ExpiresIn);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Subject = identityClaims,
                Expires = exp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            });
            var encodedToken = tokenHandler.WriteToken(token);
            var result = new LoginResponseModel
            {
                AccessToken = encodedToken,
                UserName=user.UserName
                //ExpiresIn = TimeSpan.FromMinutes(_tokenSettings.ExpiresIn).TotalSeconds,
                //Claims = _mapper.Map<IEnumerable<Claim>, IEnumerable<ClaimModel>>(claims).ToList()
            };
            return result;
        }
        private object ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        [Route("token")]
        [HttpPost]
      
        public async Task<IActionResult> Create([FromForm]string userName, [FromForm] string password, [FromForm] string grant_type)
        {
            if(await IsValidUserNameAndPassword(userName,password))
            {
                return CustomResponse(await GenerateTokenAsync(userName));
            }
            else
            {
                AddError("UserName-Password", "Invalid UserName/Password");
                return CustomResponse();
            }
        }
        private async Task<bool> IsValidUserNameAndPassword(string userName,string password)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            return await _userManager.CheckPasswordAsync(user, password);
        }
        private async Task<dynamic> GenerateTokenAsync(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new
                        {
                            ur.UserId,
                            ur.RoleId,
                            r.Name
                        };
            int exp = Convert.ToInt32(_tokenSettings.ExpiresIn);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf,new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,new DateTimeOffset(DateTime.Now.AddHours(exp)).ToUnixTimeSeconds().ToString())
            };
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));
            var result = new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.UserName
            };
            return result;
        }

    }
}
