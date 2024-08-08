using ExcelRestService.BL.Domain;
using ExcelRestService.DAL.Entyties;
using ExcelRestService.Models;

namespace ExcelRestService.Mappers
{
    public static class ResultMapper
    {
        public static Result ToDomain(this ResultEntity entity)
        {
            var result = new Result
            {
                Success = entity.Success,
                Message = entity.Message,
                FileName = entity.FileName
            };

            return result;
        }

        public static ResultModel ToModel(this Result result)
        {
            var resultModel = new ResultModel
            {
                Success = result.Success,
                Message = result.Message,
                FileName = result.FileName
            };

            return resultModel;
        }
    }
}
