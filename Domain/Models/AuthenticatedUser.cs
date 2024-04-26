using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AuthenticatedUser
    {
        public string? AccessToken { get; set; }
        public string? ApiMessage { get; set; } = string.Empty;
        public IEnumerable<string>? Roles { get; set; }
        public string? Username { get; set; }
    }
}
