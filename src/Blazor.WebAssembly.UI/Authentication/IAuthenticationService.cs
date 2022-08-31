using Blazor.WebAssembly.Core.Models;

namespace Blazor.WebAssembly.UI.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> Login(LoginModel userForAuthentication);
        Task Logout();
    }
}