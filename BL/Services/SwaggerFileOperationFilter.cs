using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExcelRestService.BL.Services
{
    /// <summary> Фильтр операций для Swagger, который добавляет поддержку загрузки файлов. </summary>
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        /// <summary> Применяет фильтр к операции Swagger. </summary>
        /// <param name="operation"> Операция Swagger, к которой применяется фильтр. </param>
        /// <param name="context"> Контекст фильтра операции. </param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            foreach (var fileParam in fileParams)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = 
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = {
                                    [fileParam.Name] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
