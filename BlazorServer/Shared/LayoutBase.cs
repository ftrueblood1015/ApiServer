using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Shared
{
    public class LayoutBase : LayoutComponentBase
    {
        public AuthenticatedUser? AuthenticatedUser { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
