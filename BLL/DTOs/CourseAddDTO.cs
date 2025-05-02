using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CourseAddDTO
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
        public int N_Lessons { get; set; } // e.g.,  11 lesson
        public TimeSpan Duration { get; set; } // e.g., 8h 30m
        public DateTime Last_Update { get; set; } // e.g., "تم التحديث: ٢٠٢٣-١٠-١٥"
        public int? CategoryId { get; set; }

        // Content Sections
        [MaxLength(2000)]
        public string Description { get; set; } // Main overview (1-2 paragraphs)

        [MaxLength(4000)]
        public string Details { get; set; }
        public string Level { get; set; }
        public string Path { get; set; }
        public string ThumbnailUrl { get; set; }


        // Progress Tracking (if user is enrolled)
        //public bool IsEnrolled { get; set; }
        //public decimal CompletionPercentage { get; set; }

        // Call-to-Action
        public float Price { get; set; }
        public bool IsFree { get; set; }
        //public string EnrollmentUrl { get; set; } // e.g., "/enroll/123"

        // Navigation Links
        //public Dictionary<string, string> Links { get; set; } = new()
        //{
        //    ["self"] = "", // Will be "/api/courses/123"
        //    ["lessons"] = "" // Will be "/api/courses/123/lessons"

        //};
    }
}
