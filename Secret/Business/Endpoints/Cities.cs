using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Города
    /// </summary>
    public class Cities
    {
        private readonly ILogger _log;
        private readonly City_oper _dl_City = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Cities()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CCity? Get(City? Elem)
        {
            if (Elem == null) return null;

            CCity item = new()
            {
                ID = Elem.ID,
                Name = Elem.Name,
                NameDeclension = Elem.NameDeclension,
                OrderNum = Elem.OrderNum,
                Flag_Hidden = Elem.Flag_Hidden,
                HiddenTitle = Elem.HiddenTitle,
                HiddenText = Elem.HiddenText,
            };
            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CCity? Get(int ID)
        {
            return Get(Elem: _dl_City.Get(ID));
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        public CCity? GetByRestaurantID(int RestaurantID)
        {
            return Get(Elem: _dl_City.GetByRestaurantID(RestaurantID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CCity> GetList(bool? Flag_Del_val = false)
        {
            List<CCity> list_result = [];
            foreach (City elem in _dl_City.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CCity? item = Get(Elem: elem);
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
        /// <param name="Name">Название города</param>
        /// <param name="NameDeclension">Название города(склонение)</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <param name="Flag_Hidden">Сокрытый флаг</param>
        /// <param name="HiddenTitle">Сокрытый заголовок</param>
        /// <param name="HiddenText">Сокрытый текст</param>
        /// <returns></returns>
        public int Add(string Name, string? NameDeclension, int OrderNum, bool Flag_Hidden, string? HiddenTitle, string? HiddenText)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Cities_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_City.Add(Name: Name, NameDeclension: NameDeclension, OrderNum: OrderNum, Flag_Hidden: Flag_Hidden, HiddenTitle: HiddenTitle,
                                HiddenText: HiddenText);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Name">Название города</param>
        /// <param name="NameDeclension">Название города(склонение)</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <param name="Flag_Hidden">Сокрытый флаг</param>
        /// <param name="HiddenTitle">Сокрытый заголовок</param>
        /// <param name="HiddenText">Сокрытый текст</param>
        /// <returns></returns>
        public int Edit(int ID, string Name, string? NameDeclension, int OrderNum, bool Flag_Hidden, string? HiddenTitle, string? HiddenText)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Cities_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_City.Edit(ID: ID, Name: Name, NameDeclension: NameDeclension, OrderNum: OrderNum, Flag_Hidden: Flag_Hidden, HiddenTitle: HiddenTitle,
                                 HiddenText: HiddenText);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Cities_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_City.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Flag_Hidden">Скрыт</param>
        /// <returns></returns>
        public int EditFlag_Hidden(int ID, bool Flag_Hidden)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Cities_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_City.EditFlag_Hidden(ID: ID, Flag_Hidden);
        }

    }
}