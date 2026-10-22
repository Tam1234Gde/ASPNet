using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Setups;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    public class SetupsController : Controller
    {
        private readonly ILogger<SetupsController> _log;
        private readonly Setups _bl_Setups;

        public SetupsController(ILogger<SetupsController> logger)
        {
            _log = logger;
            _bl_Setups = new Setups();
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Flag_ShowMenu = true;
            ViewBag.RestaurantID = null;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех настроек
        /// GET: /Setups/Setups_List
        /// </summary>
        [HttpGet]
        [Route("Setups_List")]
        public IActionResult GetSetups()
        {
            try
            {
                List<CSetup> setups = _bl_Setups.GetList();

                List<object> result = new List<object>();
                foreach (CSetup item in setups)
                {
                    result.Add(new
                    {
                        item.Key,
                        item.Val,
                        item.Val_Comment,
                        item.Val2,
                        item.Val2_Comment,
                        item.Val3,
                        item.Val3_Comment,
                        item.Val4,
                        item.Val4_Comment
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить настройку по ключу
        /// GET: /Setups/GetSetup?Key={key}
        /// </summary>
        [HttpGet]
        public IActionResult GetSetup(string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Key))
                    return Json(new { success = false, message = "Неверный ключ" });

                CSetup setup = _bl_Setups.Get(Key: Key);
                if (setup == null)
                    return Json(new { success = false, message = "Настройка не найдена" });

                object result = new
                {
                    setup.Key,
                    setup.Val,
                    setup.Val_Comment,
                    setup.Val2,
                    setup.Val2_Comment,
                    setup.Val3,
                    setup.Val3_Comment,
                    setup.Val4,
                    setup.Val4_Comment
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить настройку по ключу (только значение)
        /// GET: /Setups/GetSetupValue?Key={key}
        /// </summary>
        [HttpGet]
        public IActionResult GetSetupValue(string Key)
        {
            try
            {
                if (string.IsNullOrEmpty(Key))
                    return Json(new { success = false, message = "Неверный ключ" });

                CSetup setup = _bl_Setups.Get(Key: Key);
                if (setup == null)
                    return Json(new { success = false, message = "Настройка не найдена" });

                return Json(new { success = true, data = new { Key, Value = setup.Val } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        #region Список

        /// <summary>
        /// Список настроек
        /// </summary>
        public IActionResult Setups_List()
        {
            List<CSetup> setups = _bl_Setups.GetList();
            return View(setups);
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Изменение - GET
        /// </summary>
        [HttpGet]
        public IActionResult Setups_Edit(string Key)
        {
            CSetup? elem = _bl_Setups.Get(Key: Key);

            if (elem == null)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { message = Errors.Err200_str, code = Errors.Err200 });
            }

            Model_Setups_AddEdit data = new()
            {
                Key = elem.Key,
                Val1 = elem.Val,
                Val1_Comment = elem.Val_Comment,
                Val2 = elem.Val2,
                Val2_Comment = elem.Val2_Comment,
                Val3 = elem.Val3,
                Val3_Comment = elem.Val3_Comment,
                Val4 = elem.Val4,
                Val4_Comment = elem.Val4_Comment,
                Flag = elem.Key == Consts.SetupKey_Dop_CheckApp && HIDE_Convert.ToBool(elem.Val)
            };

            return View(model: data);
        }

        /// <summary>
        /// Изменение - POST
        /// </summary>
        [HttpPost]
        public IActionResult Setups_Edit(Model_Setups_AddEdit data)
        {
            if (data == null)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model: data);
            }

            string? keyVal1 = data.Val1;

            // Для чекбокса "Проверка приложения"
            if (data.Key == Consts.SetupKey_Dop_CheckApp)
            {
                keyVal1 = data.Flag.ToString().ToLower();
            }

            // Для часов работы по умолчанию - собираем выбранные часы
            if (data.Key == Consts.SetupKey_DefaultWorkHours)
            {
                string[] selectedHours = Request.Form["SelectedHours"].ToArray();
                keyVal1 = string.Join(",", selectedHours);
            }


            // Для часов работы по умолчанию - собираем выбранные часы
            if (data.Key == Consts.SetupKey_Restaurants_DefaultWorkHours)
            {
                string[] selectedHours = Request.Form["SelectedHours"].ToArray();
                keyVal1 = string.Join(",", selectedHours);
            }

            // Для чекбокса "Использовать часы работы по умолчанию"
            if (data.Key == Consts.SetupKey_UseDefaultWorkHours)
            {
                keyVal1 = data.Flag.ToString().ToLower();
            }

            // Для номера телефона - просто строка
            if (data.Key == Consts.SetupKey_DefaultPhoneNumber)
            {
                keyVal1 = data.Val1?.Trim();
            }

            bool check = _bl_Setups.Edit(
                Key: data.Key,
                Val: keyVal1,
                Val_Comment: data.Val1_Comment,
                Val2: data.Val2,
                Val2_Comment: data.Val2_Comment,
                Val3: data.Val3,
                Val3_Comment: data.Val3_Comment,
                Val4: data.Val4,
                Val4_Comment: data.Val4_Comment
            );

            if (!check)
            {
                _log.LogError(message: "{}", args: $"ADM # Ошибка изменения настройки # {data.Key} #");
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { message = Errors.Err203_str, code = Errors.Err203 });
            }

            TempData["Success"] = "Настройка успешно обновлена.";
            return RedirectToAction("Setups_List");
        }

        #endregion
    }
}