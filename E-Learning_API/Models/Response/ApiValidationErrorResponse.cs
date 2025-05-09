namespace E_Learning_API.Models.Response
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse() : base(400, "Validation errors occurred")
        {
            Errors = new List<string>();
        }

        public ApiValidationErrorResponse(IEnumerable<string> errors) : base(400, "Validation errors occurred")
        {
            Errors = errors ?? new List<string>();
        }
    }
}
