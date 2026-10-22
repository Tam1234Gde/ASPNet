using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	/// <summary>
	/// Результат проведения рекуррентного платежа
	/// </summary>
	public class CYokassa_Pay_Result
	{
		/// <summary>
		/// Идентификатор оплаты
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string? Id { get; set; }

		/// <summary>
		/// Статус подписки, нормальный статус "succeeded". Если статус перешёл в состояние "canceled", то нужно смотреть <see cref="CancellationDetails"/>
		/// </summary>
		[JsonProperty(PropertyName = "status")]
		public string? Status { get; set; }

		[JsonProperty("code")] public string? ErrorCode { get; set; }

		/// <summary>
		/// Данные отмены
		/// </summary>
		[JsonProperty(PropertyName = "cancellation_details")]
		public CYokassa_Pay_CancellationDetails? CancellationDetails { get; set; }

		/// <summary>
		/// Было ли успешно списание
		/// </summary>
		/// <returns>true если status=='succeeded', false в остальных случаях</returns>
		public bool IsPayed()
		{
			return Status is "succeeded";
		}

		/// <summary>
		/// <para>Получает подробный статус списания</para>
		/// <para>Если списание было оплачено, то возвращает "succeeded"</para>
		/// <para>Если списание не было оплачено, то возвращает статус из данных отмены <see cref="CancellationDetails"/></para>
		/// </summary>
		/// <returns></returns>
		public string? Get_FullStatus()
		{
			if (IsPayed())
			{
				return "succeeded";
			}

			return CancellationDetails?.Reason ?? Status ?? ErrorCode;
		}

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Status)}: {Status}, {nameof(ErrorCode)}: {ErrorCode}, {nameof(CancellationDetails)}: {CancellationDetails}";
		}

#if DEBUG

		public static CYokassa_Pay_Result ResultSuccess => new()
		{
			Id = Guid.NewGuid().ToString(),
			Status = "succeeded",
			CancellationDetails = null,
			ErrorCode = null
		};

#endif
	}
}