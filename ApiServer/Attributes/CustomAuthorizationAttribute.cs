using Domain.Configuration;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.Versioning;
using System.Security.Claims;

namespace ApiServer.Attributes
{
    [SupportedOSPlatform("windows")]
    public class CustomAuthorizationAttribute : IAuthorizationFilter
    {

        private readonly Claim _claim;

        public CustomAuthorizationAttribute(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                context!.Result = new UnauthorizedResult();
            }

            var auth = context.HttpContext.Request.Headers["Authorization"];

            var data = auth.ToString().Split(" ".ToArray());

            if (data?.Length == 2)
            {
                try
                {
                    var validationParameters = Constants.ValidationParameters[data[0]];

                    if (!JwtParser.IsValid(data[1]))
                    {
                        context.Result = new UnauthorizedResult();
                    }

                    var userRoles = JwtParser.GetValidatedUserRoles(data[1], validationParameters);

                    if (userRoles == null || !userRoles.Contains(_claim.Value))
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                catch
                {
                    // invalid token
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                // invalid header
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
