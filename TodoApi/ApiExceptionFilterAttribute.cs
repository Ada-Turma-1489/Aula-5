using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Controllers;

namespace TodoApi.Filters
{
    /*
     builder.Services.AddControllers(opt => opt.Filters.Add<ApiExceptionFilterAttribute>()})
    */
    public class ApiExceptionFilterAttribute : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Loga a exceção para fins de diagnóstico.
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada.");

            if (context.Exception is EntityNotFoundException entityNotFoundException)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "A entidade procurada não foi encontrada.",
                    Detail = "Not Found"
                };

                problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                context.Result = new ObjectResult(problem) { StatusCode = StatusCodes.Status404NotFound };
                context.ExceptionHandled = true;

                return;
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Ocorreu um erro inesperado.",
                    Detail = "Um erro ocorreu ao processar sua requisição. Por favor, tente novamente mais tarde."
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };

                context.ExceptionHandled = true;
            }
        }
    }
}