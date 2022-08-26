using Microsoft.AspNetCore.Identity;

namespace Blazor.WebAssembly.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
