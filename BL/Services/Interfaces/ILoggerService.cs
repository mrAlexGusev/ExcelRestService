namespace ExcelRestService.BL.Services.Interfaces
{
    public interface ILoggerService
    {
        /// <summary> Логирует информационное сообщение. </summary>
        /// <param name="message"> Сообщение для логирования. </param>
        void LogInfo(string message);

        /// <summary> Логирует сообщение об ошибке. </summary>
        /// <param name="message"> Сообщение для логирования. </param>
        void LogError(string message);
    }
}
