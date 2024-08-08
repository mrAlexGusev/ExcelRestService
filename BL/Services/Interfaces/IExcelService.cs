using ExcelRestService.BL.Domain;

namespace ExcelRestService.BL.Services.Interfaces
{
    public interface IExcelService
    {
        /// <summary> Асинхронно получает Excel-файл. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Поток файла, если он существует, иначе null. </returns>
        Task<FileStream> GetExcelFile(string fileName);

        /// <summary> Асинхронно сохраняет Excel-файл. </summary>
        /// <param name="file"> Файл для сохранения. </param>
        /// <returns> Результат операции сохранения файла. </returns>
        Task<Result> SaveExcelFile(IFormFile file);

        /// <summary> Асинхронно удаляет Excel-файл. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Результат операции удаления файла. </returns>
        Task<Result> DeleteExcelFile(string fileName);

        /// <summary> Асинхронно обрабатывает файл, транспонируя его, сохраняя, конвертируя в base64 и отправляя. </summary>
        /// <param name="file"> Файл для обработки. </param>
        /// <returns> Результат операции обработки и отправки файла. </returns>
        Task<Result> ProcessFile(IFormFile file);
    }
}
