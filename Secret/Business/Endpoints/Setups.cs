using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Параметры
    /// </summary>
    public class Setups
    {
        private readonly ILogger _log;
        private readonly Setup_oper _dl_Setup = new();
        private readonly DBContext _db = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Setups()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CSetup? Get(Setup? Elem)
        {
            if (Elem != null)
            {
                CSetup item = new()
                {
                    Key = Elem.Key,
                    Val = Elem.Val,
                    Val_Comment = Elem.Val_Comment,
                    Val2 = Elem.Val2,
                    Val2_Comment = Elem.Val2_Comment,
                    Val3 = Elem.Val3,
                    Val3_Comment = Elem.Val3_Comment,
                    Val4 = Elem.Val4,
                    Val4_Comment = Elem.Val4_Comment,
                };
                return item;
            }

            return null;
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Key">Ключ</param>
        /// <returns></returns>
        public CSetup? Get(string Key)
        {
            return Get(Elem: _dl_Setup.Get(Key));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CSetup> GetList()
        {
            List<CSetup> list_result = [];

            foreach (Setup elem in _dl_Setup.GetQueryAll().ToList())
            {
                CSetup? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public int? Get_Int(string Key)
        {
            int? result = HIDE_Convert.ToInt_Nullable(_dl_Setup.Get(Key: Key)?.Val);
            return result;
        }

        /// <summary>
        /// Получение даты
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public DateTime? Get_Date(string Key)
        {
            DateTime? result = HIDE_Convert.ToDateTime_Nullable(_dl_Setup.Get(Key: Key)?.Val);
            return result;
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public decimal? Get_Decimal(string Key)
        {
            decimal? result = HIDE_Convert.ToDecimal_Nullable(_dl_Setup.Get(Key: Key)?.Val);
            return result;
        }

        /// <summary>
        /// Получение
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string? Get_String(string Key)
        {
            string? result = _dl_Setup.Get(Key: Key)?.Val;
            return result;
        }

        /// <summary>
        /// Установить
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Val_New"></param>
        /// <returns></returns>
        public bool Set(string Key, string Val_New)
        {
            return _dl_Setup.Edit(Key: Key, Val_New: Val_New);
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="Key">Ключ</param>
        /// <param name="Val">Значение 1</param>
        /// <param name="Val_Comment">Значение 1 - Комментарий</param>
        /// <param name="Val2">Значение 2</param>
        /// <param name="Val2_Comment">Значение 2 - Комментарий</param>
        /// <param name="Val3">Значение 3</param>
        /// <param name="Val3_Comment">Значение 3 - Комментарий</param>
        /// <param name="Val4">Значение 4</param>
        /// <param name="Val4_Comment">Значение 4 - Комментарий</param>
        /// <returns></returns>
        public bool Edit(string Key, string? Val, string? Val_Comment, string? Val2, string? Val2_Comment, string? Val3, string? Val3_Comment, string? Val4,
                         string? Val4_Comment)
        {
            return _dl_Setup.Edit_ForAdmin(Key: Key, Val: Val, Val_Comment: Val_Comment, Val2: Val2, Val2_Comment: Val2_Comment, Val3: Val3, Val3_Comment: Val3_Comment,
                                           Val4: Val4, Val4_Comment: Val4_Comment);
        }

    }
}