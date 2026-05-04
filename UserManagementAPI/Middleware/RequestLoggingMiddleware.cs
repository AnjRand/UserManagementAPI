namespace UserManagementAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        // Le constructeur reçoit le délégué 'next' pour passer à l'étape suivante du pipeline
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // La méthode InvokeAsync est appelée à chaque requête HTTP
        public async Task InvokeAsync(HttpContext context)
        {
            // Logique avant que le reste de l'application ne traite la requête
            _logger.LogInformation($"[LOG] Requête : {context.Request.Method} {context.Request.Path}");

            // Appel du middleware suivant dans le pipeline
            await _next(context);

            // Optionnel : Logique après le traitement de la requête (réponse)
            _logger.LogInformation($"[LOG] Réponse envoyée avec le statut : {context.Response.StatusCode}");
        }
    }
}
