using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Generate a new GUID by default
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
