using Blazor.WebAssembly.Core.Models;
using System.Security.Claims;

namespace Blazor.WebAssembly.UI.Services
{
    public interface IAccountServices
    {
        Task<bool> IsVisitor(ClaimsPrincipal user);
        Task<ResponseModel> Login(LoginModel model);
        Task<ResponseModel> Register(RegisterModel model);
    }
}