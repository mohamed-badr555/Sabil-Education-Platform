using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class CourseListDTO
    {

        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } // Example: "تعلم النحو من الصفر حتى الإحتراف"

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // Example: 4 / 3

        public int N_Lessons { get; set; } // Example: 11

        [MaxLength(50)]
        public string Level { get; set; } // Example: "المبتدئ" or "المتقدم"
        public float Price { get; set; }
        public string CategoryName { get; set; }


        [MaxLength(300)]
        public string ThumbnailUrl { get; set; } // Course image path

        // For navigation
        //public Dictionary<string, string> Links { get; set; } = new()
        //{
        //    ["self"] = string.Empty // Will be populated with course URL like "/api/courses/123"
        //};

    }


}