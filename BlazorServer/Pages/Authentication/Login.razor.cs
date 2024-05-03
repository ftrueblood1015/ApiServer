using BlazorServer.Services.Authentication;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorServer.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        IAuthenticationService? AuthenticationService { get; set; }

        [Inject]
        NavigationManager? NavigationManager { get; set; }

        private UserLogin? UserLogin { get; set; }

        private AuthenticatedUser? AuthenticatedUser { get; set; }

        public bool success;

        public MudForm? Form;

        public bool isShow;

        public InputType PasswordInput = InputType.Password;

        public string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        public bool successfulLogin = true;

        protected override async Task OnInitializedAsync()
        {
            UserLogin = new UserLogin();
            AuthenticatedUser = new AuthenticatedUser();
        }

        void ShowPassword()
        { 
            if(isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        public async void LoginUser()
        {
            if (AuthenticationService == null)
            {
                throw new ArgumentNullException(nameof(AuthenticationService));
            }

            await Form!.Validate();

            if (Form!.IsValid)
            {
                AuthenticatedUser = (await AuthenticationService.AuthenticateUser(UserLogin!));
                successfulLogin = AuthenticatedUser.ApiMessage!.Contains("Success");
                StateHasChanged();
                if(successfulLogin)
                {
                    NavigateToDashboard();
                }
            }
        }

        public void NavigateToDashboard()
        {
            if (NavigationManager == null)
            {
                return;
            }
            NavigationManager.NavigateTo("/");
        }

        public void RegisterNewUser()
        {

        }
    }
}
