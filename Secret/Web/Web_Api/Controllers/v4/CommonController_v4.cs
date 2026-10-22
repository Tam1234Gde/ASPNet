using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.RestAndRant.Web_Api.Models.v3.ZDevelopers;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.RestAndRant.Web_Api.Models.v4.ZDevelopers;
using HIDE.Units_Core;
using Model_CheckApp = HIDE.RestAndRant.Web_Api.Models.v4.ZDevelopers.Model_CheckApp;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{

    /// <summary>
    /// Контроллер - Цвета посуды блюд
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("CommonController_v4 - Общий контроллер")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Common/[action]")]
    [ApiController]
    public class CommonController_v4(ILogger<CommonController_v4> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает версию приложения Android для модального окна "Обновить приложение"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_AppAndroidUpdateData_v4))]
        public JsonResult GetAppAndroidUpdateData()
        {
            Model_AppAndroidUpdateData_v4 model = new()
            {
                Version = _bl.Setups.Get(Key: Consts.SetupKey_Dop_AppUpdateVersionAndroid)?.Val ?? String.Empty
            };
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: model);
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: val);
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: Consts.Status_Success, Data: list_result);
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
                        return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: data);
                    }

                    // Если все города скрыты
                    if (listCities.All(p => p.Flag_Hidden))
                    {
                        // Получаем данные для модального окна, когда все города скрыты
                        CSetup? setup = _bl.Setups.Get(Key: Consts.SetupKey_Dop_HiddenAllCities);

                        if (setup == null)
                        {
                            return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: data);
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
                            return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: data);
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
                                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: data);
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

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: data);
        }


        /// <summary>
        /// Записывает лог в базу данных
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_Logs_Write_v4))]
        public JsonResult AddLog(Model_Logs_Write_v4 Data)
        {
            string message = Data.Message;
            if (message.Length > 5000)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            if (!Enum.IsDefined(typeof(LogLevel), Data.LogLevel))
            {
                _log.LogError(message: "{}", args: $"Неизвестный тип лога {Data.LogLevel}");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            LogLevel level = (LogLevel)Data.LogLevel;

            if (_log.IsEnabled(logLevel: level))
            {
                _log.Log(level, message: "{}", args: $"{Data.UserId} {Data.Message}".Trim());
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: 0);
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



    }
}
