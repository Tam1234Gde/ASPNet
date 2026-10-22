using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Объёмы
    /// </summary>
    public class Volumes
    {
        private readonly ILogger _log;
        private readonly Volume_oper _dl_Volume = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Volumes()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CVolume? Get(Volume? Elem)
        {
            if (Elem != null)
            {
                CVolume item = new()
                {
                    ID = Elem.ID,
                    Value = Elem.Value,
                    Name = Elem.Name,
                };
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CVolume_ForAdmin? Get_ForAdmin(Volume? Elem)
        {
            if (Elem != null)
            {
                CVolume_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Value = Elem.Value,
                    Name = Elem.Name,
                };
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CVolume? Get(int ID)
        {
            return Get(Elem: _dl_Volume.Get(ID));
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CVolume_ForAdmin? Get_ForAdmin(int ID)
        {
            return Get_ForAdmin(Elem: _dl_Volume.Get(ID));
        }

        /// <summary>
        /// Проверка удален ли объём
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Check_Volume(int ID)
        {
            return _dl_Volume.Get(ID: ID)?.Flag_Del ?? true;
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CVolume>? GetList(bool? Flag_Del_val = false)
        {
            List<CVolume> list_result = [];

            foreach (Volume elem in _dl_Volume.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CVolume? item = Get(Elem: elem);
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
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CVolume_ForAdmin>? GetList_ForAdmin(bool? Flag_Del_val = false)
        {
            List<CVolume_ForAdmin> list_result = [];

            foreach (Volume elem in _dl_Volume.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CVolume_ForAdmin? item = Get_ForAdmin(Elem: elem);
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
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Add(decimal Value, string? Name)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Volumes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Volume.Add(Value: Value, Name: Name);
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Value_New"></param>
        /// <param name="Name_New"></param>
        /// <returns></returns>
        public int Edit(int ID, decimal Value_New, string? Name_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Volumes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Volume.Edit(ID: ID, Value_New: Value_New, Name_New: Name_New);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Volumes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Volume.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}