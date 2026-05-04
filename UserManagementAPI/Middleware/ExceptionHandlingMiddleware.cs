using System.Net;
using System.Text.Json;
using UserManagementAPI.Models;

namespace UserManagementAPI.Middleware
{
    /// <summary>
    /// Middleware to catch all unhandled exceptions and return consistent JSON error responses
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception occurred");
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var traceId = context.TraceIdentifier;
            GlobalErrorResponse response;

            switch (exception)
            {
                case ArgumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = GlobalErrorResponse.BadRequest("One or more required arguments were null", traceId);
                    break;

                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = GlobalErrorResponse.BadRequest(argEx.Message, traceId);
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response = GlobalErrorResponse.Unauthorized("Access denied", traceId);
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = GlobalErrorResponse.NotFound(exception.Message, traceId);
                    break;

                case InvalidOperationException invOpEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = GlobalErrorResponse.BadRequest(invOpEx.Message, traceId);
                    break;

                default:
                    // Generic exception - Internal Server Error
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = GlobalErrorResponse.InternalServerError(
                        "An unexpected error occurred. Please try again later.", 
                        traceId);
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
