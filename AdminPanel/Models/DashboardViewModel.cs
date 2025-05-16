// AdminPanel/Models/DashboardViewModel.cs
using BLL.DTOs;
using System.Collections.Generic;

namespace AdminPanel.Models
{
    public class DashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalCategories { get; set; }
        public List<CourseListDTO> PopularCourses { get; set; } = new List<CourseListDTO>();
    }
}