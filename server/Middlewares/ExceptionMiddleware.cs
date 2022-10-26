using System.Text.Json;
using Application.Common.Exceptions;
using Application.Common.ViewModels;

namespace server.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env
        )
        {
            _env = env;
            _logger = logger;
            _next = next;
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

                context.Response.ContentType = "application/json";

                var statusCode = ex is ApiException e ? e.StatusCode : 500;

                context.Response.StatusCode = statusCode;

                var response = _env.IsDevelopment()
                    ? new ErrorVm(statusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ErrorVm(statusCode, "Internal server error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await context.Response.WriteAsJsonAsync<ErrorVm>(response, options);
            }
        }
    }
}