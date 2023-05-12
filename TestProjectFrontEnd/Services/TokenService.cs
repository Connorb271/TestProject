using Microsoft.JSInterop;
using System.Net.Http.Json;
using TestProjectFrontEnd.Models;

namespace TestProjectFrontEnd.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(LoginModel loginModel);
        Task<string> GetStoredTokenAsync();
        Task StoreTokenAsync(string token);
        Task RemoveTokenAsync();
        Task<bool> IsLoggedInAsync();
    }

    public class TokenService : ITokenService
    {
        private const string TokenStorageKey = "token";
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public TokenService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetTokenAsync(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginModel);
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();
                await StoreTokenAsync(token);
                return token;
            }
            catch (HttpRequestException e)
            {
                // Handle exception or return null in case of error
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<string> GetStoredTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenStorageKey);
        }

        public async Task StoreTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenStorageKey, token);
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenStorageKey);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetStoredTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
