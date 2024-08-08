using ExcelRestService.BL.Services.Interfaces;

namespace ExcelRestService.BL.Services
{
    /// <summary> Сервис для получения конфигурационных значений. </summary> 
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetExcelStoragePath()
        {
            return GetConfigurationValue("Paths:Excel", "Excel storage path is not configured.");
        }

        public string GetJsonStoragePath()
        {
            return GetConfigurationValue("Paths:JSON", "JSON storage path is not configured.");
        }

        public string GetLogStoragePath()
        {
            return GetConfigurationValue("Paths:Logs", "Log storage path is not configured.");
        }

        public string GetHttpBinUrl()
        {
            return GetConfigurationValue("HttpBinUrl", "HttpBin URL is not configured.");
        }

        /// <summary> Получает значение конфигурации по ключу. </summary>
        /// <param name="key"> Ключ конфигурации. </param>
        /// <param name="errorMessage"> Сообщение об ошибке, если значение не найдено. </param>
        /// <returns> Значение конфигурации. </returns>
        private string GetConfigurationValue(string key, string errorMessage)
        {
            var value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException(errorMessage);
            }
            return value;
        }
    }
}
