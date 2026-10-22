using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions
{
    /// <summary>
    /// Данные отмены
    /// </summary>
    public class CYokassa_Pay_CancellationDetails
    {
        /// <summary>
        /// Ошибка которая возникла во время платежа
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string? Reason { get; set; }

        /// <summary>
        /// <para>Сопаставляет статус отмены с текстовым описанием</para>
        /// <para>Полный список описан в https://yookassa.ru/developers/payment-acceptance/after-the-payment/declined-payments</para>
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return Reason switch
            {
                "3d_secure_failed" => "Не пройдена аутентификация по 3-D Secure.",
                "call_issuer" => "Оплата данным платежным средством отклонена по неизвестным причинам.",
                "canceled_by_merchant" => "Платеж отменен по API при оплате в две стадии",
                "card_expired" => "Истек срок действия банковской карты.",
                "country_forbidden" => "Нельзя заплатить банковской картой, выпущенной в этой стране.",
                "expired_on_capture" => "Истек срок списания оплаты у двухстадийного платежа.",
                "expired_on_confirmation" => "Истек срок оплаты: пользователь не подтвердил платеж за время, отведенное на оплату выбранным способом.",
                "fraud_suspected" => "Платеж заблокирован из-за подозрения в мошенничестве.",
                "general_decline" => "Причина не детализирована.",
                "identification_required" => "Превышены ограничения на платежи для кошелька ЮMoney. ",
                "insufficient_funds" => "Не хватает денег для оплаты.",
                "internal_timeout" => "Технические неполадки на стороне ЮKassa: не удалось обработать запрос в течение 30 секунд.",
                "invalid_card_number" => "Неправильно указан номер карты.",
                "invalid_csc" => "Неправильно указан код CVV2 (CVC2, CID). ",
                "issuer_unavailable" => "Организация, выпустившая платежное средство, недоступна. ",
                "payment_method_limit_exceeded" => "Исчерпан лимит платежей для данного платежного средства или вашего магазина. ",
                "payment_method_restricted" => "Запрещены операции данным платежным средством (например, карта заблокирована из-за утери, кошелек — из-за взлома мошенниками).",
                "permission_revoked" => "Нельзя провести безакцептное списание: пользователь отозвал разрешение на автоплатежи.",
                "unsupported_mobile_operator" => "Нельзя заплатить с номера телефона этого мобильного оператора.",
                _ => ""
            };
        }
    }
}