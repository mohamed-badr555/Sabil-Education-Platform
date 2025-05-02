namespace E_Learning_API.Models.Response
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse(int code, string message = null, object? data = null)
        {
            Code = code;
            Message = message ?? GetDefaultMessageForStatusCode(code);
            Data = data;
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made a bad request!",
                401 => "You are not authorized",
                404 => "Not-Found End-Point",
                500 => "Internal server error",
                _ => null,
            };
        }
    }
}
