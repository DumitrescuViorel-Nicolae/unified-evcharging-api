namespace Domain.Models
{
    public class GeneralResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public GeneralResponse(bool success, string? message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public GeneralResponse(bool success, string message) : this(success, message, default)
        {
            Success = success;
            Message = message;
        }

    }
}
