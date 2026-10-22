using Microsoft.AspNetCore.Mvc;
using System.Net;
using HIDE.RestAndRant.Web_Api.Models.v3;

namespace HIDE.RestAndRant.Web_Api.Helpers
{
    /// <summary>
    /// Помощник для формирования ответа в Json
    /// </summary>
    public static class Helper_Result
    {
        /// <summary>
        /// Получение ответа JSON на базе параметров
        /// </summary>
        /// <param name="Status">Статус</param>
        /// <param name="Code">Код</param>
        /// <param name="Data">Данные</param>
        /// <param name="Message">Сообщение, доступно только в Debug</param>
        /// <returns></returns>
        public static JsonResult GetJsonResult(string Status, int Code, object? Data = null, string? Message = null)
        {
            Result_v3 result;

            result = new(Status_Val: Status)
            {
                Code = Code,
                Data = Data,
#if DEBUG
                Message = Message
#endif
            };

            int code = 200;
            if (Status == "OK" && Code > 0)
            {
                code = (int)HttpStatusCode.OK;
            }
            else
            {
                code = (int)HttpStatusCode.BadRequest;
            }

            return new JsonResult(result)
            {
                StatusCode = code
            };
        }
    }
}