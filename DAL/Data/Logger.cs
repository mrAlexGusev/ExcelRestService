using ExcelRestService.DAL.Data.Abstract;

namespace ExcelRestService.DAL.Data
{
    /// <summary> Класс для логирования сообщений в файл. </summary>
    public class Logger : ILog
    {
        private string _logFilePath = string.Empty;

        /// <summary> Записывает сообщение в лог-файл с указанным уровнем. </summary>
        /// <param name="message"> Сообщение для записи в лог. </param>
        /// <param name="level"> Уровень логирования (например, "INFO", "ERROR"). </param>
        public void Log(string message, string level)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error writing to log file: {ex.Message}", ex);
            }
        }

        /// <summary> Устанавливает путь к лог-файлу и создает его, если он не существует. </summary>
        /// <param name="path"> Путь к директории, где будет создан лог-файл. </param>
        public void UploadPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.");
            }

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string logFileName = $"{DateTime.Now:yyyyMMdd}.log";
                _logFilePath = Path.Combine(path, logFileName);

                if (!File.Exists(_logFilePath))
                {
                    File.Create(_logFilePath).Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error setting log file path: {ex.Message}", ex);
            }
        }
    }
}
