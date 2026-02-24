namespace ClienteApi.Models;

public class ApiErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public object? Details { get; set; }
}
