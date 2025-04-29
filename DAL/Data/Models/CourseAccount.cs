using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class CourseAccount
    {
        //NOOOOOOOOOTES 
        public int CourseID { get; set; } //foreign key
        public Course courses { get; set; }

        public int AccountID { get; set; }//foreign key
        public ApplicationUser User { get; set; }

        public int FinishedUnits { get; set; }
        public bool IsCompleted { get; set; }
        public TimeSpan CompTime { get; set; }
        public DateTime StartDate { get; set; }
        [Range(0, 100)]
        public int Progress { get; set; }  // In percentage
        [MaxLength(500)]

        public string LastVideo { get; set; }   // Revise here
        [MaxLength(800)]
        public string? RateText { get; set; }
        [Range(1, 5)]
        public int RateStars { get; set; }
    }
}
