using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.WebAsembly.Services.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountsController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            var result= await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
