using ApiServer;
using Domain.Configuration;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// bind or config section
ConfigOptions _configurationOptions = new();

var optionsSection = builder.Configuration.GetSection(nameof(ConfigOptions));

optionsSection.Bind(_configurationOptions);
_configurationOptions.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Services.AddSingleton(_configurationOptions);
builder.Services.AddHttpContextAccessor();

// Db set up
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("ApiServer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the ApiServer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiServer",
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiServer",
                },
            },
            Array.Empty<string>()
        },
    });
});

// Dependency Injection
InjectPatternFromAssemblies(builder, "Service");
InjectPatternFromAssemblies(builder, "Repository");

// Authentication, Authorization, and Cors
ConfigureAuthentication(builder, _configurationOptions);
ConfigureAuthorization(builder);
AddCorsPolicy(builder, _configurationOptions);

var app = builder.Build();

await EnsureDbIsMigrated(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task EnsureDbIsMigrated(IServiceProvider services)
{
    using var scope = services.CreateScope();
    using var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
    if (ctx != null)
    {
        await ctx.Database.MigrateAsync();
    }
}

void InjectPatternFromAssemblies(WebApplicationBuilder builder, string pattern, params Assembly[] assembly)
{
    builder.Services.RegisterAssemblyPublicNonGenericClasses(GetAssemblies())
            .Where(c => c.Name.EndsWith(pattern, StringComparison.CurrentCultureIgnoreCase))
            .AsPublicImplementedInterfaces();
}

static Assembly[] GetAssemblies()
{
    var api = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.Contains("Api")).ToList();

    api.Add(Assembly.Load("Infrastructure"));
    api.Add(Assembly.Load("Services"));
    api.Add(Assembly.Load("Domain"));

    return api.ToArray();
}

static void ConfigureAuthorization(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();
    });
}

static void ConfigureAuthentication(WebApplicationBuilder builder, ConfigOptions configurationOptions)
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .ConfigureProgramAuthentication(configurationOptions);
}

static void AddCorsPolicy(WebApplicationBuilder builder, ConfigOptions options)
{
    builder.Services.AddCors(corsOptions =>
    {
        corsOptions.AddPolicy(options.CorsPolicy,
            corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
    });
}
