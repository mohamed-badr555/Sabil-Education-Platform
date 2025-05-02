namespace E_Learning_API.Models.Response
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int code, string message = null, string? details = null) 
            : base(code, message)
        {
            Details = details;
        }
    }
}
