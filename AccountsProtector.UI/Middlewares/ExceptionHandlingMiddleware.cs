using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AccountsProtector.AccountsProtector.Core.DTO;
using Newtonsoft.Json;

namespace AccountsProtector.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    DtoErrorsResponse errors = new DtoErrorsResponse
                    {
                        errors = new List<string> { e.InnerException.Message}
                    };
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errors));
                }
                else
                {
                    DtoErrorsResponse errors = new DtoErrorsResponse
                    {
                        errors = new List<string>() { e.Message }
                    };
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errors));
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
