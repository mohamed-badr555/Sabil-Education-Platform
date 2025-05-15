using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace BLL.DTOs
{

    public class CourseContentDTO
    {

        public string Title { get; set; } // e.g., "تعلم النحو من الصفر حتى الإحتراف"
        public string Description { get; set; } // Main overview (1-2 paragraphs)
        public string ThumbnailUrl { get; set; }
        public int StudentsCount { get; set; }
        public int progress { get; set; }
        public TimeSpan Duration { get; set; } // e.g., 8h 30m
                                               //public string? LastVideo { get; set; }   // Revise here
        public string lastAccessedVideoId { get; set; }
        public string CourseType { get; set; }

        public ICollection<UnitDTO> Units { get; set; }



    }
}
