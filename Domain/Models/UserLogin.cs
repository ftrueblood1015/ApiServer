using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserLogin
    {
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Username { get; set; }
    }
}
