using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Типы оплаты
    /// </summary>
    public class Pay_Types
    {
        private readonly ILogger _log;
        private readonly Pay_Type_oper _dl_Pay_Type = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Pay_Types()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon? Get(Pay_Type? Elem)
        {
            if (Elem != null)
            {
                CCommon item = new()
                {
                    ID = (int)Elem.ID,
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
        public CCommon? Get(int ID)
        {
            return Get(Elem: _dl_Pay_Type.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CCommon>? GetList()
        {
            List<CCommon> list_result = [];

            foreach (Pay_Type elem in _dl_Pay_Type.GetQuery().ToList())
            {
                CCommon? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

    }
}