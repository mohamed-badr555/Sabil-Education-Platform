using AdminPanel.Models;
using BLL.Managers.CategoryManager;
using BLL.Managers.CourseManager;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseManager _courseManager;
        private readonly ICategoryManager _categoryManager;

        public HomeController(
            ILogger<HomeController> logger,
            ICourseManager courseManager,
            ICategoryManager categoryManager)
        {
            _logger = logger;
            _courseManager = courseManager;
            _categoryManager = categoryManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Create a dashboard view model with statistics
                var dashboardViewModel = new DashboardViewModel
                {
                    TotalCategories = await _categoryManager.GetCategoriesCountAsync(),
                    TotalCourses = 0 // Will be populated below
                };

                // Get course count from specs params (with no filters)
                var courseParams = new BLL.Specifications.Courses.CourseSpecsParams
                {
                    PageSize = 1, // We only need the count, not the actual data
                    PageIndex = 1
                };

                var coursesResult = await _courseManager.GetAllAsync(courseParams);
                dashboardViewModel.TotalCourses = coursesResult.Count;

                // Get popular courses
                var popularCourses = await _courseManager.GetPopularAsync();
                dashboardViewModel.PopularCourses = popularCourses.Data.Take(4).ToList();

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
