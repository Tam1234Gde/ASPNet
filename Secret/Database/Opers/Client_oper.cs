using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class Client_oper
    {

        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;
        
        
        /// <summary>
        /// Контекст баззы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Client> query = from p in _db.Clients.Include(x => x.List_Orders).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса для редактиррования
        /// </summary>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Client> query = from p in _db.Clients.Include(x => x.List_Orders) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Client? Get(int ID, bool? Flag_Del_val = false)
        {
            Client? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID списка
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public IQueryable<Client?> GetList_ByIDs(List<int> List_IDs, bool? Flag_Del_val = false)
        {
            IQueryable<Client?> elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where List_IDs.Contains(p.ID) select p);
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Client? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Client? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по телефону
        /// </summary>
        /// <param name="Phone">Номер телефона</param>
        /// <returns></returns>
        public Client? Get(string Phone, bool? Flag_Del_val = false)
        {
            Client? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Phone == Phone select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение клиентов с суммой заказов
        /// </summary>
        public IQueryable<ClientWithSum> GetQueryAllWithSum(bool? Flag_Del_val = false)
        {
            IQueryable<ClientWithSum>? query = from c in GetQuery(Flag_Del_val: Flag_Del_val) select 
                                               new ClientWithSum 
                                               {
                                                   Client = c,
                                                   OrdersSum = c.List_Orders!.Sum(o => o.Order_Datas!.Sum(d => d.Dish_Summ) + (o.Delivery_Price ?? 0))
                                               };

            return query;
        }


        /// <summary>
        /// DTO для клиента с суммой заказов
        /// </summary>
        public class ClientWithSum
        {
            public Client Client { get; set; } = null!;
            public decimal? OrdersSum { get; set; }
        }


        /// <summary>
        /// Получить сумму заказов клиента
        /// </summary>
        public decimal? GetOrdersSum(int ClientID)
        {
            decimal sum = (from o in _db.Orders join od in _db.Order_Datas on o.ID equals od.OrderID where o.ClientID == ClientID && !o.Flag_Del
                           group od by o.ID into g select g.Sum(od => od.Dish_Summ)).Sum();

            decimal deliverySum = _db.Orders.Where(o => o.ClientID == ClientID && !o.Flag_Del).Sum(o => o.Delivery_Price ?? 0);

            return sum + deliverySum;
        }


        /// <summary>
        /// Получение по телефону и GUID
        /// </summary>
        /// <param name="Phone">Номер телефона</param>
        /// <param name="SmsCode">SMS Код</param>
        /// <returns></returns>
        public Client? Get_BySms(string Phone, string SmsCode, bool? Flag_Del_val = false)
        {
            Client? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Phone == Phone && p.Code_Phone == SmsCode select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client> GetQueryAll(bool? Flag_Del_val = false)
        {
            IQueryable<Client> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) select p;


            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client> GetQueryAllActive(int ActiveDays, bool? Flag_Del_val = false)
        {
            IQueryable<Client> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Date_LastAction > (DateTime.Now.AddDays(-ActiveDays)) select p;
            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Phone">Номер телкфона</param>
        /// <param name="Name">Имя</param>
        /// <param name="Date_Birth">Дата рождения</param>
        /// <param name="Photo">Фото</param>
        /// <param name="Flag_IsPolicy">Флаг принятия политики</param>
        /// <returns></returns>
        public Client? Add(string Phone, string Name, DateTime? Date_Birth, bool Flag_IsPolicy, byte[]? Photo, string? Delivery_Address, string? Delivery_Apartment,
                           string? Delivery_EntranceCode, string? Delivery_EntranceNumber, string? Delivery_Floor)
        {
            string loginfo = $"Phone='{Phone}' Name='{Name}' Date_Birth='{Date_Birth}' Flag_IsPolicy='{Flag_IsPolicy}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client elem = new()
            {
                Phone = Phone,
                Name = Name,
                Date_Birth = Date_Birth,
                Guid = Guid.NewGuid().ToString(),
                Code_Phone = "",
                Photo = Photo,
                Delivery_Address = Delivery_Address,
                Delivery_Apartment = Delivery_Apartment,
                Delivery_EntranceCode = Delivery_EntranceCode,
                Delivery_EntranceNumber = Delivery_EntranceNumber,
                Delivery_Floor = Delivery_Floor,
                Flag_IsPolicy = Flag_IsPolicy,
                Date_LastAction = DateTime.Now,
                Date_LastUpdate = DateTime.Now,
                Date_LastZakaz = null,
                Date_Registration = DateTime.Now,
            };

            _db.Clients.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return null;
            }

        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Phone_New">Новый номер телефона</param>
        /// <param name="Name_New">Новое имя</param>
        /// <param name="Date_Birth_New">Новая дата рожденя</param>
        /// <param name="Photo_New">Новое фото</param>
        /// <param name="Flag_IsPolicy_New">Новый флаг приятия полтики</param>
        /// <returns></returns>
        public int Edit(int ID, string Phone_New, string Name_New, DateTime? Date_Birth_New, bool Flag_IsPolicy_New, byte[]? Photo_New, string? Delivery_Address_New,
                        string? Delivery_Apartment_New, string? Delivery_EntranceCode_New, string? Delivery_EntranceNumber_New, string? Delivery_Floor_New,
                        string? Name_Another_New, string? Phone_Another_New, string? Car_Brand_New, string? Car_RegNumber_New, string? Car_Color_New)
        {
            string loginfo = $"ID='{ID}' Phone_New='{Phone_New}' Name_New='{Name_New}' Date_Birth_New='{Date_Birth_New}'" +
                             $"Flag_IsPolicy_New='{Flag_IsPolicy_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Phone = Phone_New;
            elem.Photo = Photo_New;
            elem.Name = Name_New;
            elem.Delivery_Address = Delivery_Address_New;
            elem.Delivery_Apartment = Delivery_Apartment_New;
            elem.Delivery_EntranceCode = Delivery_EntranceCode_New;
            elem.Delivery_EntranceNumber = Delivery_EntranceNumber_New;
            elem.Delivery_Floor = Delivery_Floor_New;
            elem.Name_Another = Name_Another_New;
            elem.Phone_Another = Phone_Another_New;
            elem.Car_Brand = Car_Brand_New;
            elem.Car_RegNumber = Car_RegNumber_New;
            elem.Car_Color = Car_Color_New;
            elem.Date_Birth = Date_Birth_New;
            elem.Flag_IsPolicy = Flag_IsPolicy_New;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение доставки
        /// </summary>
        /// <param name="ID">Идетификатор записи</param>
        /// <param name="Delivery_Address">Адрес доставеи</param>
        /// <param name="Delivery_Apartment">Apartment</param>
        /// <param name="Delivery_EntranceCode">Код домофона</param>
        /// <param name="Delivery_EntranceNumber">EntranceNumber</param>
        /// <param name="Delivery_Floor">Floor</param>
        /// <returns></returns>
        public int Edit_Delivery(int ID, string? Delivery_Address, string? Delivery_Apartment, string? Delivery_EntranceCode,
                                 string? Delivery_EntranceNumber, string? Delivery_Floor)
        {
            string loginfo = $"ID='{ID}' Delivery_Address='{Delivery_Address}' Delivery_Apartment='{Delivery_Apartment}' Delivery_EntranceCode='{Delivery_EntranceCode}'" +
                             $"Delivery_EntranceNumber='{Delivery_EntranceNumber}' Delivery_Floor='{Delivery_Floor}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Delivery_Address = Delivery_Address;
            elem.Delivery_Apartment = Delivery_Apartment;
            elem.Delivery_EntranceCode = Delivery_EntranceCode;
            elem.Delivery_EntranceNumber = Delivery_EntranceNumber;
            elem.Delivery_Floor = Delivery_Floor;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение данных автомобиля
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Car_Brand">Марка автомобиля</param>
        /// <param name="Car_RegNumber">Регистрационный номер автомобиля</param>
        /// <param name="Car_Color">Цвет автомобиля</param>
        /// <returns></returns>
        public int Edit_Car(int ID, string? Car_Brand, string? Car_RegNumber, string? Car_Color)
        {
            string loginfo = $"ID='{ID}' Car_Brand='{Car_Brand}' Car_RegNumber='{Car_RegNumber}' Car_Color='{Car_Color}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Car_Brand = Car_Brand;
            elem.Car_RegNumber = Car_RegNumber;
            elem.Car_Color = Car_Color;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение данных другое
        /// </summary>
        /// <param name="ID">Идентфикатор записи</param>
        /// <param name="Name_Another">Имя</param>
        /// <param name="Phone_Another">Номер телефона</param>
        /// <returns></returns>
        public int Edit_Another(int ID, string? Name_Another, string? Phone_Another)
        {
            string loginfo = $"ID='{ID}' Name_Another='{Name_Another}' Phone_Another='{Phone_Another}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name_Another = Name_Another;
            elem.Phone_Another = Phone_Another;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение дня рождения
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Name_New">Новое имя</param>
        /// <param name="Date_Birth_New">Новая дата рождения</param>
        /// <returns></returns>
        public int Edit(int ID, string? Name_New, DateTime? Date_Birth_New)
        {
            string loginfo = $"ID='{ID}'Name_New='{Name_New}' Date_Birth_New='{Date_Birth_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name_New;
            elem.Date_Birth = Date_Birth_New;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение фото
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Photo">Фотография</param>
        /// <returns></returns>
        public int Edit_Photo(int ID, byte[] Photo)
        {
            string loginfo = $"ID='{ID}' Photo.Length='{Photo.Length}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Photo = Photo;
            elem.Date_PhotoUpdate = DateTime.Now;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение флага политики
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Flag_IsPolicy">Флфг политики</param>
        /// <returns></returns>
        public int Edit_Flag_IsPolicy(int ID, bool Flag_IsPolicy)
        {
            string loginfo = $"ID='{ID}' Flag_IsPolicy='{Flag_IsPolicy}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_IsPolicy = Flag_IsPolicy;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение SMS-кода
        /// </summary>
        /// <param name="ID">Идертифкатор записи</param>
        /// <param name="CodeSms_New">Новый SMS код</param>
        /// <returns></returns>
        public int Edit_Code_SMS(int ID, string CodeSms_New)
        {
            string loginfo = $"ID='{ID}' SmsCode_New='{CodeSms_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Code_Phone = CodeSms_New;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение флага удаления
        /// </summary>
        /// <param name="ID">Идентифкатор записи</param>
        /// <param name="Flag_Del_New">Удален</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            elem.Date_LastUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            elem.Date_Del = null;
            elem.UserID_Del = null;
            elem.Name = null;
            if (elem.Photo != null)
            {
                elem.Date_PhotoUpdate = DateTime.Now;
            }
            elem.Photo = null;
            elem.Date_Birth = null;
            elem.Flag_IsPolicy = false;
            elem.Date_LastUpdate = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Удалить изображение клиента
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public int Del_Image(int ID)
        {
            string loginfo = $"ID='{ID}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Photo = null;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}", exception: ex);
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение даты изменения изображения
        /// </summary>
        /// <param name="ID">Идентифкатор записи</param>
        /// <returns></returns>
        public int Edit_Image_ChangeData(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Date_PhotoUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение изображения клиента
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Client_Image">Изображение</param>
        /// <returns></returns>
        public int Edit_Image(int ID, byte[] Client_Image)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Photo = Client_Image;
            elem.Date_PhotoUpdate = DateTime.Now;
            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}", exception: ex);
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Обновление даты последней активности
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public int Edit_LastAction(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}"); //TODO: MAE - Переделать так везде 
                return Errors.Err200;
            }

            elem.Date_LastAction = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}", exception: ex);
                }
                return Errors.Err203;
            }
        }
    }
}