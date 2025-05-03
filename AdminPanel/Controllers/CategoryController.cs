using AdminPanel.Models;
using BLL.DTOs.CategoryManagement;
using BLL.Managers.CategoryManager;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryManager categoryManager, ILogger<CategoryController> logger)
        {
            _categoryManager = categoryManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryManager.GetAllCategoriesAsync();
                var totalCount = await _categoryManager.GetCategoriesCountAsync();

                var viewModel = new CategoryViewModel
                {
                    Categories = categories,
                    TotalCount = totalCount
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                TempData["Error"] = "Failed to load categories. Please try again.";
                return View(new CategoryViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Invalid category data. Please check your input.";
                    return RedirectToAction(nameof(Index));
                }

                if (await _categoryManager.ExistsByNameAsync(categoryDto.Name))
                {
                    TempData["Error"] = "A category with this name already exists.";
                    return RedirectToAction(nameof(Index));
                }

                // Don't need to set ID here as it will be generated in the manager
                // The CategoryManager.AddAsync method will handle ID generation
                
                await _categoryManager.AddAsync(categoryDto);
                TempData["Success"] = "Category added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                TempData["Error"] = "Failed to add category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var category = await _categoryManager.GetByIdAsync(id);
                return PartialView("_EditCategory", category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category for edit");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Invalid category data. Please check your input.";
                    return RedirectToAction(nameof(Index));
                }

                var existingCategory = await _categoryManager.GetByIdAsync(categoryDto.Id);

                if (existingCategory.Name != categoryDto.Name &&
                    await _categoryManager.ExistsByNameAsync(categoryDto.Name))
                {
                    TempData["Error"] = "A category with this name already exists.";
                    return RedirectToAction(nameof(Index));
                }

                await _categoryManager.UpdateAsync(categoryDto);
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                TempData["Error"] = "Failed to update category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _categoryManager.DeleteAsync(id);
                TempData["Success"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                TempData["Error"] = "Failed to delete category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
