using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class EntityBase
    {
        [Required]
        public Guid Id { get; set; }
    }
}
