using Blazored.LocalStorage;
using Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace BlazorServer.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly ApiServerClient _apiServerClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;


        public AuthenticationService(ApiServerClient apiServerClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _apiServerClient = apiServerClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthenticatedUser> AuthenticateUser(UserLogin userLogin)
        {
            var authCodeUri = new Uri($"Authentication/Login", UriKind.Relative);

            var authenticatedUser = await _apiServerClient.PostAsJson<AuthenticatedUser, UserLogin>(authCodeUri, userLogin);

            return await StoreToken(authenticatedUser);
        }

        public async Task Logout()
        {
            // clear storage
            await _localStorage.RemoveItemAsync("AccessToken");
            await _localStorage.RemoveItemAsync("Roles");
            await _localStorage.RemoveItemAsync("Username");

            // remove token param from header 
            _apiServerClient.Client.DefaultRequestHeaders.Authorization = null;

            // update authState
            ((AuthStatHelper)_authStateProvider).NotifyUserLogout();
        }

        private async Task<AuthenticatedUser> StoreToken(AuthenticatedUser authenticatedUser)
        {
            // Store needed info
            await _localStorage.SetItemAsync("AccessToken", authenticatedUser.AccessToken);
            await _localStorage.SetItemAsync("Roles", authenticatedUser.Roles);
            await _localStorage.SetItemAsync("Username", authenticatedUser.Username);

            // Add token as parameter to headers
            _apiServerClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticatedUser?.AccessToken);

            // Update the authState
            ((AuthStatHelper)_authStateProvider).NotifyUserAuthentication(authenticatedUser?.AccessToken ?? string.Empty);

            return authenticatedUser!;
        }
    }
}
