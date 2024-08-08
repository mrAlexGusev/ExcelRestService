using ExcelRestService.BL.Services.Interfaces;
using ExcelRestService.DAL.Data.Abstract;

namespace ExcelRestService.BL.Services
{
    /// <summary> Сервис для логирования сообщений. </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILog _logger;

        public LoggerService(IConfigurationService configurationService, ILog logger)
        {
            string logFilePath = configurationService.GetLogStoragePath();
            _logger = logger;
            _logger.UploadPath(logFilePath);
        }

        public void LogInfo(string message)
        {
            _logger.Log(message, "INFO");
        }

        public void LogError(string message)
        {
            _logger.Log(message, "ERROR");
        }
    }
}
