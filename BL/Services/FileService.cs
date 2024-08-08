using ExcelRestService.BL.Domain;
using ExcelRestService.BL.Services.Interfaces;
using ExcelRestService.DAL.Data.Abstract;
using ExcelRestService.Mappers;
using System.Net;

namespace ExcelRestService.BL.Services
{

    /// <summary> Сервис для работы с файлами. </summary>
    public class FileService : IFileService
    {
        private readonly IRepository _repository;
        private readonly IConfigurationService _configuration;
        private readonly IJsonWriter _jsonWriter;
        private readonly ILoggerService _loggerService;

        public FileService(IConfigurationService configuration, IRepository repository, IJsonWriter jsonWriter, ILoggerService loggerService)
        {
            _repository = repository;
            _configuration = configuration;
            _jsonWriter = jsonWriter;
            _loggerService = loggerService;
        }

        public async Task<Result> SaveExcelFileAsync(IFormFile file)
        {
            if (file == null)
            {
                _loggerService.LogError("File is null.");
                return new Result { Success = false, Message = "File is null." };
            }

            try
            {
                var path = GetExcelFilePath(file.FileName);
                var resultEntity = await _repository.SaveAsync(file, path);
                var result = resultEntity.ToDomain();
                _loggerService.LogInfo($"File {file.FileName} saved successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error saving file {file.FileName}: {ex.Message}");
                return new Result { Success = false, Message = ex.Message };
            }
        }

        public async Task<FileStream> GetExcelFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _loggerService.LogError("File name is null or empty.");
                return null;
            }

            try
            {
                var path = GetExcelFilePath(fileName);
                var fileStream = await _repository.GetAsync(path);
                if (fileStream != null)
                {
                    _loggerService.LogInfo($"File {fileName} retrieved successfully.");
                }
                else
                {
                    _loggerService.LogInfo($"File {fileName} not found.");
                }
                return fileStream;
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error retrieving file {fileName}: {ex.Message}");
                return null;
            }
        }

        public async Task<Result> DeleteExcelFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _loggerService.LogError("File name is null or empty.");
                return new Result { Success = false, Message = "File name is null or empty." };
            }

            try
            {
                var path = GetExcelFilePath(fileName);
                var resultEntity = await _repository.DeleteAsync(path);
                var result = resultEntity.ToDomain();
                if (result.Success)
                {
                    _loggerService.LogInfo($"File {fileName} deleted successfully.");
                }
                else
                {
                    _loggerService.LogInfo($"File {fileName} not found.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error deleting file {fileName}: {ex.Message}");
                return new Result { Success = false, Message = ex.Message };
            }
        }

        /// <summary> Получает полный путь к Excel-файлу. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Полный путь к файлу. </returns>
        private string GetExcelFilePath(string fileName)
        {
            string excelStoragePath = _configuration.GetExcelStoragePath();

            // Проверка на наличие расширения .xlsx
            if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".xlsx";
            }

            string fullPath = Path.Combine(excelStoragePath, fileName);
            return fullPath;
        }

        public async Task SaveJsonFileAsync(string fileName, string jsonContent, string statusCode)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _loggerService.LogError("Invalid input parameters for saving JSON file.");
                return;
            }

            try
            {
                var jsonStoragePath = _configuration.GetJsonStoragePath();
                var filePath = Path.Combine(jsonStoragePath, fileName);
                _jsonWriter.WriteJsonToFile(filePath, jsonContent, statusCode);
                _loggerService.LogInfo($"JSON file {fileName} saved successfully with status code {statusCode}.");
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error saving JSON file {fileName}: {ex.Message}");
            }
        }
    }
}
