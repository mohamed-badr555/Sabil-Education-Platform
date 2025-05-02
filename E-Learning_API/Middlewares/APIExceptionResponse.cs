public class ApiExceptionResponse
{
 
    public int StatusCode { get; set; }
    public string Message { get; set; }
     public string Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object> Extensions { get; set; } = new();
    public string Path { get; set; }
    public string Type { get; set; }

    public ApiExceptionResponse(int statusCode, string message, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    // Factory method for common error types
    public static ApiExceptionResponse NotFound(string message)
        => new(404, message);

    public static ApiExceptionResponse BadRequest(string message)
        => new(400, message);

    public static ApiExceptionResponse Unauthorized(string message)
        => new(401, message);
}