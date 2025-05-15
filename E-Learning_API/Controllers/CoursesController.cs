using AutoMapper;
using BLL.DTOs;
using BLL.Managers.CourseManager;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using E_Learning_API.Models.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Controllers
{
    [EnableCors("ReactApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseApiController
    {
        private readonly ICourseManager _courseManager;

        public CoursesController(ICourseManager courseManager)
        {
            _courseManager = courseManager;
        }

        // In CoursesController.cs
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseFormat<Pagination<CourseListDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<Pagination<CourseListDTO>>>> GetAllAsync([FromQuery] CourseSpecsParams courseparams)
        {
            var pagination = await _courseManager.GetAllAsync(courseparams);
            return OkResponse(pagination, "List of courses");
        }

        [HttpGet("popular")]
        [ProducesResponseType(typeof(ApiResponseFormat<Pagination<CourseListDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<Pagination<CourseListDTO>>>> GetPopular()
        {
            var courses = await _courseManager.GetPopularAsync();
            return OkResponse(courses, "Popular courses retrieved");
        }

        [HttpGet("categories")]
        [ProducesResponseType(typeof(ApiResponseFormat<IEnumerable<CategoryReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<IEnumerable<CategoryReadDTO>>>> GetAllCategories()
        {
            var categories = await _courseManager.GetAllCategoriesAsync();
            return OkResponse(categories, "Course categories retrieved");
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<CourseContentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<CourseContentDTO>>> GetById(string Id)
        {
            try
            {
                var course = await _courseManager.GetByIdAsync(Id);
                if (course == null)
                    return NotFoundResponse<CourseContentDTO>($"Course with ID {Id} not found");

                return OkResponse(course, "Course details");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFoundResponse<CourseContentDTO>(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseFormat<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseFormat<string>>> Add(CourseAddDTO courseAddDto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse<string>("Invalid model state");

            await _courseManager.AddAsync(courseAddDto);
            return NoContentResponse<string>("Course added successfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseFormat<string>>> UpdateCourse(string id, CourseAddDTO course)
        {
            if (id != course.Id)
                return BadRequestResponse<string>("Course ID mismatch");

            var existingCourse = await _courseManager.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFoundResponse<string>($"Course with ID {id} not found");

            await _courseManager.UpdateAsync(course);
            return NoContentResponse<string>("Course updated successfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<string>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseFormat<string>>> DeleteCourse(string id)
        {
            var course = await _courseManager.GetByIdAsync(id);
            if (course == null)
                return NotFoundResponse<string>($"Course with ID {id} not found");

            await _courseManager.DeleteAsync(id);
            return NoContentResponse<string>("Course deleted successfully");
        }

        [HttpGet("{coursePath}/units/{unitOrderIndex}/videos/{videoOrderIndex}")]
        [ProducesResponseType(typeof(ApiResponseFormat<VideoDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseFormat<VideoDetailsDTO>>> GetVideo(string coursePath, int unitOrderIndex, int videoOrderIndex)
        {
            var video = await _courseManager.GetVideoAsync(coursePath, unitOrderIndex, videoOrderIndex);

            if (video == null)
                return NotFound("Video not found");

            return OkResponse(video, "Video retrieved successfully");
        }

        [HttpGet("{coursePath}/content")]
        [ProducesResponseType(typeof(ApiResponseFormat<CourseContentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseFormat<CourseContentDTO>>> GetCourseContent(string coursePath)
        {
            var content = await _courseManager.GetCourseContentAsync(coursePath);

            if (content == null)
                return NotFound("Course content not found");

            return OkResponse(content, "Content retrieved successfully");
        }
    }
}
