using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Opers;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Clients;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        readonly Client_oper _dl_Client = new();
        private readonly IWebHostEnvironment _env;
        private int client_id;
        readonly Images Images_BL;


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех клиентов
        /// GET: /Clients
        /// </summary>
        [HttpGet]
        public IActionResult GetClients()
        {
            try
            {
                List<CClient> clients = _bl.Clients.GetList_ForAdmin();

                List<object> result = new List<object>();
                foreach (CClient item in clients)
                {
                    string imageUrl = Images_BL.Get_Client(ClientID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Phone,
                        item.Name,
                        item.Date_Birth,
                        item.Flag_IsPolicy,
                        item.Date_LastZakaz,
                        item.Guid,
                        Bonuses = _bl.Bonuses.Check_Bonuses(item.ID),
                        item.Flag_Del,
                        item.Date_LastUpdate,
                        item.Date_Registration,
                        item.SmsCode,
                        item.CardData,
                        item.Date_LastAction,
                        item.Date_PhotoUpdate,
                        item.Delivery_Address,
                        item.Delivery_Apartment,
                        item.Delivery_EntranceCode,
                        item.Delivery_EntranceNumber,
                        item.Delivery_Floor,
                        item.Name_Another,
                        item.Phone_Another,
                        item.Car_Brand,
                        item.Car_RegNumber,
                        item.Car_Color,
                        Image_Url = imageUrl ?? string.Empty
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
        /// Получить клиента по ID
        /// GET: /Clients/GetClient?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetClient(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CClient client = _bl.Clients.Get(ID: ID);
                if (client == null)
                    return Json(new { success = false, message = "Клиент не найден" });

                string imageUrl = Images_BL.Get_Client(ClientID: client.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                object result = new
                {
                    client.ID,
                    client.Phone,
                    client.Name,
                    client.Date_Birth,
                    client.Flag_IsPolicy,
                    client.Date_LastZakaz,
                    client.Guid,
                    Bonuses = _bl.Bonuses.Check_Bonuses(client.ID),
                    client.Flag_Del,
                    client.Date_LastUpdate,
                    client.Date_Registration,
                    client.SmsCode,
                    client.CardData,
                    client.Date_LastAction,
                    client.Date_PhotoUpdate,
                    client.Delivery_Address,
                    client.Delivery_Apartment,
                    client.Delivery_EntranceCode,
                    client.Delivery_EntranceNumber,
                    client.Delivery_Floor,
                    client.Name_Another,
                    client.Phone_Another,
                    client.Car_Brand,
                    client.Car_RegNumber,
                    client.Car_Color,
                    Image_Url = imageUrl ?? string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить клиента по номеру телефона
        /// GET: /Clients/GetClientByPhone?Phone={phone}
        /// </summary>
        [HttpGet]
        public IActionResult GetClientByPhone(string Phone)
        {
            try
            {
                if (string.IsNullOrEmpty(Phone))
                    return Json(new { success = false, message = "Неверный номер телефона" });

                List<CClient> clients = _bl.Clients.GetList_ForAdmin();

                CClient client = null;
                foreach (CClient item in clients)
                {
                    if (item.Phone == Phone)
                    {
                        client = item;
                        break;
                    }
                }

                if (client == null)
                    return Json(new { success = false, message = "Клиент не найден" });

                string imageUrl = Images_BL.Get_Client(ClientID: client.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                object result = new
                {
                    client.ID,
                    client.Phone,
                    client.Name,
                    client.Date_Birth,
                    client.Flag_IsPolicy,
                    client.Date_LastZakaz,
                    client.Guid,
                    Bonuses = _bl.Bonuses.Check_Bonuses(client.ID),
                    client.Flag_Del,
                    client.Date_LastUpdate,
                    client.Date_Registration,
                    client.SmsCode,
                    client.CardData,
                    client.Date_LastAction,
                    client.Date_PhotoUpdate,
                    client.Delivery_Address,
                    client.Delivery_Apartment,
                    client.Delivery_EntranceCode,
                    client.Delivery_EntranceNumber,
                    client.Delivery_Floor,
                    client.Name_Another,
                    client.Phone_Another,
                    client.Car_Brand,
                    client.Car_RegNumber,
                    client.Car_Color,
                    Image_Url = imageUrl ?? string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Определение ID клиента
        /// </summary>
        private int ClientID
        {
            get
            {
                if (client_id <= 0 && HttpContext != null)
                {
                    client_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return client_id;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public ClientsController(ILogger<ClientsController> Logger, BL_Context BL, IWebHostEnvironment Env)
        {
            _log = Logger;
            _log.LogDebug("Constructor ClientsController");
            _bl = BL;
            _env = Env;
            Images_BL = new Images();
        }

        /// <summary>
        /// Список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            List<CClient> list_clients = _bl.Clients.GetList_ForAdmin();
            //List<Model_Client> list_clients_result = [];
            //foreach (var item in list_clients)
            //{
            //    Model_Client client = new()
            //    {
            //        ID = item.ID,
            //        Name = item.Name,
            //        Phone = item.Phone,
            //        Date_Birth = item.Date_Birth,
            //        Flag_IsPolicy = item.Flag_IsPolicy,
            //        Date_LastZakaz = item.Date_LastZakaz,
            //        Guid = item.Guid, 
            //        Bonuses = _bl.Bonuses.Check_Bonuses(item.ID),
            //        Flag_Del = item.Flag_Del,
            //        Date_LastUpdate = item.Date_LastUpdate,
            //        Date_Registration = item.Date_Registration,
            //        SmsCode = item.SmsCode,
            //        CardData = item.CardData,
            //        Date_LastAction = item.Date_LastAction,
            //        Date_PhotoUpdate = item.Date_PhotoUpdate,
            //    };

            //    string? image_url = Images_BL.Get_Client(ClientID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
            //    if (!string.IsNullOrWhiteSpace(image_url))
            //    {
            //        client.Image_Url = image_url;
            //    }
            //    list_clients_result.Add(client);
            //}
            return View(model: list_clients);
        }

        /// <summary>
        /// Таблица > Список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Clients_Part_Table()
        {
            return Index();
        }

        #region Добавление

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Clients_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                DateTime? date = HIDE_Convert.ToDateTime_Nullable(data.Date_Birth);

                CClient? client = _bl.Clients.Add(Phone: data.Phone, Name: data.Name ?? "", Date_Birth: date, Photo: null, Flag_IsPolicy: data.Flag_IsPolicy,
                                                  Delivery_Address: data.Delivery_Address, Delivery_Apartment: data.Delivery_Apartment,
                                                  Delivery_EntranceCode: data.Delivery_EntranceCode, Delivery_EntranceNumber: data.Delivery_EntranceNumber,
                                                  Delivery_Floor: data.Delivery_Floor);

                if (client == null)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = client });
                }

                if (data.UploadImge != null)
                {
                    byte[] image_data = Helper_Image.Add_Client(ClientID: client.ID, CurrentFile: data.UploadImge, ContentRootPath: _env.ContentRootPath);
                    _bl.Clients.Edit_Image(ID: client.ID, Client_Image: image_data);
                }

                return RedirectToAction("Index"); //Возвращаем к списку клиентов
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID, bool Flag_IsErrorDelPhoto)
        {
            CClient? client = _bl.Clients.Get(ID: ID);
            if (client != null)
            {
                if (Flag_IsErrorDelPhoto)
                {
                    ModelState.AddModelError("Image_Url", "Ошибка удаления изображения");
                }

                Model_Clients_AddEdit data = new() //TODO: KA - подойти к KMI и обсудить это AutoMapper
                {
                    ID = client.ID,
                    Phone = client.Phone,
                    Name = client.Name,
                    Date_Birth = string.Format("{0:yyyy-MM-dd}", client.Date_Birth),
                    SmsCode = client.SmsCode,
                    Flag_IsPolicy = client.Flag_IsPolicy,
                    Guid = client.Guid,
                    Image = client.Photo,
                    Delivery_Floor = client.Delivery_Floor,
                    Delivery_Address = client.Delivery_Address,
                    Delivery_EntranceNumber = client.Delivery_EntranceNumber,
                    Delivery_EntranceCode = client.Delivery_EntranceCode,
                    Delivery_Apartment = client.Delivery_Apartment,
                    Name_Another = client.Name_Another,
                    Phone_Another = client.Phone_Another,
                    Car_Color = client.Car_Color,
                    Car_RegNumber = client.Car_RegNumber,
                    Car_Brand = client.Car_Brand,
                };

                string? image_url = Images_BL.Get_Client(ClientID: data.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                if (!string.IsNullOrWhiteSpace(image_url))
                {
                    data.Image_Url = image_url;
                }

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Clients_AddEdit data)
        {
            if (data != null)
            {
                DateTime? date = HIDE_Convert.ToDateTime_Nullable(data.Date_Birth);

                // Преобразование из IFormFile в byte[]
                byte[]? img_Data = null;
                if (data.UploadImge != null)
                {
                    using var binaryReader = new BinaryReader(data.UploadImge.OpenReadStream());
                    img_Data = binaryReader.ReadBytes((int)data.UploadImge.Length);
                }
                else
                {
                    img_Data = data.Image;
                }

                // Получаем текущее изображение блюда в базе
                //byte[]? client_image = _bl.Clients.Get(ID: data.ID)?.Photo;

                int client_id = _bl.Clients.Edit(ID: data.ID, Phone_New: data.Phone, Name_New: data.Name ?? "",
                                                 Date_Birth_New: date, Photo_New: img_Data, Flag_IsPolicy_New: data.Flag_IsPolicy,
                                                 Delivery_Address_New: data.Delivery_Address, Delivery_Apartment_New: data.Delivery_Apartment,
                                                 Delivery_EntranceCode_New: data.Delivery_EntranceCode, Delivery_EntranceNumber_New: data.Delivery_EntranceNumber,
                                                 Delivery_Floor_New: data.Delivery_Floor, Name_Another_New: data.Name_Another, Phone_Another_New: data.Phone_Another,
                                                 Car_Brand_New: data.Car_Brand, Car_RegNumber_New: data.Car_RegNumber, Car_Color_New: data.Car_Color);

                if (client_id > 0)
                {
                    if (data.UploadImge != null)
                    {
                        Helper_Image.Del_Client(ClientID: client_id, ContentRootPath: _env.ContentRootPath);
                        byte[] image_data = Helper_Image.Add_Client(ClientID: client_id, CurrentFile: data.UploadImge, ContentRootPath: _env.ContentRootPath);
                        _bl.Clients.Edit_Image(ID: client_id, Client_Image: image_data);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model: data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CClient? client = _bl.Clients.Get(ID: ID);

                if (client != null)
                {
                    Model_Clients_AddEdit data = new()
                    {
                        Name = client.Name
                    };

                    return View(model: data);
                }
                else
                {
                    return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Clients_AddEdit Data)
        {
            if (Data != null)
            {
                int res = _bl.Clients.Del_Account(ClientID: Data.ID);

                if (res < 0)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления аккаунта");

                    return View(model: Data);
                }

                if (res != Data.ID)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                    return View(model: Data);
                }

                res = _bl.Orders.Del_All(ClientID: Data.ID);
                if (res < 0)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления заказов аккаунта");

                    return View(model: Data);
                }

                if (res != Data.ID)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                    return View(model: Data);
                }

                res = _bl.Order_Favorites.Del_All(ClientID: Data.ID);
                if (res < 0)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления избранных заказов аккаунта");

                    return View(model: Data);
                }

                if (res != Data.ID)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                    return View(model: Data);
                }

                res = _bl.Bonuses.Del_All(ClientID: Data.ID);
                if (res < 0)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка обнуления бонусов аккаунта");

                    return View(model: Data);
                }

                if (res != Data.ID)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                    return View(model: Data);
                }

                res = _bl.ClientPay_Cards.Cards_DelAll(ClientID: Data.ID);
                if (res < 0)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления банковских карт аккаунта");

                    return View(model: Data);
                }
                if (res != Data.ID)
                {

                    _log.LogError(message: "{}", args: $"Err # {res} # Ошибка удаления банковских карт аккаунта, код не совпадает");

                    return View(model: Data);
                }

                if (client_id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Удаление

        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Client_DelImage(int ID)
        {
            if (ID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            int result = _bl.Clients.Del_Image(ID: ID);
            if (result < 0)
            {
                _log.LogError("Ошибка удаления изображения");
                return RedirectToAction(controllerName: "Clients", actionName: "Edit", routeValues: new { ID, Flag_IsErrorDelPhoto = true });
            }

            Helper_Image.Del_Client(ClientID: ID, ContentRootPath: _env.ContentRootPath);
            return RedirectToAction(controllerName: "Clients", actionName: "Edit", routeValues: new { ID });
        }

        /// <summary>
        /// Редирект на страницу с описанием ошибки
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                 controllerName: Contst_Admin.ErrorControllerName,
                                 routeValues: new { ErrCode, Url });

        }

        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Flag_ShowMenu = UserAdmin();
            base.OnActionExecuting(filterContext);
        }

        #region Получение Флага > Админ

        /// <summary>
        /// Получение Флага > Админ
        /// </summary>
        /// <returns></returns>
        public bool UserAdmin()
        {
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null && user.Flag_Root)
            {
                ViewBag.Flag_ShowMenu = true;
                return true;
            }

            ViewBag.Flag_ShowMenu = false;
            return false;
        }

        #endregion Получение Флага > Админ

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени клиента
        /// </summary>
        /// <returns></returns>
        public string? ClientName()
        {
            CClient? client = _bl.Clients.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (client != null)
            {
                return client.Name;
            }
            return "";
        }

        #endregion Получение имени клиента


        /// <summary>
        /// Экспорт в формате Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportToExcel()
        {

            List<Database.Definitions.Client> clients = _dl_Client.GetQueryAll(Flag_Del_val: false).ToList();
            ExcelPackage.License.SetNonCommercialPersonal(AppConfig.Excel_CommercialPersonal);

            using (ExcelPackage package = new())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Клиенты");

                // Заголовки
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Телефон";
                worksheet.Cells[1, 3].Value = "Имя";
                worksheet.Cells[1, 4].Value = "Дата регистрации";
                worksheet.Cells[1, 5].Value = "Последняя активность";
                worksheet.Cells[1, 6].Value = "Сумма заказов";
                worksheet.Cells[1, 7].Value = "Кол-во заказов";
                worksheet.Cells[1, 8].Value = "Дата последнего заказа";

                // Стиль заголовков
                using (ExcelRange range = worksheet.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Заполняем данные
                int row = 2;
                foreach (Database.Definitions.Client client in clients)
                {
                    decimal? ordersSum = _dl_Client.GetOrdersSum(client.ID);
                    int ordersCount = client.List_Orders?.Count ?? 0;
                    DateTime? lastOrderDate = client.List_Orders != null && client.List_Orders.Any() ? client.List_Orders.Max(o => o.Date_Act) : null;

                    worksheet.Cells[row, 1].Value = client.ID;
                    worksheet.Cells[row, 2].Value = client.Phone;
                    worksheet.Cells[row, 3].Value = client.Name;
                    worksheet.Cells[row, 4].Value = client.Date_Registration?.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[row, 5].Value = client.Date_LastAction.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[row, 6].Value = ordersSum ?? 0;
                    worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[row, 7].Value = ordersCount;
                    worksheet.Cells[row, 8].Value = lastOrderDate?.ToString("dd.MM.yyyy HH:mm");

                    row++;
                }

                worksheet.Cells.AutoFitColumns();
                MemoryStream stream = new();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Клиенты_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}