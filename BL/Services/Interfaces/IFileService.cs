using System.Net;

namespace ExcelRestService.BL.Services.Interfaces
{
    public interface IFileService : IExcelRepository
    {
        /// <summary> Асинхронно сохраняет JSON-файл. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <param name="jsonContent"> Содержимое JSON. </param>
        /// <param name="statusCode"> Код статуса. </param>
        Task SaveJsonFileAsync(string fileName, string jsonContent, string statusCode);
    }
}
