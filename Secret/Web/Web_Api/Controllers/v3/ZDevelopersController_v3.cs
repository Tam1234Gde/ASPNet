using Asp.Versioning;
//using AspHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database;
using HIDE.RestAndRant.Web_Api.Auth;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.RestAndRant.Web_Api.Models.v3.ZDevelopers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Для разработчиков
    /// </summary>
    [Tags("ZDevelopers_v3 - Для разработчиков")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/ZDevelopers/[action]")]
    [ApiController]
    public class ZDevelopersController_v3 : ControllerBase
    {
        private readonly ILogger<ZDevelopersController_v3> _log;
        private readonly BL_Context _bl;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="BL"></param>
        public ZDevelopersController_v3(ILogger<ZDevelopersController_v3> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogTrace("Contstructor");
            _bl = BL;
            _db = new DBContext();
        }

        /// <summary>
        /// Возвращает IP-адрес клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string? GetIp()
        {
            return Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }


        /// <summary>
        /// Проверка ведутся тех. работы\городов\ресторанов на работоспособность
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_CheckApp))]
        public JsonResult CheckApp(int? RestaurantID)
        {
            Model_CheckApp? data = new();

            if (RestaurantID.HasValue)
            {
                // Проверяем ведутся ли технические работы
                bool checkTechWorks = HIDE_Convert.ToBool(_bl.Setups.Get(Key: Consts.SetupKey_Dop_CheckApp)?.Val);

                if (checkTechWorks) // Если ведутся технические работы
                {
                    data.ModalShow = true;
                    data.ModalTitle = "Ведутся технические работы";
                    data.ModalText = "Скоро приложение станет доступно. Попробуйте зайти позже.";
                }
                else // Если технические работы не ведутся
                {
                    // Получаем список всех городов
                    List<CCity>? listCities = _bl.Cities.GetList();

                    if (listCities != null && listCities.Count == 0 || listCities == null)
                    {
                        return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err, Data: data);
                    }

                    // Если все города скрыты
                    if (listCities.All(p => p.Flag_Hidden))
                    {
                        // Получаем данные для модального окна, когда все города скрыты
                        CSetup? setup = _bl.Setups.Get(Key: Consts.SetupKey_Dop_HiddenAllCities);

                        if (setup == null)
                        {
                            return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err, Data: data);
                        }

                        data.ModalShow = true;
                        data.ModalTitle = setup.Val;
                        data.ModalText = setup.Val2;
                    }
                    else
                    {
                        // Получаем город по коду ресторана
                        CCity? city = _bl.Cities.GetByRestaurantID(RestaurantID: RestaurantID.Value);

                        if (city == null)
                        {
                            return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err, Data: data);
                        }

                        // Если город скрыт
                        if (city.Flag_Hidden)
                        {
                            data.ModalShow = true;
                            data.ModalTitle = city.HiddenTitle;
                            data.ModalText = city.HiddenText;
                            data.ModalButtonText = "Поменять город";
                        }
                        else
                        {
                            // Получаем ресторан
                            CRestaurant? restaurant = _bl.Restaurants.Get(ID: RestaurantID.Value);

                            if (restaurant == null)
                            {
                                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err, Data: data);
                            }

                            // Если ресторан скрыт
                            if (restaurant.Flag_Hidden)
                            {
                                data.ModalShow = true;
                                data.ModalTitle = restaurant.HiddenTitle;
                                data.ModalText = restaurant.HiddenText;
                                data.ModalButtonText = "Поменять адрес";
                                data.CityID = restaurant.CityID;
                            }
                            else
                            {
                                data.ModalShow = false;
                            }
                        }
                    }
                }
            }
            else
            {
                // Проверяем ведутся ли технические работы
                bool checkTechWorks = HIDE_Convert.ToBool(_bl.Setups.Get(Key: Consts.SetupKey_Dop_CheckApp)?.Val);

                if (checkTechWorks) // Если ведутся технические работы
                {
                    data.ModalShow = true;
                    data.ModalTitle = "Ведутся технические работы";
                    data.ModalText = "Скоро приложение станет доступно. Попробуйте зайти позже.";
                }
                else
                {
                    data.ModalShow = false;
                }
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: data);
        }


        /// <summary>
        /// Возвращает сумму при которой доставка становится бесплатной
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        public JsonResult GetDelivery_PriceFree()
        {
            decimal? val = HIDE_Convert.ToDecimal(_bl.Setups.Get(Key: Consts.SetupKey_Dop_Delivery_PriceFree)?.Val);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает текст и версию приложения IOS для модального окна "Обновить приложение"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_AppUpdateData_v3))]
        public JsonResult GetAppIOSUpdateData()
        {
            Model_AppUpdateData_v3 model = new()
            {
                Text = _bl.Setups.Get(Key: Consts.SetupKey_Dop_AppUpdate)?.Val ?? String.Empty,
                Version = _bl.Setups.Get(Key: Consts.SetupKey_Dop_AppUpdateVersionIOS)?.Val ?? String.Empty
            };
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: model);
        }


        /// <summary>
        /// Возвращает версию приложения Android для модального окна "Обновить приложение"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_AppAndroidUpdateData_v3))]
        public JsonResult GetAppAndroidUpdateData()
        {
            Model_AppAndroidUpdateData_v3 model = new()
            {
                Version = _bl.Setups.Get(Key: Consts.SetupKey_Dop_AppUpdateVersionAndroid)?.Val ?? String.Empty
            };
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: model);
        }


        /// <summary>
        /// Возвращает текст при бронировании столиков(Бронировать можно на ближайшие N месяцев)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        public JsonResult GetBooking_Text()
        {
            string? val = _bl.Setups.Get(Key: Consts.SetupKey_Dop_BookingText)?.Val;
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает возможных статусов заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon_Ordered>))]
        public JsonResult GetOrderStatusesList()
        {
            List<CCommon_Ordered>? list_result = _bl.OrderStatuses.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает возможных способов получения заказа(С собой\Поесть в кафе\Доставка\Вынести к машине)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetOrderTypesList()
        {
            List<CCommon>? list_result = _bl.Order_Types.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает список видов оплаты (картой, нал, ...)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetPayTypesList()
        {
            List<CCommon>? list_result = _bl.Pay_Types.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Записывает лог в базу данных
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_Logs_Write_v3))]
        public JsonResult AddLog(Model_Logs_Write_v3 Data)
        {
            string message = Data.Message;
            if (message.Length > 5000)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            if (!Enum.IsDefined(typeof(LogLevel), Data.LogLevel))
            {

                _log.LogError(message: "{}", args: $"Неизвестный тип лога {Data.LogLevel}");


                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            LogLevel level = (LogLevel)Data.LogLevel;


            if (_log.IsEnabled(level))
            {
                _log.Log(level, message: "{}", args: $"{Data.UserId} {Data.Message}".Trim());
            }


            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: 0);
        }


        /// <summary>
        /// Выполнение скрипта
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuth]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Выполнение скрипта")]
        public JsonResult ExecuteScript(Model_Execute_Script_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            if (Data != null && Data.Guid == AppConfig.Guid_Script && AppConfig.Allow_Script)
            {
                int result = 0;
                if (!string.IsNullOrWhiteSpace(Data.SqlText))
                {
                    result = _db.Database.ExecuteSqlRaw(Data.SqlText);
                }

                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
            }
            else
            {
                _log.LogWarning(message: "Требуется проверка Guid_Script и Allow_Script");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

        }


//        /// <summary>
//        /// Загружает и выполняет скрипт
//        /// </summary>
//        /// <param name="scriptGuid">Guid</param>
//        /// <param name="formFile">Файл для выполнения</param>
//        /// <returns></returns>
//        [HttpPost]
//#if DEBUG
//        [AllowAnonymous]
//#else
//        [BasicAuth]
//#endif
//        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Выполнение скрипта")]
//        [Produces("application/json")]
//        [Consumes("multipart/form-data")]
//        public async Task<JsonResult> UploadAndExecuteScript([FromForm] string scriptGuid, IFormFile formFile)
//        {
//            if (!AppConfig.Allow_Script)
//            {
//                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
//            }

//#if !DEBUG
//            CCommon? client = Helper_Client.GetClient(HttpContext.User);
//            if (client == null)
//            {
//                _log.LogWarning(message: "Не удалось идентифицировать клиента");
//                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
//            }
//#endif

//            if (scriptGuid != AppConfig.Guid_Script)
//            {
//                _log.LogWarning(message: "Ключ для выполнения запроса не совпадает");
//                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
//            }
//            using StreamReader sr = new(formFile.OpenReadStream());
//            string content = await sr.ReadToEndAsync();
//            //string resultMessage = _db.ExecuteScript(content);

//            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: 205, Data: resultMessage);
//        }


        /// <summary>
        /// Возвращает название последней миграции.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetLastMigration()
        {
            string? result = Task.Run(() => _bl.ZDeveloper.GetLastMigration()).Result;

            if (result != null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
            }
            else
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err, Data: null);
            }
        }
    }
}