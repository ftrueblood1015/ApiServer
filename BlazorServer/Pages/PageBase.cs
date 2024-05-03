using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorServer.Shared;
using MudBlazor;
using BlazorServer.Services.Authentication;

namespace BlazorServer.Pages
{
    public class PageBase : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = new();

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        public IDialogService? DialogService { get; set; }

        [Inject]
        public ISnackbar? SnackbarService { get; set; }

        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        public bool ShowAlertMessage { get; internal set; }

        public Severity AlertSeverity { get; internal set; } = Severity.Info;

        public string AlertMessage { get; internal set; } = string.Empty;

        public PageBase Page
        {
            get
            {
                return this;
            }
        }

        public void ClearAlertMessage()
        {
            ShowAlertMessage = false;
            AlertSeverity = Severity.Info;
            AlertMessage = string.Empty;
        }
    }
}
