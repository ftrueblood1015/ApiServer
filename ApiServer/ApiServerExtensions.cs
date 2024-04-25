using Domain.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace ApiServer
{
    public static class ApiServerExtensions
    {
        public static AuthenticationBuilder ConfigureProgramAuthentication(this AuthenticationBuilder authBuilder, ConfigOptions options)
        {

            var validationParams = options?.Token.CreateValidationParameters()!;

            authBuilder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParams;
            });

            return authBuilder;
        }
    }
}
