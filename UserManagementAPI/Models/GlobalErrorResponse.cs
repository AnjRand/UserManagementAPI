namespace UserManagementAPI.Models
{
    /// <summary>
    /// Standard error response format for all API errors
    /// </summary>
    public class GlobalErrorResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string TraceId { get; set; }

        public GlobalErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public GlobalErrorResponse(string error, string message, int statusCode, string traceId = null)
            : this()
        {
            Error = error;
            Message = message;
            StatusCode = statusCode;
            TraceId = traceId;
        }

        /// <summary>
        /// Create a 400 Bad Request error response
        /// </summary>
        public static GlobalErrorResponse BadRequest(string message, string traceId = null)
            => new("BadRequest", message, 400, traceId);

        /// <summary>
        /// Create a 401 Unauthorized error response
        /// </summary>
        public static GlobalErrorResponse Unauthorized(string message = "Unauthorized", string traceId = null)
            => new("Unauthorized", message, 401, traceId);

        /// <summary>
        /// Create a 403 Forbidden error response
        /// </summary>
        public static GlobalErrorResponse Forbidden(string message = "Forbidden", string traceId = null)
            => new("Forbidden", message, 403, traceId);

        /// <summary>
        /// Create a 404 Not Found error response
        /// </summary>
        public static GlobalErrorResponse NotFound(string message = "Resource not found", string traceId = null)
            => new("NotFound", message, 404, traceId);

        /// <summary>
        /// Create a 500 Internal Server Error response
        /// </summary>
        public static GlobalErrorResponse InternalServerError(string message = "Internal server error", string traceId = null)
            => new("InternalServerError", message, 500, traceId);

        /// <summary>
        /// Create a 422 Unprocessable Entity error response
        /// </summary>
        public static GlobalErrorResponse UnprocessableEntity(string message, string traceId = null)
            => new("UnprocessableEntity", message, 422, traceId);
    }
}
