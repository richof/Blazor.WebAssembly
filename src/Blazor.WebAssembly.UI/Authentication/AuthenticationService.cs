using Blazor.WebAssembly.Core.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Blazor.WebAssembly.UI.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient client,
                                     AuthenticationStateProvider authStateProvider,
                                     ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }
        public async Task<LoginResponseModel> Login(LoginModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                //new KeyValuePair<string, string>("content-type","application/json"),
                new KeyValuePair<string, string>("grant_type","password"),  
                new KeyValuePair<string, string>("userName",userForAuthentication.Email),
                new KeyValuePair<string, string>("password",userForAuthentication.Password)
            });
            var authResult = await _client.PostAsync($"{_client.BaseAddress}/accounts/token", data);
            var authContent = await authResult.Content.ReadAsStringAsync();
            if (!authResult.IsSuccessStatusCode)
            {
                return null;
            }
            var result = JsonSerializer.Deserialize<LoginResponseModel>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await _localStorage.SetItemAsync("authToken", result.AccessToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            return result;
        }
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
