namespace ExcelRestService.BL.Domain
{
    /// <summary> Представляет результат операции. </summary>
    public class Result
    {
        /// <summary> Имя файла, связанного с результатом операции. </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary> Флаг успешности операции. </summary>
        public bool Success { get; set; }

        /// <summary> Сообщение, описывающее результат операции. </summary>
        public string Message { get; set; } = string.Empty;
    }
}
