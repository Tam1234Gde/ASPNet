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
    /// Бонусы клиента
    /// </summary>
    public class Bonuses
    {
        private readonly ILogger _log;
        private readonly Bonus_oper _dl_Bonus = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Bonuses()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CBonus? Get(Bonus? Elem)
        {
            if (Elem != null)
            {
                CBonus item = new()
                {
                    ID = Elem.ID,
                    Bonuses = Elem.Bonuses,
                    Client = Elem.Client?.Phone,
                    ClientID = Elem.ClientID,
                    OrderID = Elem.OrderID,
                    Date_Action = Elem.Date_Action,
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
        public CBonus? Get(int ID)
        {
            return Get(Elem: _dl_Bonus.Get(ID));
        }


        /// <summary>
        /// Получения текущего количества бонусов клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Количество начисляемых бонусов за блюдо</returns>
        public decimal? Check_Bonuses(int ClientID)
        {
            return _dl_Bonus.GetQuery_ByClientID(ClientID: ClientID).Sum(x => x.Bonuses);
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CBonus>? GetList()
        {
            List<CBonus> list_result = [];

            foreach (Bonus elem in _dl_Bonus.GetQuery().ToList())
            {
                CBonus? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result];
        }


        /// <summary>
        /// Получение даты последнего редактирование избранных заказов
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <returns></returns>
        public DateTime? GetMaxLastEditDate(int ClientID)
        {
            DateTime? date_max = null;
            IQueryable<Bonus> query = _dl_Bonus.GetQuery_ByClientID(ClientID: ClientID);
            if (query != null && query.Any())
            {
                date_max = query.Max(x => x.Date_Action);
            }

            return date_max;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Bonuses"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public int Add(int ClientID, decimal Bonuses, int OrderID)
        {
            _bl_Setup.Set(Key: Consts.SetupBonuses_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Bonus.Add(ClientID: ClientID, Bonuses: Bonuses, OrderID: OrderID);
        }


        /// <summary>
        /// Удаление всех бонусов клиента (нужно при удалении аккаунта)
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Del_All(int ClientID)
        {
            int result = _dl_Bonus.SetAll_FlagDel(ClientID: ClientID);

            if (result < 0)
            {

                _log.LogError(message: "{}", args: Errors.GetErrorString(result));

                return result;
            }

            _bl_Setup.Set(Key: Consts.SetupBonuses_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return result;
        }

    }
}