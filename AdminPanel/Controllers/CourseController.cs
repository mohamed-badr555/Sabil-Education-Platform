using AdminPanel.Helper;
using AdminPanel.Models;
using BLL.DTOs;
using BLL.Managers.CategoryManager;
using BLL.Managers.CourseManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace AdminPanel.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseManager _courseManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseManager courseManager, ICategoryManager categoryManager, ILogger<CourseController> logger)
        {
            _courseManager = courseManager;
            _categoryManager = categoryManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var coursesParams = new BLL.Specifications.Courses.CourseSpecsParams
                {
                    PageSize = 50  // Get more courses for admin panel
                };
                var courses = await _courseManager.GetAllAsync(coursesParams);
                var categories = await _categoryManager.GetAllCategoriesAsync();

                var viewModel = new CourseViewModel
                {
                    Courses = courses.Data.ToList(),
                    TotalCount = courses.Count,
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Value = c.Id,
                        Text = c.Name
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses");
                TempData["Error"] = "Failed to retrieve courses. Please try again.";
                return View(new CourseViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseAddDTO courseDto)
        {
            try
            {
                // Remove Id and ThumbnailUrl from ModelState validation
                ModelState.Remove("Id");
                ModelState.Remove("ThumbnailUrl");

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid course data. Please check your input.";
                    return RedirectToAction(nameof(Index));
                }

                //// Generate a valid Id (GUID)
                //courseDto.Id = Guid.NewGuid().ToString();

                // Set default values for required fields if they're null
                //courseDto.Level = courseDto.Level ?? "Beginner";
                courseDto.Path = string.IsNullOrEmpty(courseDto.Path) && !string.IsNullOrEmpty(courseDto.Title)
                    ? GenerateSlug(courseDto.Title)
                    : courseDto.Path;
                //courseDto.Last_Update = DateTime.Now;

                // Handle file upload
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0]; // Get the first file (thumbnailFile)
                    if (file != null && file.Length > 0)
                    {
                        // Save the file and get the filename
                        courseDto.ThumbnailUrl = await DocumentSettings.UploadFile(file, "courses");
                    }
                }

                await _courseManager.AddAsync(courseDto);
                TempData["Success"] = "Course added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding course: {Message}", ex.Message);
                TempData["Error"] = "Failed to add course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var course = await _courseManager.GetByIdAsync(id);
                var categories = await _categoryManager.GetAllCategoriesAsync();

                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList();

                return PartialView("_EditCourse", course);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course for edit");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseAddDTO courseDto)
        {
            try
            {
                // Remove ThumbnailUrl from ModelState validation as we handle it separately
                ModelState.Remove("ThumbnailUrl");

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid course data. Please check your input.";
                    return RedirectToAction(nameof(Index));
                }

                // Get the original course to preserve thumbnail if no new one is uploaded
                var originalCourse = await _courseManager.GetByIdAsync(courseDto.Id);

                // Handle file upload - only update ThumbnailUrl if a new file is provided
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0]; // Get the first file (thumbnailFile)
                    if (file != null && file.Length > 0)
                    {
                        // Update file and get the new filename
                        courseDto.ThumbnailUrl = await DocumentSettings.UpdateFile(file, "courses", originalCourse.ThumbnailUrl);
                    }
                }
                else
                {
                    // Important: Preserve the original ThumbnailUrl if no new file is uploaded
                    courseDto.ThumbnailUrl = originalCourse.ThumbnailUrl;
                }

                courseDto.Last_Update = DateTime.Now;
                await _courseManager.UpdateAsync(courseDto);
                TempData["Success"] = "Course updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course: {Message}", ex.Message);
                TempData["Error"] = "Failed to update course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _courseManager.DeleteAsync(id);
                TempData["Success"] = "Course deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course");
                TempData["Error"] = "Failed to delete course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to generate a URL-friendly slug from a title
        private string GenerateSlug(string title)
        {
            // Convert to lowercase
            string slug = title.ToLower();

            // Replace spaces and special characters with hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }
    }
}
