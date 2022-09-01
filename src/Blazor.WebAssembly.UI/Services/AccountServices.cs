using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.Models;
using Blazor.WebAssembly.UI.Authentication;
using Blazor.WebAssembly.UI.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Blazor.WebAssembly.UI.Services
{
    public class AccountServices : BaseService, IAccountServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AccountServices(HttpClient httpClient,
                               ILocalStorageService localStorage,
                               AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }
        public async Task<ResponseModel> Login(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/accounts/login", model);
            return await MapResponse(response);
        }
        public async Task<ResponseModel> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}/accounts/register", model);

            var result= await MapResponse(response);
            if(!result.Errors.Any())
            {
                var token = JsonSerializer.Deserialize<AccessTokenModel>(result.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive=true});
                await _localStorage.SetItemAsync("authToken", token.AccessToken);
                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(token.AccessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
            }
            return result;
            
        }
        public async Task<bool> IsVisitor(ClaimsPrincipal user)
        {
            var result= user.IsInRole("visitor");
            return result;

        }
    }
}
