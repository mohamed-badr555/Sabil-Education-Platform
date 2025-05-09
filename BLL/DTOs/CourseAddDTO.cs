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

            [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(4000)]
        public string Details { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = "Beginner";

        [MaxLength(500)]
        public string Path { get; set; }

        public float Price { get; set; }

        public bool IsFree { get; set; }

        public string Intro_Video { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string CategoryId { get; set; }

        [MaxLength(200)]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Course type is required")]
        public string CourseType { get; set; }
        public DateTime Last_Update { get; set; }

        public string ThumbnailUrl { get; set; }
        
        public int CourseTypeAsInt 
        {
            get
            {
                if (string.IsNullOrEmpty(CourseType))
                    return 0;
                
                return CourseType switch
                {
                    "Online" => 1,
                    "Recorded" => 2,
                    "Live" => 3,
                    _ => 0
                };
            }
        }
    }
}
