using ExcelRestService.BL.Domain;

namespace ExcelRestService.BL.Services.Interfaces
{
    public interface IHttpService
    {
        /// <summary> Асинхронно отправляет файл в формате base64. </summary>
        /// <param name="base64File"> Содержимое файла в формате base64. </param>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Результат операции отправки файла. </returns>
        Task<Result> SendBase64FileAsync(string base64File, string fileName);
    }
}
