namespace E_Learning_API.Models.Response
{
    public class ApiResponseFormat<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponseFormat(int code, string message, T? data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static ApiResponseFormat<T> Success(string message, T data)
        {
            return new ApiResponseFormat<T>(200, message, data);
        }

        public static ApiResponseFormat<T> NotFound(string message = null)
        {
            return new ApiResponseFormat<T>(404, message ?? "Not-Found End-Point", default);
        }

        public static ApiResponseFormat<T> BadRequest(string message = null)
        {
            return new ApiResponseFormat<T>(400, message ?? "You have made a bad request!", default);
        }

        public static ApiResponseFormat<T> ServerError(string message = null)
        {
            return new ApiResponseFormat<T>(500, message ?? "Internal Server Error", default);
        }
    }
}
