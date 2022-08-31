using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.Models;

using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Blazor.WebAssembly.UI.Services
{
    public class AccountServices : BaseService, IAccountServices
    {
        private readonly HttpClient _httpClient;

        public AccountServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseModel> Login(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/accounts/login", model);
            return await MapResponse(response);
        }
        public async Task<ResponseModel> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/accounts/register", model);

            return await MapResponse(response);
        }
        public async Task<bool> IsVisitor(ClaimsPrincipal user)
        {
            var result= user.IsInRole("visitor");
            return result;

        }
    }
}
