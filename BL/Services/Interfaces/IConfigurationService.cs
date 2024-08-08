namespace ExcelRestService.BL.Services.Interfaces
{
    public interface IConfigurationService
    {
        /// <summary> Получает путь к хранилищу Excel файлов. </summary>
        /// <returns> Путь к хранилищу Excel файлов. </returns>
        string GetExcelStoragePath();

        /// <summary> Получает путь к хранилищу JSON файлов. </summary>
        /// <returns> Путь к хранилищу JSON файлов. </returns>
        string GetJsonStoragePath();

        /// <summary> Получает путь к хранилищу лог-файлов. </summary>
        /// <returns> Путь к хранилищу лог-файлов. </returns>
        string GetLogStoragePath();

        /// <summary> Получает URL для HttpBin. </summary>
        /// <returns> URL для HttpBin. </returns>
        string GetHttpBinUrl();
    }
}
