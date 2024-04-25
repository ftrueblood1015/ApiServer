using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrackableBase : EntityBase
    {
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set;}
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
