// BLL/Managers/CategoryManager/CategoryManager.cs
using AutoMapper;
using BLL.DTOs.CategoryManagement;
using DAL.Data.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Managers.CategoryManager
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryManager(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync(bool includeDeleted = false)
        {
            var categories = await _categoryRepo.GetAllAsync(includeDeleted);
            var categoryDtos = _mapper.Map<List<CategoryDTO>>(categories);

            // Enrich with course count
            foreach (var categoryDto in categoryDtos)
            {
                var category = categories.FirstOrDefault(c => c.Id == categoryDto.Id);
                categoryDto.CourseCount = category?.Courses?.Count ?? 0;
            }

            return categoryDtos;
        }

        public async Task<CategoryDTO> GetByIdAsync(string id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            categoryDto.CourseCount = category.Courses?.Count ?? 0;

            return categoryDto;
        }

        public async Task<CategoryDTO> AddAsync(CategoryDTO categoryDto)
        {
            // Generate a new ID for the category
            categoryDto.Id = Guid.NewGuid().ToString();

            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepo.InsertAsync(category);

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateAsync(CategoryDTO categoryDto)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryDto.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");

            _mapper.Map(categoryDto, category);
            await _categoryRepo.UpdateAsync(category);
        }

        public async Task DeleteAsync(string id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            await _categoryRepo.SoftDeleteAsync(category);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return category != null;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _categoryRepo.AnyAsync(c => c.Name == name && !c.IsDeleted);
        }

        public async Task<int> GetCategoriesCountAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories.Count;
        }
    }
}
