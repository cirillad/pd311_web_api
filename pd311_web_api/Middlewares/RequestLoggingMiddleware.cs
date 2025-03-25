using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logFilePath = "logs/requests.log";

        // Переконатися, що каталог існує
        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    public async Task Invoke(HttpContext context)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {context.Request.Method} | {context.Request.Path}\n";

        // Запис логу у файл
        await File.AppendAllTextAsync(_logFilePath, logEntry);

        // Передача запиту далі по конвеєру
        await _next(context);
    }
}
