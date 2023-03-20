namespace Kiwify.API.Models
{
    public class ApiResponse<T> : ErrorMessage
    {
        public ApiResponse()
        {
        }

        public ApiResponse(T? body) : this()
        {
            Body = body;
        }

        public T? Body { get; set; }
    }

    public class ErrorMessage
    {
        public ErrorMessage()
        {
            ServerTime = DateTime.UtcNow;
        }

        public ErrorMessage(string errorMsg, DateTime? serverTime = default) : this()
        {
            ErrorMsg = errorMsg;
            ServerTime = serverTime ?? DateTime.UtcNow;
        }

        public string? ErrorMsg { get; set; }
        public DateTime ServerTime { get; set; }

        public bool HasError() => !string.IsNullOrEmpty(ErrorMsg);
    }
}
