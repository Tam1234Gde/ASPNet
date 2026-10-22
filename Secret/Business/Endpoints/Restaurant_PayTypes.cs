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
    /// Доступные типы оплаты в ресторане
    /// </summary>
    public class Restaurant_PayTypes
    {
        private readonly ILogger _log;
        private readonly Restaurant_PayType_oper _dl_Restaurant_PayType = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_PayTypes()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CRestaurant_PayType? Get(Restaurant_PayType? Elem)
        {
            if (Elem == null) return null;

            CRestaurant_PayType item = new()
            {
                ID = Elem.ID,
                RestaurantID = Elem.RestaurantID,
                Restaurant = Elem.Restaurant?.Adress,
                Pay_TypeID = Elem.Pay_TypeID,
                Pay_Type = Elem.Pay_Type?.Name,
                Order_TypeID = Elem.Order_TypeID,
                Order_Type = Elem.Order_Type?.Name,
            };
            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CRestaurant_PayType? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_PayType.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Order_TypeID">Тип заказа(С собой\Поесть в кафе\Доставка\К машине)</param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_PayType> GetList(int? RestaurantID, EOrder_Type? Order_TypeID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_PayType> list_result = [];
            foreach (Restaurant_PayType elem in _dl_Restaurant_PayType.GetQuery(RestaurantID: RestaurantID, Order_TypeID: Order_TypeID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_PayType? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            if (list_result.Count == 0) //TODO: MAE - понять что это было
            {
                list_result.Add(new CRestaurant_PayType
                {
                    Pay_TypeID = EPayType.CardInApp
                });
            }

            return list_result;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_PayType> GetList_ForAdmin(bool? Flag_Del_val = false)
        {
            List<CRestaurant_PayType> list_result = [];
            foreach (Restaurant_PayType elem in _dl_Restaurant_PayType.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_PayType? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Order_TypeID">Тип заказа(С собой\Поесть в кафе\Доставка\К машине)</param>
        /// <param name="Pay_TypeID">Вид оплаты (картой, нал, ...)</param>
        /// <returns></returns>
        public int Add(int RestaurantID, EOrder_Type Order_TypeID, EPayType Pay_TypeID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurant_PayTypes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_PayType.Add(RestaurantID: RestaurantID, Order_TypeID: Order_TypeID, Pay_TypeID: Pay_TypeID);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Order_TypeID">Тип заказа(С собой\Поесть в кафе\Доставка\К машине)</param>
        /// <param name="Pay_TypeID">Вид оплаты (картой, нал, ...)</param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, EOrder_Type Order_TypeID, EPayType Pay_TypeID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurant_PayTypes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_PayType.Edit(ID: ID, RestaurantID: RestaurantID, Order_TypeID: Order_TypeID, Pay_TypeID: Pay_TypeID);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurant_PayTypes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_PayType.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        public int AddAll(int RestaurantID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurant_PayTypes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            List<CCommon>? listPayTypes = new Pay_Types().GetList();
            List<CCommon>? listOrderTypes = new Order_Types().GetList();

            int result = 0;
            if (listPayTypes != null && listPayTypes.Count != 0 && listOrderTypes != null && listOrderTypes.Count != 0)
            {
                foreach (CCommon payType in listPayTypes)
                {
                    foreach (CCommon orderType in listOrderTypes)
                    {
                        result = _dl_Restaurant_PayType.Add(RestaurantID: RestaurantID, Order_TypeID: (EOrder_Type)orderType.ID, Pay_TypeID: (EPayType)payType.ID);
                    }
                }
            }

            return result;
        }

    }
}