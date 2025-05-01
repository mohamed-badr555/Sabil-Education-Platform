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



        //[HttpGet("paged")]
        //public async Task<ActionResult<Pagination<CourseListDTO>>> GetPagedCourses(int page = 1, int pageSize = 10)
        //{
         
        //        var result = await _courseMananger.GetPagedCoursesAsync(page, pageSize);
        //        return Ok(result); // Return the paged result
       
        //}




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var course = await _courseMananger.GetByIdAsync(Id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var courses = await _courseMananger.SearchCoursesAsync(searchTerm);
            return Ok(courses);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterCourses([FromQuery] string? level, [FromQuery] string? category)
        {
            var courses = await _courseMananger.FilterCoursesAsync(level, category);
            return Ok(courses);
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
        public async Task<IActionResult> UpdateCourse(int id, CourseAddDTO course)
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
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseMananger.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            await _courseMananger.DeleteAsync(id);
            return NoContent();
        }
    }





}
