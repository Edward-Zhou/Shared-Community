using SharedCommunity.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models.Entities
{
    public abstract class EntityBase: IObjectState
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}
