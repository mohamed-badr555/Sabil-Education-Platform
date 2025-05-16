using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Course :BaseEntity
    {
       
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Level is required.")]
        public string Level { get; set; }   // Consider converting it to enum
        //public int N_Lessons { get; set; }
        [MaxLength(500)]
        public string? Path { get; set; }   // Path of the course videos directory
        public float Price { get; set; }
        public bool IsFree { get; set; } = false;
        [Range(1, 5)]
        public int Rating { get; set; } = 2;
        [MaxLength(2000)]
        public string Description { get; set; }
        [MaxLength(1500)]
        public string Details { get; set; }
        [MaxLength(50)]
        public TimeSpan Duration { get; set; }
        public DateTime Last_Update { get; set; }
        //public bool IsPublished { get; set; }
        public string? Intro_Video { get; set; }   //  
        //[MaxLength(300)]
        //public int Num_Units { get; set; }
        [MaxLength(300)]
        public string ThumbnailUrl { get; set; }    // New !!!

   

        [MaxLength(200, ErrorMessage = "Tags must not exceed 200 characters.")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Course type is required.")]
        public int CourseType { get; set; }


        public int StudentCount { get; set; }





        #region Category - Course (1-M)
        [Required(ErrorMessage = "Category is required.")]
        public string? CategoryID { get; set; } //Foreign key
        public Category Category { get; set; }
        #endregion

        #region Course - CourseUnit (1-M)
        public ICollection<CourseUnit> CourseUnits { get; set; }
        #endregion

        #region Course - Account (M-M)
        public ICollection<CourseAccount> CourseAccounts { get; set; }


        #endregion
    }
}
