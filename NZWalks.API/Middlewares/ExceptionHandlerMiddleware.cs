using Microsoft.AspNetCore.Http;
using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await next(httpcontext);
            }
            catch (Exception ex) 
            {
                var errorId = Guid.NewGuid();

                //Log this exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");
                //Return a custom error response

                httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpcontext.Response.ContentType = "application/json";


                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. We are looking into resolving this"
                };

                await httpcontext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
