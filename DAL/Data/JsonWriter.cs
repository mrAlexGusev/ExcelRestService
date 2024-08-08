using ExcelRestService.DAL.Data.Abstract;
using Newtonsoft.Json;

namespace ExcelRestService.DAL.Data
{
    /// <summary> Класс для записи JSON-контента в файл. </summary>
    public class JsonWriter : IJsonWriter
    {
        public void WriteJsonToFile(string filePath, string jsonContent, string statusCode)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            try
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var jsonObject = new
                {
                    StatusCode = statusCode,
                    Content = jsonContent
                };

                var jsonString = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error writing JSON file to {filePath}: {ex.Message}", ex);
            }
        }
    }
}
