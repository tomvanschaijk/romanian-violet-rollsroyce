using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RomanianVioletRollsRoyce.Crosscutting.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Responses;

namespace RomanianVioletRollsRoyce.Crosscutting.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private const string DefaultBadRequestMessage = "One or more of the provided arguments are invalid. Please check the documentation and correct the request.";
        private const string DefaultErrorMessage = "Unexpected error occurred. If this issue continues, please contact support.";

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exc)
            {
                await HandleExceptionAsync(context, exc);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exc)
        {
            _logger.LogError(exc, exc.Message + "Exception: {@Exception}", exc);

            int code;
            ErrorResponse body;
            switch (exc)
            {
                case CustomException exception:
                    code = exception.HttpStatusCode;
                    body = new ErrorResponse(exception.HttpStatusCode, exception.Message);
                    break;
                case WebException _:
                case ArgumentException _:
                    code = StatusCodes.Status400BadRequest;
                    body = new ErrorResponse(StatusCodes.Status400BadRequest, DefaultBadRequestMessage);
                    break;
                default:
                    code = StatusCodes.Status500InternalServerError;
                    body = new ErrorResponse(StatusCodes.Status500InternalServerError, DefaultErrorMessage);
                    break;
            }

            var result = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(result);
        }
    }
}
