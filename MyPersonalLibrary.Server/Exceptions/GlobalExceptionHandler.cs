using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyPersonalLibrary.Server.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        IProblemDetailsService ProblemDetailsService;
        public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            ProblemDetailsService = problemDetailsService ?? throw new ArgumentNullException(nameof(problemDetailsService));
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = exception switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return await ProblemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occured",
                    Detail = exception.Message
                }
            });
        }
    }
}
