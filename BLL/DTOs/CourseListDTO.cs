using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class CourseListDTO
    {
        // Changed from int to string to match the Course entity
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        //public int N_Lessons { get; set; }

        [MaxLength(50)]
        public string Level { get; set; }

        public float Price { get; set; }

        public string Category { get; set; }

        [MaxLength(300)]
        public string ThumbnailUrl { get; set; }

        public  string CategoryName { get; set; }
    }


}