using AutoMapper;
using BLL.DTOs;
using BLL.Managers.CourseManager;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseManager _courseMananger;

        public CoursesController(ICourseManager courseMananger)
        {
            _courseMananger = courseMananger;
        }


        [HttpGet]             // matches GET /courses
        public async Task<ActionResult<Pagination<CourseListDTO>>> GetAllAsync([FromQuery] CourseSpecsParams courseparams)
        {
            var pagination = await _courseMananger.GetAllAsync(courseparams);
            return Ok(pagination);

        }


        [HttpGet("popular")]      // matches GET /courses/popular
        public async Task<IActionResult> GetPopular()
        {
            var courses = await _courseMananger.GetPopularAsync();
            return Ok(courses);
        }



        [HttpGet("categories")]      // matches GET /courses/categories
        public async Task<IActionResult> GetAllCategories()
        {
            var courses = await _courseMananger.GetAllCategoriesAsync();
            return Ok(courses);
        }



        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var course = await _courseMananger.GetByIdAsync(Id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }


        [HttpPost]
        public async Task<IActionResult> Add(CourseAddDTO courseAddDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseMananger.AddAsync(courseAddDto);
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(string id, CourseAddDTO course)
        {
            if (id != course.Id)
                return BadRequest();

            var existingCourse = await _courseMananger.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFound();

            await _courseMananger.UpdateAsync(course);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var course = await _courseMananger.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            await _courseMananger.DeleteAsync(id);
            return NoContent();
        }


        [HttpGet("{coursePath}/units/{unitOrderIndex}/videos/{videoOrderIndex}")]
        public async Task<IActionResult> GetVideo(string coursePath, int unitOrderIndex, int videoOrderIndex)
        {
            var video = await _courseMananger.GetVideoAsync(coursePath, unitOrderIndex, videoOrderIndex);

            if (video == null)
            {
                return NotFound(new { Message = "Video not found." });
            }

            return Ok(new
            {
                Code = 200,
                Message = "Video retrieved successfully",
                Data = video
            });
        }


        [HttpGet("{coursePath}/content")]
        public async Task<IActionResult> GetCourseContent(string coursePath)
        {
            var content = await _courseMananger.GetCourseContentAsync(coursePath);

            if (content == null)
            {
                return NotFound(new { Message = "content not found." });
            }

            return Ok(new
            {
                Code = 200,
                Message = "content retrieved successfully",
                Data = content
            });
        }

    }





}
