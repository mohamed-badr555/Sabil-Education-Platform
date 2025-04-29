using BLL.DTOs;
using BLL.Managers.EnrollmentManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentManager _enrollmentManager;

        public EnrollmentsController(IEnrollmentManager enrollmentManager)
        {
            _enrollmentManager = enrollmentManager;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollRequestDTO enrollRequest)
        {
            var result = await _enrollmentManager.EnrollAccountAsync(enrollRequest.StudentId, enrollRequest.CourseId);

            if (result)
            {
                return Ok("Enrollment successful.");
            }
            else
            {
                return BadRequest("Enrollment failed. Please check the provided information.");
            }
        }
    }
}
