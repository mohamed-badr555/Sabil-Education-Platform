using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs.CategoryManagement
{
    public class CategoryDTO
    {
        public string Id { get; set; } = new Guid().ToString();

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public int CourseCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
