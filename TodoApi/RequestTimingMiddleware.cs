using System.Diagnostics;

namespace TodoApi
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        { 
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();

                _logger.LogInformation("Requisição {Identifier} => {Method} {Path} levou {ElapsedMs} ns e retornou {StatusCode}", 
                    context.TraceIdentifier,
                    context.Request.Method,
                    context.Request.Path.Value,
                    sw.Elapsed.TotalNanoseconds,
                    context.Response?.StatusCode);
            }
        }
    }
}
