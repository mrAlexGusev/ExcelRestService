using ExcelRestService.DAL.Data.Abstract;
using ExcelRestService.DAL.Entyties;

namespace ExcelRestService.DAL.Data
{
    /// <summary> Репозиторий для работы с Excel файлами. </summary>
    public class ExcelRepository : IRepository
    {
        public async Task<ResultEntity> SaveAsync(IFormFile file, string path)
        {
            if (file == null)
            {
                return new ResultEntity { Success = false, Message = "File is null." };
            }

            if (string.IsNullOrEmpty(path))
            {
                return new ResultEntity { Success = false, Message = "Path is null or empty." };
            }

            try
            {
                var directoryPath = Path.GetDirectoryName(path);

                // Создание директории, если она не существует
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Создание и запись файла
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new ResultEntity { Success = true, Message = $"File saved successfully" };
            }
            catch (Exception ex)
            {
                return new ResultEntity { Success = false, Message = ex.Message };
            }
        }

        public async Task<FileStream> GetAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            try
            {
                if (File.Exists(path))
                {
                    return new FileStream(path, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResultEntity> DeleteAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new ResultEntity { Success = false, Message = "File path is null or empty." };
            }

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return new ResultEntity { Success = true, Message = "File deleted successfully" };
                }
                else
                {
                    return new ResultEntity { Success = false, Message = "File not found" };
                }
            }
            catch (Exception ex)
            {
                return new ResultEntity { Success = false, Message = ex.Message };
            }
        }
    }
}
