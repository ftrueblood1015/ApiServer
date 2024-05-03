using BlazorServer.Data;
using Blazored.LocalStorage;
using MudBlazor;
using MudBlazor.Services;
using NetCore.AutoRegisterDi;
using System.Reflection;
using Domain.Models;
using BlazorServer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorServer.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json").AddEnvironmentVariables().Build();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddScoped<AuthenticationStateProvider, AuthStatHelper>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(config.GetValue<string>("AppBaseUrl")!) });

builder.Services.AddScoped(sp => new ApiServerClient(new HttpClient { BaseAddress = new Uri(config.GetValue<string>("ApiServerBaseUrl")!) }));

builder.Services.AddScoped<AuthenticatedUser>();

InjectPatternFromAssemblies(builder, "Service");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

static void InjectPatternFromAssemblies(WebApplicationBuilder builder, string pattern, params Assembly[] assembly)
{
    builder.Services.RegisterAssemblyPublicNonGenericClasses(assembly)
         .Where(c => c.Name.EndsWith(pattern, StringComparison.CurrentCultureIgnoreCase))
         .AsPublicImplementedInterfaces();
}
