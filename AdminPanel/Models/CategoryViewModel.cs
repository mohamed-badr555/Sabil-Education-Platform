namespace AdminPanel.Models
{
    public class CategoryViewModel
    {
        public List<BLL.DTOs.CategoryManagement.CategoryDTO> Categories { get; set; } = new();
        public BLL.DTOs.CategoryManagement.CategoryDTO CategoryForEdit { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
