namespace PractTest.API.Shared
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static APIResponse<T> Success(T? data, string message, int statusCode = StatusCodes.Status200OK)
        {
            return new APIResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static APIResponse<string> Error(string message, string error, int statusCode = StatusCodes.Status500InternalServerError)
        {
            return new APIResponse<string>
            {
                StatusCode = statusCode,
                Message = message,
                Data = error
            };
        }
    }
}
