using ExcelRestService.BL.Domain;
using ExcelRestService.BL.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace ExcelRestService.BL.Services
{
    /// <summary> Сервис для отправки файлов через HTTP. </summary>
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggerService _loggerService;
        private readonly IConfigurationService _configuration;
        private readonly IFileService _fileService;

        public HttpService(HttpClient httpClient, ILoggerService loggerService, IConfigurationService configuration, IFileService fileService)
        {
            _httpClient = httpClient;
            _loggerService = loggerService;
            _configuration = configuration;
            _fileService = fileService;
        }

        /// <summary> Получает UUID с помощью GET-запроса к /uuid. </summary>
        /// <returns> UUID или null, если запрос не удался. </returns>
        public async Task<string> GetUuidAsync()
        {
            var uuidUrl = $"{_configuration.GetHttpBinUrl()}/uuid";
            var uuidResponse = await _httpClient.GetAsync(uuidUrl);

            if (uuidResponse.IsSuccessStatusCode)
            {
                var uuidResponseContent = await uuidResponse.Content.ReadAsStringAsync();
                var uuidObject = JsonConvert.DeserializeObject<dynamic>(uuidResponseContent);
                return uuidObject?.uuid;
            }
            else
            {
                _loggerService.LogError("Failed to retrieve UUID from httpbin.org/uuid");
                return null;
            }
        }

        public async Task<Result> SendBase64FileAsync(string base64File, string fileName)
        {
            if (string.IsNullOrEmpty(base64File))
            {
                _loggerService.LogError("Base64 file content is null or empty.");
                return new Result { Success = false, Message = "Base64 file content is null or empty." };
            }

            try
            {
                // Получение UUID с помощью GET-запроса к /uuid
                var uuid = await GetUuidAsync();

                // Если UUID не получен, используем fileName для формирования имени файла
                var jsonFileName = uuid != null
                    ? $"{uuid}.json"
                    : $"{Path.GetFileNameWithoutExtension(fileName)}.json";

                // Отправка POST-запроса с base64File
                var url = $"{_configuration.GetHttpBinUrl()}/post";
                var content = new StringContent(base64File, Encoding.UTF8, "application/octet-stream");
                var response = await _httpClient.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseStatusCode = response.StatusCode;

                await _fileService.SaveJsonFileAsync(jsonFileName, responseContent, responseStatusCode.ToString());

                if (response.IsSuccessStatusCode)
                {
                    _loggerService.LogInfo("File sent successfully.");
                    return new Result { Success = true, Message = "File sent successfully.", FileName = jsonFileName };
                }
                else
                {
                    _loggerService.LogError($"Error sending file: {responseContent}");
                    return new Result { Success = false, Message = responseContent, FileName = jsonFileName };
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error sending file: {ex.Message}");
                return new Result { Success = false, Message = ex.Message };
            }
        }
    }
}
