using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Данные, необходимые для инициирования выбранного сценария подтверждения платежа пользователем
    /// </summary>
    public class CYookassa_Confirmation_v2
	{
        /// <summary>
        /// Код сценария подтверждения
        /// </summary>
        [JsonProperty(PropertyName = "type")]
		public string Type { get; set; } = null!;


        /// <summary>
        /// URL, на который вернется пользователь после подтверждения или отмены платежа на веб-странице. Не более 2048 символов.
        /// </summary>
		[JsonProperty(PropertyName = "return_url")]
		public string ReturnUrl { get; set; } = null!;
	}
}