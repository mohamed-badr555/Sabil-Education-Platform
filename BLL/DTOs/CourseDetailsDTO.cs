using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{

    public class CourseDetailsDTO
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } // e.g., "تعلم النحو من الصفر حتى الإحتراف"

        [Range(1, 5)]
        public int Rating { get; set; } // e.g., 4 / 3

        // Intro Video
        [MaxLength(500)]
        public string Intro_Video { get; set; } // Embedded video URL

        // Key Stats
        //public int N_Lessons { get; set; } // e.g.,  11 lesson
        public TimeSpan Duration { get; set; } // e.g., 8h 30m
        public DateTime Last_Update { get; set; } // e.g., "تم التحديث: ٢٠٢٣-١٠-١٥"
        public string CategoryName { get; set; }

        // Content Sections
        [MaxLength(2000)]
        public string Description { get; set; } // Main overview (1-2 paragraphs)

        [MaxLength(4000)]
        public string Details { get; set; }
        public string Level { get; set; }
        public string Path { get; set; }
        public string ThumbnailUrl { get; set; }

    
        public float Price { get; set; }
        public bool IsFree { get; set; }
        public string CourseType { get; set; }
        public string Tags { get; set; }
    }
}
