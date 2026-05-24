using System.Net;
using System.Text.Json;

namespace SmartPOS.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex.Message switch
            {
                "Invalid credentials" => HttpStatusCode.Unauthorized,
                var msg when msg.Contains("not found") => HttpStatusCode.NotFound,
                var msg when msg.Contains("required") => HttpStatusCode.BadRequest,
                var msg when msg.Contains("already exists") => HttpStatusCode.BadRequest,
                var msg when msg.Contains("must be") => HttpStatusCode.BadRequest,
                var msg when msg.Contains("stock not available") => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = (int)statusCode,
                message = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}