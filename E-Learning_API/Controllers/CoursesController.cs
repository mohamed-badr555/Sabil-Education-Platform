using AutoMapper;
using BLL.DTOs;
using BLL.Managers.CourseManager;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using E_Learning_API.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Controllers
{
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

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<CourseDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<CourseDetailsDTO>>> GetById(string Id)
        {
            try
            {
                var course = await _courseManager.GetByIdAsync(Id);
                if (course == null)
                    return NotFoundResponse<CourseDetailsDTO>($"Course with ID {Id} not found");

                return OkResponse(course, "Course details");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFoundResponse<CourseDetailsDTO>(ex.Message);
            }
        }
    }





}
