using BLL.DTOs.CategoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.CategoryManager
{
    public interface ICategoryManager
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync(bool includeDeleted = false);
        Task<CategoryDTO> GetByIdAsync(string id);
        Task<CategoryDTO> AddAsync(CategoryDTO categoryDto);
        Task UpdateAsync(CategoryDTO categoryDto);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
        Task<int> GetCategoriesCountAsync();
    }
}
