namespace ExcelRestService.DAL.Data.Abstract
{
    public interface IJsonWriter
    {
        /// <summary> Записывает JSON-контент в файл по указанному пути. </summary>
        /// <param name="filePath"> Путь к файлу. </param>
        /// <param name="jsonContent"> Содержимое JSON для записи. </param>
        /// <param name="statusCode"> Код статуса для включения в JSON-объект. </param>
        void WriteJsonToFile(string filePath, string jsonContent, string statusCode);
    }
}
