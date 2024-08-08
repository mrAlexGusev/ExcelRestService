using ExcelRestService.BL.Domain;

namespace ExcelRestService.BL.Services.Interfaces
{
    public interface IExcelRepository
    {
        /// <summary> Асинхронно получает Excel-файл. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Поток файла, если он существует, иначе null. </returns>
        Task<FileStream> GetExcelFileAsync(string fileName);

        /// <summary> Асинхронно сохраняет Excel-файл.
        /// <param name="file"> Файл для сохранения. </param>
        /// <returns> Результат операции сохранения файла. </returns>
        Task<Result> SaveExcelFileAsync(IFormFile file);

        /// <summary> Асинхронно удаляет Excel-файл. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Результат операции удаления файла. </returns>
        Task<Result> DeleteExcelFileAsync(string fileName);
    }
}
