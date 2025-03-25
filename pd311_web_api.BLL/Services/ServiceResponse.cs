namespace pd311_web_api.BLL.Services
{
    public class ServiceResponse<T>
    {
        public ServiceResponse() { }

        public ServiceResponse(string message, bool isSuccess = false, T? payload = default, string? jwtToken = null)
        {
            Message = message;
            IsSuccess = isSuccess;
            Payload = payload;
            JwtToken = jwtToken;  // Додаємо JWT токен
        }

        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T? Payload { get; set; } = default;
        public string? JwtToken { get; set; } = null;  // Додаємо властивість для зберігання JWT токена
    }
}
