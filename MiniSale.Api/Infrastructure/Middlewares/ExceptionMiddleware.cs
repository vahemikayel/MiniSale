using FluentValidation;
using Microsoft.AspNetCore.Http;
using MiniSale.Api.Infrastructure.Exceptions;
using MiniSale.Api.Models.Response;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSale.Api.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
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
                ResultModel response;
                var responseJson = string.Empty;
                Log.Error(ex, ex.Message);
                context.Response.ContentType = "application/json";
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                if (ex is ValidationException validationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = validationException?.Errors?.Select(x => x.ErrorMessage)?.ToList();
                    var clientMessage = errors?.FirstOrDefault();
                    response = new ApiValidationErrorResponse()
                    {
                        Message = clientMessage,
                        Errors = errors,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                    responseJson = JsonSerializer.Serialize(response, jsonOptions);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new ApiExceptionModel(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
                    responseJson = JsonSerializer.Serialize(response, jsonOptions);
                }

                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
