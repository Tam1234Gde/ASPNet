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
    /// Цвета
    /// </summary>
    public class Colors
    {
        private readonly ILogger _log;
        private readonly Color_oper _dl_Color = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Colors()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CColor? Get(Color? Elem)
        {
            if (Elem != null)
            {
                CColor item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    Name_Eng = Elem.Name_Eng,
                    HexColor = Elem.HexColor
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
        public CColor? Get(int ID)
        {
            return Get(Elem: _dl_Color.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CColor>? GetList(bool? Flag_Del_val = false)
        {
            List<CColor> list_result = [];

            foreach (Color elem in _dl_Color.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CColor? item = Get(Elem: elem);
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
        /// <param name="Name"></param>
        /// <param name="Name_Eng"></param>
        /// <param name="HexColor"></param>
        /// <returns></returns>
        public int Add(string Name, string Name_Eng, string HexColor)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Colors_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Color.Add(Name: Name, Name_Eng: Name_Eng, HexColor: HexColor);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <param name="Name_Eng_New"></param>
        /// <param name="HexColor_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, string Name_Eng_New, string HexColor_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Colors_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Color.Edit(ID: ID, Name_New: Name_New, Name_Eng_New: Name_Eng_New, HexColor_New: HexColor_New);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Colors_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Color.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}