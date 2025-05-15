using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace BLL.DTOs
{
    public class MyCourseDTO
    {
        public string ID { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime subscriptionTime { get; set; }
 
        public int progressPercentage { get; set; }
        public string? LastVideoId { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string path { get; set; }
        public TimeSpan Duration { get; set; }
        public string Category { get; set; }  // Category name


        //public string? RateText { get; set; }
        //public int? RateStars { get; set; }
    }
}
