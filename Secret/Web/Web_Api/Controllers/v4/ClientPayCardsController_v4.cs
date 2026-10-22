using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Auth;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4._Common;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.RestAndRant.Web_Api.Models.v4.ClientPayCards;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Банковские карты клиента
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("ClientPayCards_v4 - Банковские карты клиента")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/ClientPayCards/[action]")]
    [ApiController]
    [BasicAuth]
    public class ClientPayCardsController_v4(ILogger<ClientPayCardsController_v4> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Получение карты клиента
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CClientPay_Card_Data>))]
        public JsonResult GetCard()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClientPay_Card_Data? card = _bl.ClientPay_Cards.Get_Card(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: card);
        }


        /// <summary>
        /// Получение даты последнего обновления списка карт клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime?))]
        public JsonResult GetLastUpdate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            DateTime? date = _bl.ClientPay_Cards.GetLastUpdateDate(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: date);
        }


        /// <summary>
        /// Добавление карты в момент привязки, с указанием данных инициализационного платежа
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>Код платежа или код ошибки</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CClientPay_Card_Data))]
        public JsonResult AddNewCard(Model_ClientPayCards_AddNewCard_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }


            //Проверка на вторую карту
            CClientPay_Card_Data? card = _bl.ClientPay_Cards.Get_Card(ClientID: client.ID);

            if(card != null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: -200, Message: $"У клиента уже есть привязанная карта {card.ID}");
            }


            int paymentid = _bl.ClientPay_Cards.AddNewCard(ClientID: client.ID, ClientPay_Card_Type: Data.ClientPay_Card_Type, CardIdent: Data.CardIdent,
                                                           CardLastNum: Data.CardLastNum, PaymentIdent: Data.PaymentIdent, Summ: Data.Summ, Flag_Refund: true,
                                                           YooKassaRefundID: out string? _, out CClientPay_Card_Data? ClientPay_Card_Data);

            if (paymentid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: paymentid, Message: Errors.GetErrorString(paymentid));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: paymentid, Data: ClientPay_Card_Data);
        }


        /// <summary>
        /// Удаление карты 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>Код платежа или код ошибки</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Card_Del(Model_Common_Int_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int cardid = _bl.ClientPay_Cards.Card_Del(CardID: Data.Data_Int);

            if (cardid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: cardid, Message: Errors.GetErrorString(cardid));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: cardid);
        }

    }
}