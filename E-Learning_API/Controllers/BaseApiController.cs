using E_Learning_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult<ApiResponseFormat<T>> OkResponse<T>(T? data, string message = null)
        {
            return Ok(new ApiResponseFormat<T>(200, message ?? "Success", data));
        }

        protected ActionResult<ApiResponseFormat<T>> NotFoundResponse<T>(string message = null)
        {
            return NotFound(new ApiResponseFormat<T>(404, message ?? "Not-Found End-Point", default));
        }

        protected ActionResult<ApiResponseFormat<T>> BadRequestResponse<T>(string message = null, T data = default)
        {
            return BadRequest(new ApiResponseFormat<T>(400, message ?? "You have made a bad request!", data));
        }

        protected ActionResult<ApiResponseFormat<T>> ServerErrorResponse<T>(string message = null)
        {
            return StatusCode(500, new ApiResponseFormat<T>(500, message ?? "Internal Server Error", default));
        }

        protected ActionResult<ApiResponseFormat<T>> NoContentResponse<T>(string message = null)
        {
       
          return StatusCode(204, new ApiResponseFormat<T>(204, message ?? "No Content", default));
        }
    }
}
