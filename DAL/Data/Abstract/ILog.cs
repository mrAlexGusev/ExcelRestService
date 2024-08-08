namespace ExcelRestService.DAL.Data.Abstract
{
    public interface ILog
    {
        /// <summary> Устанавливает путь к лог-файлу и создает его, если он не существует. </summary>
        /// <param name="path"> Путь к директории, где будет создан лог-файл. </param>
        void UploadPath(string path);

        /// <summary> Записывает сообщение в лог-файл с указанным уровнем. </summary>
        /// <param name="message"> Сообщение для записи в лог. </param>
        /// <param name="level"> Уровень логирования (например, "INFO", "ERROR"). </param>
        void Log(string message, string level);
    }
}
