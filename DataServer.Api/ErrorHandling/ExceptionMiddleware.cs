using DataServer.Common.Exceptions;
using DataServer.Common.ResponseObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataServer.Api.ErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UnAuthorizedException ex)
            {
                Console.WriteLine($"A new unauthorized exception has been thrown: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, 401);
            }
            catch (InternalException ex)
            {
                Console.WriteLine($"A new internal exception has been thrown: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, 500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, 503);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, string message, int statusCode)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var responseObject = new CustomResponse<object>()
            {
                StatusCode = statusCode,
                Data = null,
                HasError = true,
                Error = new CustomResponse<object>.CustomError() { Message = message }
            };

            await httpContext.Response.WriteAsJsonAsync(responseObject);
        }
    }
}