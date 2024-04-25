using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Application
{
    public class User : TrackableBase
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? DisplayName { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
