using ExcelRestService.DAL.Entyties;

namespace ExcelRestService.DAL.Data.Abstract
{
    public interface IRepository
    {
        /// <summary> Асинхронно получает файл по указанному пути. </summary>
        /// <param name="path"> Путь к файлу. </param>
        /// <returns> FileStream файла, если он существует, иначе null. </returns>
        Task<FileStream> GetAsync(string path);

        /// <summary> Асинхронно удаляет файл по указанному пути. </summary>
        /// <param name="filePath"> Путь к файлу для удаления. </param>
        /// <returns> Объект ResultEntity с результатом операции. </returns>
        Task<ResultEntity> DeleteAsync(string path);

        /// <summary> Асинхронно сохраняет файл по указанному пути. </summary>
        /// <param name="file"> Файл для сохранения. </param>
        /// <param name="path"> Путь для сохранения файла. </param>
        /// <returns> Объект ResultEntity с результатом операции. </returns>
        Task<ResultEntity> SaveAsync(IFormFile file, string path);
    }
}
