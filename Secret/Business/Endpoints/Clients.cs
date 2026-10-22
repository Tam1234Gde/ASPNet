using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;
using static HIDE.RestAndRant.Database.Opers.Client_oper;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Клиенты
    /// </summary>
    public class Clients
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;
        private readonly Client_oper _dl_Client = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Clients()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogDebug("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClient? Get(Client? Elem)
        {
            if (Elem != null)
            {
                CClient item = new()
                {
                    ID = Elem.ID,
                    Phone = Elem.Phone,
                    Guid = Elem.Guid,
                    SmsCode = Elem.Code_Phone,
                    Name = Elem.Name,
                    Flag_IsPolicy = Elem.Flag_IsPolicy,
                    Date_Birth = Elem.Date_Birth,
                    Date_PhotoUpdate = Elem.Date_PhotoUpdate,
                    Date_LastAction = Elem.Date_LastAction,
                    Date_LastUpdate = Elem.Date_LastUpdate,
                    Flag_Del = Elem.Flag_Del,
                    Date_Del = Elem.Date_Del,
                    Photo = Elem.Photo,
                    UserID_Del = Elem.UserID_Del,
                    Date_Registration = Elem.Date_Registration,
                    Delivery_Address = Elem.Delivery_Address,
                    Delivery_Apartment = Elem.Delivery_Apartment,
                    Delivery_EntranceCode = Elem.Delivery_EntranceCode,
                    Delivery_EntranceNumber = Elem.Delivery_EntranceNumber,
                    Delivery_Floor = Elem.Delivery_Floor,
                    Car_Brand = Elem.Car_Brand,
                    Car_Color = Elem.Car_Color,
                    Car_RegNumber = Elem.Car_RegNumber,
                    Name_Another = Elem.Name_Another,
                    Phone_Another = Elem.Phone_Another,
                    Date_LastZakaz = Elem.OrdersLastDate,
                    City = Elem.OrdersCity,
                    OrdersSum = Elem.OrdersSum
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CClient? Get(int ID)
        {
            return Get(Elem: _dl_Client.Get(ID));
        }


        /// <summary>
        /// Вход через API для приложений
        /// </summary>
        /// <param name="ID">Код пользователя</param>
        /// <param name="Guid">Идентификатор пользователя</param>
        /// <param name="Ip">Ip-Адрес</param>
        /// <returns></returns>
        public CCommon? Login_Api(string ID, string Guid, string? Ip)
        {
            string loginfo = $"ID='{ID}' Guid='{Guid}' Ip='{Ip}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            int? id = HIDE_Convert.ToInt_Nullable(ID);

            if (id.HasValue)
            {
                Client? client = _dl_Client.Get(ID: id.Value);

                if (client != null && client.Guid == Guid)
                {
                    CCommon result = new()
                    {
                        ID = client.ID,
                        Name = client.Guid,
                    };

                    return result;
                }
                else
                {
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "{}", args: $"client='{client}' " + loginfo);
                    }
                    return null;
                }
            }

            return null;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClient> GetList(bool? Flag_Del_val = false)
        {
            List<CClient> list_result = [];

            foreach (Client elem in _dl_Client.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CClient? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список существующих клиентов по списку их ID
        /// </summary>
        /// <returns></returns>
        public List<CClient> GetList_ByIDs(List<int> List_IDs)
        {
            List<CClient> list_result = [];

            foreach (Client? elem in _dl_Client.GetList_ByIDs(List_IDs: List_IDs).ToList())
            {
                if(elem == null)
                {
                    continue;
                }

                CClient? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClient> GetList_ForAdmin()
        {
            List<CClient> list_result = [];

            List<Client> list_client = [.. _dl_Client.GetQueryAll()];
            List<ClientWithSum> list_client_sum = [.. _dl_Client.GetQueryAllWithSum()];

            // Создаем словарь для быстрого поиска суммы по ID клиента
            Dictionary<int, decimal?> sumDict = list_client_sum.ToDictionary(cs => cs.Client.ID, cs => cs.OrdersSum);

            foreach (Client elem in list_client)
            {
                CClient? item = Get(Elem: elem);
                if (item != null)
                {
                    // Добавляем сумму заказов
                    item.OrdersSum = sumDict.ContainsKey(elem.ID) ? sumDict[elem.ID] : 0;

                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список активных за последние ActiveDays дней пользователей
        /// </summary>
        /// <returns></returns>
        public List<CClient> GetList_Active(int ActiveDays)
        {
            List<CClient> list_result = [];

            foreach (Client elem in _dl_Client.GetQueryAllActive(ActiveDays: ActiveDays).ToList())
            {
                CClient? item = Get(Elem: elem);
                if (item != null)
                {

                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Получение клиента по его телефону.
        /// Если клиент по телефону найден не был, то клиент добавляется в БД
        /// </summary>
        /// <returns></returns>
        public CClient? GetOrAdd(string Phone, bool Flag_SendSms)
        {

            string loginfo = $"Phone='{Phone}' Flag_SendSms='{Flag_SendSms}'";

            CClient? client_result;

            Client? client = _dl_Client.Get(Phone: Phone);
            if (client != null)
            {
                //                if (client.Flag_Del)
                //                {
                //                    //Если клиент был удален, то восстанавливаем его
                //                    int client_restore_id = _dl_Client.Edit_FlagDel(ID: client.ID, Flag_Del_New: false);

                //                    if (client_restore_id < 0)
                //                    {
                //
                //                        _log.LogCritical(message: Errors.GetErrorString(ErrorVal: client_restore_id));
                //
                //                        return null;
                //                    }

                //                    if (client_restore_id != client.ID)
                //                    {
                //                        _log.LogCritical(message: Errors.Err200_str);
                //                        return null;
                //                    }

                //                    client_result = Get(Elem: client);

                //                    if (client_result == null)
                //                    {
                //                        _log.LogCritical(message: Errors.Err200_str);
                //                        return null;
                //                    }
                //                }

                client_result = Get(Elem: client);
            }
            else
            {
                client = _dl_Client.Add(Phone: Phone, Name: client?.Name ?? "", Date_Birth: client?.Date_Birth, Photo: null, Flag_IsPolicy: false, Delivery_Address: null,
                                        Delivery_Apartment: null, Delivery_EntranceCode: null, Delivery_EntranceNumber: null, Delivery_Floor: null);
                if (client != null)
                {
                    client_result = Get(Elem: client);
                }
                else
                {
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "{}", args: $"client='{client}' " + loginfo);
                    }
                    client_result = null;
                }
            }

            if (client_result == null)
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: $"client_result='{client_result}' " + loginfo);
                }
                return null;
            }

            if (Flag_SendSms)
            {
                string code = Helper_CodeGenerator.NumGenerate_Sms(ClientID: client_result.ID); //Получение кода подтверждения

                if (_dl_Client.Edit_Code_SMS(ID: client_result.ID, CodeSms_New: code) != client_result.ID) //Если ошибка при установке кода, то возвращаем null
                {
                    _log.LogError("Ошибка установки СМС-кода");
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "{}", args: $"Flag_SendSms='{Flag_SendSms}' " + loginfo);
                    }
                    return null;
                }

                Helper_SmsSender sms_sender = new();

                bool result_sms = sms_sender.SendSms(ClientID: client_result.ID, Phone: Phone, Text: AppConfig.SMS_Shablon.ReHIDE(oldValue: Consts_Sms.Sms_ReHIDEKey, newValue: code));
                if (result_sms)
                {

                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "{}", args: $"Отправлено SMS для входа через smsint для '{Phone}'");
                    }

                }
                else
                {

                    _log.LogError(message: "{}", args: $"Не удалось отправить СМС для '{Phone}'");

                }

            }

            return client_result;
        }


        /// <summary>
        /// Получить клиента по телефону и СМС коду
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="SmsCode"></param>
        /// <returns></returns>
        public CClient? GetClient_ByPhone_AndSms(string Phone, string SmsCode)
        {
            return Get(Elem: _dl_Client.Get_BySms(Phone: Phone, SmsCode: SmsCode));
        }


        /// <summary>
        /// Получить клиента по телефону
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public CClient? GetClient_ByPhone(string Phone)
        {
            return Get(Elem: _dl_Client.Get(Phone: Phone));
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="Name"></param>
        /// <param name="Date_Birth"></param>
        /// <param name="Photo"></param>
        /// <param name="Flag_IsPolicy"></param>
        /// <param name="Delivery_Address"></param>
        /// <param name="Delivery_Apartment"></param>
        /// <param name="Delivery_EntranceCode"></param>
        /// <param name="Delivery_EntranceNumber"></param>
        /// <param name="Delivery_Floor"></param>
        /// <returns></returns>
        public CClient? Add(string Phone, string Name, DateTime? Date_Birth, bool Flag_IsPolicy, byte[]? Photo, string? Delivery_Address, string? Delivery_Apartment,
                            string? Delivery_EntranceCode, string? Delivery_EntranceNumber, string? Delivery_Floor)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return Get(_dl_Client.Add(Phone: Phone, Name: Name, Date_Birth: Date_Birth, Photo: Photo, Flag_IsPolicy: Flag_IsPolicy, Delivery_Address: Delivery_Address,
                                      Delivery_Apartment: Delivery_Apartment, Delivery_EntranceCode: Delivery_EntranceCode, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                      Delivery_Floor: Delivery_Floor));
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Phone_New"></param>
        /// <param name="Name_New"></param>
        /// <param name="Date_Birth_New"></param>
        /// <param name="Flag_IsPolicy_New"></param>
        /// <param name="Photo_New"></param>
        /// <param name="Delivery_Address_New"></param>
        /// <param name="Delivery_Apartment_New"></param>
        /// <param name="Delivery_EntranceCode_New"></param>
        /// <param name="Delivery_EntranceNumber_New"></param>
        /// <param name="Delivery_Floor_New"></param>
        /// <param name="Name_Another_New"></param>
        /// <param name="Phone_Another_New"></param>
        /// <param name="Car_Brand_New"></param>
        /// <param name="Car_RegNumber_New"></param>
        /// <param name="Car_Color_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Phone_New, string Name_New, DateTime? Date_Birth_New, bool Flag_IsPolicy_New, byte[]? Photo_New, string? Delivery_Address_New,
                        string? Delivery_Apartment_New, string? Delivery_EntranceCode_New, string? Delivery_EntranceNumber_New, string? Delivery_Floor_New,
                        string? Name_Another_New, string? Phone_Another_New, string? Car_Brand_New, string? Car_RegNumber_New, string? Car_Color_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Client.Edit(ID: ID, Phone_New: Phone_New, Name_New: Name_New, Date_Birth_New: Date_Birth_New, Photo_New: Photo_New,
                                   Flag_IsPolicy_New: Flag_IsPolicy_New, Delivery_Address_New: Delivery_Address_New, Delivery_Apartment_New: Delivery_Apartment_New,
                                   Delivery_EntranceCode_New: Delivery_EntranceCode_New, Delivery_EntranceNumber_New: Delivery_EntranceNumber_New,
                                   Delivery_Floor_New: Delivery_Floor_New, Name_Another_New: Name_Another_New, Phone_Another_New: Phone_Another_New, Car_Brand_New: Car_Brand_New,
                                   Car_RegNumber_New: Car_RegNumber_New, Car_Color_New: Car_Color_New);
        }


        /// <summary>
        /// Изменение имени и даты рождения
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <param name="Date_Birth_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string? Name_New, DateTime? Date_Birth_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Client.Edit(ID: ID, Name_New: Name_New, Date_Birth_New: Date_Birth_New);
        }


        /// <summary>
        /// Изменение данных доставки
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Delivery_Address"></param>
        /// <param name="Delivery_Apartment"></param>
        /// <param name="Delivery_EntranceCode"></param>
        /// <param name="Delivery_EntranceNumber"></param>
        /// <param name="Delivery_Floor"></param>
        /// <returns></returns>
        public int Edit_Delivery(int ID, string? Delivery_Address, string? Delivery_Apartment, string? Delivery_EntranceCode, string? Delivery_EntranceNumber,
                                 string? Delivery_Floor)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Client.Edit_Delivery(ID: ID, Delivery_Address: Delivery_Address, Delivery_Apartment: Delivery_Apartment, Delivery_EntranceCode: Delivery_EntranceCode,
                                            Delivery_EntranceNumber: Delivery_EntranceNumber, Delivery_Floor: Delivery_Floor);
        }


        /// <summary>
        /// Изменение данных машины
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Car_Brand"></param>
        /// <param name="Car_RegNumber"></param>
        /// <param name="Car_Color"></param>
        /// <returns></returns>
        public int Edit_Car(int ID, string? Car_Brand, string? Car_RegNumber, string? Car_Color)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Client.Edit_Car(ID: ID, Car_Brand: Car_Brand, Car_RegNumber: Car_RegNumber, Car_Color: Car_Color);
        }


        /// <summary>
        /// Изменение данных другого человека
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_Another"></param>
        /// <param name="Phone_Another"></param>
        /// <returns></returns>
        public int Edit_Another(int ID, string? Name_Another, string? Phone_Another)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Client.Edit_Another(ID: ID, Name_Another: Name_Another, Phone_Another: Phone_Another);
        }


        /// <summary>
        /// Редактирование SMS кода
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="SmsCode_New"></param>
        /// <returns></returns>
        public bool Edit_AndSendSmsCode(int ClientID, string SmsCode_New)
        {
            string loginfo = $"ClientID='{ClientID}' SmsCode_New='{SmsCode_New}'";

            int clientid = _dl_Client.Edit_Code_SMS(ID: ClientID, CodeSms_New: SmsCode_New);
            if (clientid == ClientID)
            {
                //TODO: KMI - Сделать отправку SMS
                return true;
            }
            else
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: loginfo);
                }
                return false;
            }
        }


        /// <summary>
        /// Получение фотографии пользователя по его ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? Photo_Get(int ID)
        {
            return _dl_Client.Get(ID: ID)?.Photo;
        }


        /// <summary>
        /// Получение фотографии пользователя по его ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Photo"></param>
        /// <returns></returns>
        public int Photo_Set(int ID, byte[] Photo)
        {
            return _dl_Client.Edit_Photo(ID: ID, Photo: Photo);
        }


        /// <summary>
        /// Установка флага политики
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_IsPolicy"></param>
        /// <returns></returns>
        public int Flag_IsPolicy_Set(int ID, bool Flag_IsPolicy)
        {
            return _dl_Client.Edit_Flag_IsPolicy(ID: ID, Flag_IsPolicy: Flag_IsPolicy);
        }


        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Del_Image(int ID)
        {
            return _dl_Client.Del_Image(ID: ID);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Del(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Clients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Client.Edit_FlagDel(ID: ID, Flag_Del_New: true);
        }


        /// <summary>
        /// Изменение даты изменения изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Image_ChangeData(int ID)
        {
            return _dl_Client.Edit_Image_ChangeData(ID: ID);
        }


        /// <summary>
        /// Удаление аккаунта
        /// </summary>
        /// <returns></returns>
        public int Del_Account(int ClientID)
        {
            int result = _dl_Client.Edit_FlagDel(ID: ClientID, Flag_Del_New: true);
            if (result < 0)
            {

                _log.LogError(message: "{}", args: Errors.GetErrorString(result));

                return result;
            }
            return result;

        }


        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Client_Image"></param>
        /// <returns></returns>
        public int Edit_Image(int ID, byte[] Client_Image)
        {
            return _dl_Client.Edit_Image(ID: ID, Client_Image: Client_Image);
        }


        /// <summary>
        /// Обновить дату последней активности
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_LastAction(int ID)
        {
            return _dl_Client.Edit_LastAction(ID: ID);
        }
    }
}