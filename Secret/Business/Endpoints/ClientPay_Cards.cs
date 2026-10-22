using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Платежи по картам
    /// </summary>
    public class ClientPay_Cards
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Класс работы с БД данных карт
        /// </summary>
        private readonly ClientPay_Card_Data_oper _dl_ClientPay_Card_Data = new();


        /// <summary>
        /// Класс работы с БД данных оплат
        /// </summary>
        private readonly ClientPay_Card_Pay_oper _dl_ClientPay_Card_Pay = new();


        /// <summary>
        /// Класс работы с БД типов карт
        /// </summary>
        private readonly ClientPay_Card_Type_oper _dl_ClientPay_Card_Type = new();


        /// <summary>
        /// Класс работы с настройками
        /// </summary>
        private readonly Setups _bl_Setup = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientPay_Cards()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace(message: "Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClientPay_Card_Data? Get_CardData(ClientPay_Card_Data? Elem)
        {
            if (Elem != null)
            {
                CClientPay_Card_Data item = new()
                {
                    ID = Elem.ID,
                    CardIdent = Elem.CardIdent,
                    ClientPay_Card_TypeID = Elem.ClientPay_Card_Type?.SName,
                    ClientPay_Card_Type = Elem.ClientPay_Card_Type?.Name,
                    CardLastNum = Elem.CardLastNum
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CClientPay_Card_Data? Get_CardData(int ClientID, int ID)
        {
            ClientPay_Card_Data? elem = _dl_ClientPay_Card_Data.Get(ID);

            if (elem == null || elem.ClientID != ClientID)
            {
                _log.LogError(message: Errors.Err200_str);
                return null;
            }

            return Get_CardData(Elem: elem);
        }


        /// <summary>
        /// Получение даты последнего обновления списка вопросов. 
        /// </summary>
        /// <returns>Возвращает дату последней нотификации или null если нотификаций не было</returns>
        public DateTime? GetLastUpdateDate(int ClientID)
        {
            IQueryable<ClientPay_Card_Data> query = _dl_ClientPay_Card_Data.GetQuery(ClientID: ClientID, Flag_Del_val: null);
            DateTime? date_update_max = (from p in query orderby p.Date_Update descending select p).Max(p => (DateTime?)p.Date_Update);
            return date_update_max;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClientPay_Card_Data> GetList_CardData(int ClientID)
        {
            List<CClientPay_Card_Data> list_result = [];

            IQueryable<ClientPay_Card_Data> query = _dl_ClientPay_Card_Data.GetQuery(ClientID: ClientID);

            foreach (ClientPay_Card_Data elem in query.ToList())
            {
                CClientPay_Card_Data? item = Get_CardData(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            return list_result;
        }


        /// <summary>
        /// Добавление новой карты с инициализационным платежом
        /// </summary>
        /// <returns></returns>
        public int AddNewCard(int ClientID, string ClientPay_Card_Type, string CardIdent, string CardLastNum, string PaymentIdent, decimal Summ, bool Flag_Refund,
                              out string? YooKassaRefundID, out CClientPay_Card_Data? ClientPay_Card_Data)
        {
            string loginfo = $"ClientID='{ClientID}' CardIdent='{CardIdent}' CardLastNum='{CardLastNum}' ClientPay_Card_Type='{ClientPay_Card_Type}' " +
                             $"PaymentIdent='{PaymentIdent}' Summ='{Summ}' Flag_Refund='{Flag_Refund}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            YooKassaRefundID = null;
            ClientPay_Card_Data = null;

            EClientPay_Card_Type cardtype = _dl_ClientPay_Card_Type.Get_ByName(ClientPay_Card_Type);

            if (cardtype == EClientPay_Card_Type._NotSet)
            {

                _log.LogWarning(message: "Передан несуществующий в базе тип карты, применен тип по умолчанию # {}", args: loginfo);

                cardtype = EClientPay_Card_Type.Unknown;
            }

            if (_dl_ClientPay_Card_Data.Check_CardIdentPresent(ClientID: ClientID, CardIdent: CardIdent))
            {

                _log.LogWarning(message: "Карта уже добавлена у пользователя # {} ## {}", args: [Errors.Err900_str, loginfo]);

                return Errors.Err900;
            }

            int carddata_id = _dl_ClientPay_Card_Data.Add(ClientID: ClientID, ClientPay_Card_TypeID: cardtype, CardIdent: CardIdent, CardLastNum: CardLastNum);

            if (carddata_id < 0)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: Errors.GetErrorString(ErrorVal: carddata_id));
                }

                return carddata_id;
            }

            int paydata_id = _dl_ClientPay_Card_Pay.Add(ClientID: ClientID, OrderID: null, ClientPay_Card_Pay_TypeID: EClientPay_Card_Pay_Type.AddInitialPay,
                                                        ClientPay_Card_DataID: carddata_id, PaymentIdent: PaymentIdent, Summ: Summ);

            if (paydata_id < 0)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: Errors.GetErrorString(ErrorVal: paydata_id));
                }

                return paydata_id;
            }

            paydata_id = _dl_ClientPay_Card_Pay.Edit_FlagComplete(ID: paydata_id, FlagComplete_val: true);

            if (paydata_id < 0)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: Errors.GetErrorString(ErrorVal: paydata_id));
                }

                return paydata_id;
            }

            if (Flag_Refund) //Если задан возврат из Юкассы
            {
                bool result = Helper_YooKassa.YooKassa.Refund(Payment_id: PaymentIdent, Summ: Summ, RestaurantLogin: string.Empty,
                                                              RestaurantPassw: string.Empty, YooKassaID: out string? YooKassaID, out string? Log);

                //TODO: добавить в _dl_ClientPay_Card_Pay строчку с возвратом

                if (!result)
                {

                    if (_log.IsEnabled(LogLevel.Critical))
                    {
                        _log.LogCritical(message: "Error: {} # {} ## {}", args: [Errors.Err910, Errors.Err910_str, Log]);
                    }

                    _dl_ClientPay_Card_Data.Edit_FlagDel(ID: carddata_id, FlagDel_val: true);

                    return Errors.Err910;
                }

                YooKassaRefundID = YooKassaID;
            }

            ClientPay_Card_Data? clientPay_Card_Data = _dl_ClientPay_Card_Data.Get(carddata_id);
            ClientPay_Card_Data = Get_CardData(clientPay_Card_Data);

            return carddata_id;
        }


        /// <summary>
        /// Получить карту клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public CClientPay_Card_Data? Get_Card(int ClientID)
        {
            IQueryable<ClientPay_Card_Data> query = _dl_ClientPay_Card_Data.GetQuery(ClientID: ClientID);

            foreach (ClientPay_Card_Data elem in query.ToList())
            {
                CClientPay_Card_Data? item = Get_CardData(Elem: elem);
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }


        /// <summary>
        /// Пометка карты клиента на удаление
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int Card_Del(int CardID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Cards_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_ClientPay_Card_Data.Edit_FlagDel(ID: CardID, FlagDel_val: true);
        }


        /// <summary>
        /// Удаление всех карт клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Cards_DelAll(int ClientID)
        {
            int result = _dl_ClientPay_Card_Data.SetAll_FlagDel(ClientID: ClientID);

            if (result < 0)
            {
                _log.LogError(message: "{}", args: Errors.GetErrorString(result));
                return result;
            }

            return result;
        }

    }
}