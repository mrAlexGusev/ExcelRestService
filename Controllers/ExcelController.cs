using ExcelRestService.BL.Services.Interfaces;
using ExcelRestService.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ExcelRestService.Controllers
{
    /// <summary> Контроллер для работы с Excel-файлами. </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        /// <summary> Загружает Excel-файл и обрабатывает его. </summary>
        /// <param name="file"> Файл для загрузки. </param>
        /// <returns> Результат операции загрузки и обработки файла. </returns>
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadExcelFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file uploaded" });
            }

            var serviceResult = await _excelService.ProcessFile(file);
            var result = serviceResult.ToModel();

            return result.Success ? Ok(new { message = result.Message + $" Result file: {result.FileName}" }) : BadRequest(new { error = result.Message });
        }

        /// <summary> Получает Excel-файл по имени. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Файл, если он существует, иначе ошибку. </returns>
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetExcelFile([FromRoute] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new { error = "Empty path" });
            }

            var fileStream = await _excelService.GetExcelFile(fileName);

            if (fileStream == null)
            {
                return NotFound(new { error = "File not found" });
            }

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary> Удаляет Excel-файл по имени. </summary>
        /// <param name="fileName"> Имя файла. </param>
        /// <returns> Результат операции удаления файла. </returns>
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteExcelFile([FromRoute] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new { error = "Empty path" });
            }

            var serviceResult = await _excelService.DeleteExcelFile(fileName);
            var result = serviceResult.ToModel();

            return result.Success ? Ok(new { message = result.Message }) : BadRequest(new { error = result.Message });
        }
    }
}
