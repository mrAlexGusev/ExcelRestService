using ExcelRestService.BL.Services.Interfaces;
using ExcelRestService.BL.Services;
using ExcelRestService.DAL.Data.Abstract;
using ExcelRestService.DAL.Data;
using Microsoft.OpenApi.Models;

namespace ExcelRestService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddScoped<IExcelService, ExcelService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<ILoggerService, LoggerService>();
            builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
            builder.Services.AddScoped<IRepository, ExcelRepository>();
            builder.Services.AddScoped<IJsonWriter, JsonWriter>();
            builder.Services.AddScoped<ILog, Logger>();

            builder.Services.AddHttpClient<IHttpService, HttpService>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Excel API", Version = "v1" });

                c.OperationFilter<SwaggerFileOperationFilter>();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Excel API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
 
        }
    }
}
