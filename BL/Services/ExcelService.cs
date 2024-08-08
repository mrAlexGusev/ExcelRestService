using ExcelRestService.BL.Domain;
using ExcelRestService.BL.Services.Interfaces;
using OfficeOpenXml;

namespace ExcelRestService.BL.Services
{
    /// <summary> Сервис для работы с Excel-файлами. </summary>
    public class ExcelService : IExcelService
    {
        private readonly IFileService _fileService;
        private readonly ILoggerService _loggerService;
        private readonly IHttpService _httpService;

        public ExcelService(IFileService fileService, ILoggerService loggerService, IHttpService httpService)
        {
            _fileService = fileService;
            _loggerService = loggerService;
            _httpService = httpService;

            // Установка контекста лицензии для библиотеки EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<Result> SaveExcelFile(IFormFile file)
        {
            if (file == null)
            {
                _loggerService.LogError("File is null.");
                return new Result { Success = false, Message = "File is null." };
            }

            try
            {
                var result = await _fileService.SaveExcelFileAsync(file);
                _loggerService.LogInfo($"File {file.FileName} saved successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error saving file {file.FileName}: {ex.Message}");
                return new Result { Success = false, Message = ex.Message };
            }
        }

        public async Task<FileStream> GetExcelFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _loggerService.LogError("File name is null or empty.");
                return null;
            }

            try
            {
                var fileStream = await _fileService.GetExcelFileAsync(fileName);
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

        public async Task<Result> DeleteExcelFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _loggerService.LogError("File name is null or empty.");
                return new Result { Success = false, Message = "File name is null or empty." };
            }

            try
            {
                var result = await _fileService.DeleteExcelFileAsync(fileName);
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

        /// <summary> Асинхронно транспонирует Excel-файл. </summary>
        /// <param name="file"> Файл для транспонирования. </param>
        /// <returns> Транспонированный файл, если операция успешна, иначе null. </returns>
        private async Task<IFormFile> TransposeExcelFile(IFormFile file)
        {
            if (file == null)
            {
                _loggerService.LogError("File is null.");
                return null;
            }

            try
            {
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var transposedWorksheet = package.Workbook.Worksheets.Add("Transposed");

                    // Транспонирование данных
                    for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                    {
                        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            transposedWorksheet.Cells[col, row].Value = worksheet.Cells[row, col].Value;
                        }
                    }

                    // Удаление исходного листа и переименование транспонированного
                    package.Workbook.Worksheets.Delete(worksheet);
                    transposedWorksheet.Name = worksheet.Name;

                    var memoryStream = new MemoryStream();
                    package.SaveAs(memoryStream);
                    memoryStream.Position = 0;

                    var transposedFile = new FormFile(memoryStream, 0, memoryStream.Length, file.Name, file.FileName);
                    return transposedFile;
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error transposing file {file.FileName}: {ex.Message}");
                return null;
            }
        }

        /// <summary> Асинхронно конвертирует Excel-файл в base64. </summary>
        /// <param name="file"> Файл для конвертации. </param>
        /// <returns> Содержимое файла в формате base64. </returns>
        private async Task<string> ConvertExcelToBase64(IFormFile file)
        {
            if (file == null)
            {
                _loggerService.LogError("File is null.");
                return null;
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error converting file {file.FileName} to base64: {ex.Message}");
                return null;
            }
        }

        public async Task<Result> ProcessFile(IFormFile file)
        {
            if (file == null)
            {
                _loggerService.LogError("File is null.");
                return new Result { Success = false, Message = "File is null." };
            }

            // Проверка, что файл является файлом Excel
            if (!IsExcelFile(file))
            {
                _loggerService.LogError("File is not an Excel file.");
                return new Result { Success = false, Message = "File is not an Excel file." };
            }

            try
            {
                // Транспонирование файла
                var transposedFile = await TransposeExcelFile(file);
                if (transposedFile == null)
                {
                    return new Result { Success = false, Message = "Error transposing file." };
                }

                // Сохранение транспонированного файла
                var saveResult = await SaveExcelFile(transposedFile);
                if (!saveResult.Success)
                {
                    return saveResult;
                }

                // Преобразование файла в base64
                var base64File = await ConvertExcelToBase64(transposedFile);
                if (base64File == null)
                {
                    return new Result { Success = false, Message = "Error converting file to base64." };
                }

                // Отправка файла на указанный URL
                var sendResult = await _httpService.SendBase64FileAsync(base64File, file.FileName);
                return sendResult;
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error processing and sending file {file.FileName}: {ex.Message}");
                return new Result { Success = false, Message = ex.Message };
            }
        }

        /// <summary> Проверяет, является ли файл файлом Excel. </summary>
        /// <param name="file"> Файл для проверки. </param>
        /// <returns> True, если файл является файлом Excel, иначе False. </returns>
        private bool IsExcelFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".xls", ".xlsx" };
            var allowedMimeTypes = new[] { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var fileMimeType = file.ContentType.ToLower();

            return allowedExtensions.Contains(fileExtension) && allowedMimeTypes.Contains(fileMimeType);
        }
    }
}
