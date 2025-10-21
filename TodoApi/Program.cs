using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;
using TodoApi.Filters;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("app-log.txt", rollingInterval: RollingInterval.Month)
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder();

            builder.Host.UseSerilog(); // IMPORTANTE

            builder.Services.AddControllers(opt =>
            {

                opt.Filters.Add<ApiExceptionFilterAttribute>();
            })
                .ConfigureApiBehaviorOptions(o =>
                {
                    //o.SuppressMapClientErrors = true; // <- desliga o mapeamento 4xx/5xx => ProblemDetails
                })
                .AddJsonOptions(options =>
                {

                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddDbContext<TodoDbContext>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                
                var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                //c.EnableAnnotations();
            });

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<RequestTimingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
