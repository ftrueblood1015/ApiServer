using Blazored.LocalStorage;
using Domain.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorServer.Services.Authentication
{
    public class AuthStatHelper : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous;
        private readonly ApiServerClient _apiServerClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStatHelper(ApiServerClient apiServerClient, ILocalStorageService localStorage)
        {
            _apiServerClient = apiServerClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = string.Empty;

            try
            {
                token = await _localStorage.GetItemAsync<string>("AccessToken");
            }
            catch
            {
                token = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            _apiServerClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            ClaimsPrincipal claimsPrincipal = CreateClaims(token);

            return new AuthenticationState(claimsPrincipal);
        }

        public void NotifyUserAuthentication(string token)
        {

            ClaimsPrincipal claimsPrincipal = CreateClaims(token);

            var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

        private ClaimsPrincipal CreateClaims(string token)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaims(token), "jwtAuthType"));
        }
    }
}
