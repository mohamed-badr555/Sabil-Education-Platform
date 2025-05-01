using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Category :BaseEntity
    {
       
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
