using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Application
{
    public class UserRole : EntityBase
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        public User? User { get; set; }

        public Role? Role { get; set; }
    }
}
