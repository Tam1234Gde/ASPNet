using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Столы в ресторане
    /// </summary>
    public class Restaurant_Tables
    {
        private readonly ILogger _log;
        private readonly Restaurant_Table_oper _dl_Restaurant_Table = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Tables()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CRestaurant_Table? Get(Restaurant_Table? Elem)
        {
            if (Elem != null)
            {
                CRestaurant_Table item = new()
                {
                    ID = Elem.ID,
                    RestaurantID = Elem.RestaurantID,
                    Restaurant = Elem.Restaurant?.Adress,
                    Name = Elem.Name,
                    Coordinates = Elem.Coordinates,
                    Flag_IsAvailable = Elem.Flag_IsAvailable,
                    Flag_IsAvailableApp = Elem.Flag_IsAvailableApp,
                    Restaurant_Table_TypeID = Elem.Restaurant_Table_TypeID,
                    Restaurant_Table_Type = Elem.Restaurant_Table_Type?.Name,
                    Coordinates_Rotation = Elem.Coordinates_Rotation,
                    Coordinates_X = Elem.Coordinates_X,
                    Coordinates_Y = Elem.Coordinates_Y,
                    Dop_Text = Elem.Dop_Text,
                    Price = Elem.Price,
                    Restaurant_Table_StatusID = Elem.Restaurant_Table_StatusID,
                    QR_Code = Elem.QR_Code,
                    Restaurant_HallID = Elem.Restaurant_HallID,
                    Restaurant_Hall = Elem.Restaurant_Hall?.Name
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
        public CRestaurant_Table? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Table.Get(ID));
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="QR_Code"></param>
        /// <returns></returns>
        public CRestaurant_Table? Get(string QR_Code)
        {
            return Get(Elem: _dl_Restaurant_Table.Get(QR_Code));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Table>? GetListAll(bool? Flag_Del_val = false)
        {
            List<CRestaurant_Table> list_result = [];

            foreach (Restaurant_Table elem in _dl_Restaurant_Table.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Table? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_HallID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Table>? GetList(int RestaurantID, int Restaurant_HallID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Table> list_result = [];

            foreach (Restaurant_Table elem in _dl_Restaurant_Table.GetQueryByRestaurant_HallID(RestaurantID: RestaurantID, Restaurant_HallID: Restaurant_HallID,
                                                                                               Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Table? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Table>? GetList(int RestaurantID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Table> list_result = [];

            foreach (Restaurant_Table elem in _dl_Restaurant_Table.GetQuery_ByRestarantID(RestaurantID: RestaurantID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Table? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_HallID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Table>? GetListByRestaurant_Table_TypeID(int RestaurantID, int Restaurant_Table_TypeID, int? Restaurant_HallID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Table> list_result = [];

            foreach (Restaurant_Table elem in _dl_Restaurant_Table.GetQueryByRestaurant_TableTypeID(RestaurantID: RestaurantID, Restaurant_Table_TypeID: Restaurant_Table_TypeID,
                                                                                                    Restaurant_HallID: Restaurant_HallID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Table? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Name"></param>
        /// <param name="Coordinates"></param>
        /// <param name="Flag_IsAvailableApp"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Coordinates_X"></param>
        /// <param name="Coordinates_Y"></param>
        /// <param name="Coordinates_Rotation"></param>
        /// <param name="Dop_Text"></param>
        /// <param name="Price"></param>
        /// <param name="Restaurant_Table_StatusID"></param>
        /// <param name="QR_Code"></param>
        /// <returns></returns>
        public int Add(int RestaurantID, string Name, string? Coordinates, bool Flag_IsAvailableApp, bool Flag_IsAvailable, ERestaurant_Table_Type? Restaurant_Table_TypeID,
                       decimal? Coordinates_X, decimal? Coordinates_Y, decimal? Coordinates_Rotation, string? Dop_Text, decimal? Price,
                       ERestaurant_Table_Status? Restaurant_Table_StatusID, string? QR_Code)
        {
            _bl_Setup.Set(Key: Consts.SetupRestaurant_Tables_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant_Table.Add(RestaurantID: RestaurantID, Name: Name, Coordinates: Coordinates, Flag_IsAvailableApp: Flag_IsAvailableApp,
                                            Flag_IsAvailable: Flag_IsAvailable, Restaurant_Table_TypeID: Restaurant_Table_TypeID, Coordinates_X: Coordinates_X,
                                            Coordinates_Y: Coordinates_Y, Coordinates_Rotation: Coordinates_Rotation, Dop_Text: Dop_Text, Price: Price,
                                            Restaurant_Table_StatusID: Restaurant_Table_StatusID, QR_Code: QR_Code);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RestaurantID"></param>
        /// <param name="Name"></param>
        /// <param name="Coordinates"></param>
        /// <param name="Flag_IsAvailableApp"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Coordinates_X"></param>
        /// <param name="Coordinates_Y"></param>
        /// <param name="Coordinates_Rotation"></param>
        /// <param name="Dop_Text"></param>
        /// <param name="Price"></param>
        /// <param name="Restaurant_Table_StatusID"></param>
        /// <param name="QR_Code"></param>
        /// <param name="Restaurant_HallID"></param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, string Name, string? Coordinates, bool Flag_IsAvailableApp, bool Flag_IsAvailable, ERestaurant_Table_Type? Restaurant_Table_TypeID,
                        decimal? Coordinates_X, decimal? Coordinates_Y, decimal? Coordinates_Rotation, string? Dop_Text, decimal? Price,
                        ERestaurant_Table_Status? Restaurant_Table_StatusID, string? QR_Code, int? Restaurant_HallID)
        {
            _bl_Setup.Set(Key: Consts.SetupRestaurant_Tables_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant_Table.Edit(ID: ID, RestaurantID: RestaurantID, Name: Name, Coordinates: Coordinates, Flag_IsAvailableApp: Flag_IsAvailableApp,
                                             Flag_IsAvailable: Flag_IsAvailable, Restaurant_Table_TypeID: Restaurant_Table_TypeID, Coordinates_X: Coordinates_X,
                                             Coordinates_Y: Coordinates_Y, Coordinates_Rotation: Coordinates_Rotation, Dop_Text: Dop_Text, Price: Price,
                                             Restaurant_Table_StatusID: Restaurant_Table_StatusID, QR_Code: QR_Code, Restaurant_HallID: Restaurant_HallID);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupRestaurant_Tables_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant_Table.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

        /// <summary>
        /// Установить флаг Flag_IsAvailable
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <returns></returns>
        public int Edit_Flag_IsAvailable(int ID, bool Flag_IsAvailable)
        {
            _bl_Setup.Set(Key: Consts.SetupRestaurant_Tables_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant_Table.Edit_Flag_IsAvailable(ID: ID, Flag_IsAvailable: Flag_IsAvailable);
        }

    }
}