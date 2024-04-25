using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class NameDescriptionBase : TrackableBase
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
