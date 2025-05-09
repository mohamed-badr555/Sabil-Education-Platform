using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DAL.Data.Models
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Generate a new GUID by default
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public string UpdatedBy { get; set; }
        //public DateTime? UpdatedAt { get; set; }

    }
}
