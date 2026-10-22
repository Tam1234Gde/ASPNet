using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
	/// <summary>
	/// Сумма в Юкасса
	/// </summary>
	public class CYookassa_Amount_v2
	{
		/// <summary>
		/// Сумма в выбранной валюте. Выражается в виде строки и пишется через точку, например 10.00. Количество знаков после точки зависит от выбранной валюты.
		/// </summary>
		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; } = null!;


		/// <summary>
		/// Трехбуквенный код валюты в формате ISO-4217. Пример: RUB.
		/// </summary>
		[JsonProperty(PropertyName = "currency")]
		public string Currency { get; set; } = null!;
	}
}
