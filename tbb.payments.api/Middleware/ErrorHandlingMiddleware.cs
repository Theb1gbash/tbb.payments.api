using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using tbb.payments.api.Exceptions;
using tbb.payments.api.Models;

namespace tbb.payments.api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PaymentException ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex, ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex, ErrorCodes.UnknownError, "An unexpected error occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ErrorCodes errorCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                ErrorCode = errorCode.ToString(),
                Message = message
            }.ToString());
        }
    }
}
