using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class CourseAccount : BaseEntity
    {
        //NOOOOOOOOOTES 
        public string CourseID { get; set; } //foreign key
        public Course course { get; set; }

        public string UserId { get; set; }//foreign key
        public ApplicationUser User { get; set; }

        public int FinishedUnits { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public TimeSpan CompTime { get; set; } = TimeSpan.Zero;
        public DateTime StartDate { get; set; }
        [Range(0, 100)]
        public int Progress { get; set; } = 0;
        [MaxLength(500)]

        public string? LastVideo { get; set; }   // Revise here
        [MaxLength(800)]
        public string? RateText { get; set; }
        [Range(1, 5)]
        public int? RateStars { get; set; }
    }
}
