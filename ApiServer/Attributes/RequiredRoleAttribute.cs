using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiServer.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredRoleAttribute : TypeFilterAttribute
    {
        public RequiredRoleAttribute(string role) : base(typeof(CustomAuthorizationAttribute))
        {
            Arguments = new object[] { new Claim("role", role) };
        }
    }
}
