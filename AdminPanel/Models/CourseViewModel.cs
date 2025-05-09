using BLL.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class CourseViewModel
    {
        public List<CourseListDTO> Courses { get; set; } = new List<CourseListDTO>();
        public int TotalCount { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        // Properties for adding a new course
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; }

        [MaxLength(4000, ErrorMessage = "Details cannot exceed 4000 characters")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; }

        [MaxLength(500, ErrorMessage = "Path cannot exceed 500 characters")]
        public string Path { get; set; }

        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100,000")]
        public float Price { get; set; }

        public bool IsFree { get; set; }

        [MaxLength(500, ErrorMessage = "Video URL cannot exceed 500 characters")]
        public string Intro_Video { get; set; }

        public string CategoryID { get; set; }

        [MaxLength(200, ErrorMessage = "Tags cannot exceed 200 characters")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Course type is required")]
        public string CourseType { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
