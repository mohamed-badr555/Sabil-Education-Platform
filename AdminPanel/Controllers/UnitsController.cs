using BLL.DTOs;
using BLL.Managers.CourseManager;
using BLL.Managers.UnitManager;
using DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AdminPanel.Controllers
{
    public class UnitsController : Controller
    {
        private readonly ICourseManager _courseManager;
        private readonly IUnitManager _unitManager;
        private readonly ILogger<UnitsController> _logger;

        public UnitsController(
            ICourseManager courseManager,
            IUnitManager unitManager,
            ILogger<UnitsController> logger)
        {
            _courseManager = courseManager;
            _unitManager = unitManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string courseId)
        {
            try
            {
                if (string.IsNullOrEmpty(courseId))
                {
                    TempData["Error"] = "Course ID is required.";
                    return RedirectToAction("Index", "Course");
                }

                var course = await _courseManager.GetByIdAsync(courseId);
                if (course == null)
                {
                    TempData["Error"] = $"Course with ID {courseId} not found.";
                    return RedirectToAction("Index", "Course");
                }

                var units = await _unitManager.GetUnitsByCourseIdAsync(courseId);

                ViewBag.CourseId = courseId;
                ViewBag.CourseTitle = course.Title;

                return View(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving units for course ID: {CourseId}", courseId);
                TempData["Error"] = "Failed to retrieve course units. Please try again.";
                return RedirectToAction("Index", "Course");
            }
        }

        // AdminPanel/Controllers/UnitsController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUnit(UnitDTO unitDto)
        {
            try
            {
                // Remove Id from ModelState validation since we'll generate it
                ModelState.Remove("Id");
                // Remove videos from ModelState validation since it will be null for new units
                ModelState.Remove("videos");

                if (string.IsNullOrEmpty(unitDto.CourseID))
                {
                    TempData["Error"] = "Course ID is required.";
                    return RedirectToAction("Index", "Course");
                }

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid unit data. Please check your input.";
                    return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
                }

                // Initialize videos collection to prevent null reference exception
                unitDto.videos = new List<VideoDetailsDTO>();

                var (success, message) = await _unitManager.AddUnitAsync(unitDto);
                if (success)
                {
                    TempData["Success"] = message;
                }
                else
                {
                    TempData["Error"] = message;
                }

                return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding unit: {Message}", ex.Message);
                TempData["Error"] = "Failed to add unit. Please try again.";
                return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUnit(UnitDTO unitDto)
        {
            try
            {
                ModelState.Remove("videos");
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid unit data. Please check your input.";
                    return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
                }

                var (success, message) = await _unitManager.UpdateUnitAsync(unitDto);
                if (success)
                {
                    TempData["Success"] = message;
                }
                else
                {
                    TempData["Error"] = message;
                }

                return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating unit: {Message}", ex.Message);
                TempData["Error"] = "Failed to update unit. Please try again.";
                return RedirectToAction(nameof(Index), new { courseId = unitDto.CourseID });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUnit(string id, string courseId)
        {
            try
            {
                // Get all videos associated with this unit
                var unit = await _unitManager.GetUnitByIdAsync(id);
                if (unit != null && unit.videos != null && unit.videos.Any())
                {
                    // Delete all videos in the unit first
                    foreach (var video in unit.videos)
                    {
                        await _unitManager.DeleteVideoAsync(video.Id);
                    }
                }

                // Then delete the unit itself
                await _unitManager.DeleteUnitAsync(id);

                TempData["Success"] = "Unit and all its videos deleted successfully.";
                return RedirectToAction(nameof(Index), new { courseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting unit");
                TempData["Error"] = "Failed to delete unit. Please try again.";
                return RedirectToAction(nameof(Index), new { courseId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVideo(VideoDetailsDTO videoDto)
        {
            try
            {
                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(videoDto.CourseUnitID);

                // Remove unnecessary validations
                ModelState.Remove("Id");
                ModelState.Remove("VideoComments");

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid video data. Please check your input.";
                    return RedirectToAction(nameof(Index), new { courseId });
                }

                // Format YouTube URL if needed
                videoDto.URL = FormatYouTubeUrl(videoDto.URL);

                var (success, message) = await _unitManager.AddVideoAsync(videoDto);
                if (success)
                {
                    TempData["Success"] = message;
                }
                else
                {
                    TempData["Error"] = message;
                }

                return RedirectToAction(nameof(Index), new { courseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding video: {Message}", ex.Message);
                TempData["Error"] = "Failed to add video. Please try again.";

                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(videoDto.CourseUnitID);
                return RedirectToAction(nameof(Index), new { courseId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVideo(VideoDetailsDTO videoDto)
        {
            try
            {
                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(videoDto.CourseUnitID);

                // Remove unnecessary validations
                ModelState.Remove("VideoComments");

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogError($"Model error for {state.Key}: {error.ErrorMessage}");
                        }
                    }

                    TempData["Error"] = "Invalid video data. Please check your input.";
                    return RedirectToAction(nameof(Index), new { courseId });
                }

                // Format YouTube URL if needed
                videoDto.URL = FormatYouTubeUrl(videoDto.URL);

                var (success, message) = await _unitManager.UpdateVideoAsync(videoDto);
                if (success)
                {
                    TempData["Success"] = message;
                }
                else
                {
                    TempData["Error"] = message;
                }

                return RedirectToAction(nameof(Index), new { courseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating video: {Message}", ex.Message);
                TempData["Error"] = "Failed to update video. Please try again.";

                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(videoDto.CourseUnitID);
                return RedirectToAction(nameof(Index), new { courseId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVideo(string id, string unitId)
        {
            try
            {
                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(unitId);

                await _unitManager.DeleteVideoAsync(id);
                TempData["Success"] = "Video deleted successfully.";
                return RedirectToAction(nameof(Index), new { courseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting video");
                TempData["Error"] = "Failed to delete video. Please try again.";

                string courseId = await _unitManager.GetCourseIdByUnitIdAsync(unitId);
                return RedirectToAction(nameof(Index), new { courseId });
            }
        }

        // Helper method to convert YouTube URLs to embed format
        private string FormatYouTubeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            // Look for YouTube URL patterns
            var youtubeRegex = new Regex(@"(youtu\.be\/|youtube\.com\/(watch\?(.*&)?v=|(embed|v)\/))([^\?&""'>]+)");
            var match = youtubeRegex.Match(url);

            if (match.Success)
            {
                var videoId = match.Groups[5].Value;
                return $"https://youtube.com/embed/{videoId}";
            }

            return url;
        }
    }
}
