using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2;
using HIDE.Ithelper.Web.Business_YooKassa_Backend.Enums;
using HIDE.RestAndRant.Common;
using HIDE.Units_Core;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend
{
	/// <summary>
	/// Работа с Юкассой (новый класс, используется для создания платежей с учетом вебхуков)
	/// </summary>
	public class Yookassa_v2
	{
		/// <summary>
		/// Логгер
		/// </summary>
		private readonly ILogger _log;


		/// <summary>
		/// Логин ЮКассы
		/// </summary>
		private readonly string _login;


		/// <summary>
		/// Пароль - ключ ЮКассы
		/// </summary>
		private readonly string _password;


		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="Login"></param>
		/// <param name="Password"></param>
		public Yookassa_v2(string Login, string Password)
		{
			_log = AppLog.CreateLogger(this.GetType());
			_log.LogTrace("Constructor");

			_login = Login;
			_password = Password;
		}


		/// <summary>
		/// Проверяет платеж
		/// </summary>
		/// <param name="Object">Объект платежа</param>
		/// <returns>Ссылка для оплаты, или NULL</returns>
		public CYookassa_Payment_Result_v2? CheckPayment(object Object)
        {
			string json;
            if (Object is string jsonString)
            {
                json = jsonString;
            }
            else if (Object is JsonElement jsonElement)
            {
                json = jsonElement.GetRawText();
            }
            else if (Object is JToken jToken)
            {
                json = jToken.ToString(Formatting.None);
            }
            else
            {
                json = JsonConvert.SerializeObject(Object);
            }

            CYookassa_Payment_Result_v2? payment;

            try
            {
                payment = JsonConvert.DeserializeObject<CYookassa_Payment_Result_v2>(json);
            }
            catch (Exception ex)
            {
				if (_log.IsEnabled(logLevel: LogLevel.Error))
                {
                    _log.LogError(ex, "Failed to deserialize payment result");
                }
                return null;
            }

			if (payment != null && (payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Waiting_For_Capture) || payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Succeeded) || payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Canceled)))
			{
				return payment;
			}

			return null;
        }


        /// <summary>
        /// Проверяет способ оплаты (при привязке на нулевую сумму)
        /// </summary>
        /// <param name="Object">Объект способа оплаты</param>
        /// <returns>Ссылка для оплаты, или NULL</returns>
        public CYookassa_Payment_Method_v2? CheckPaymentMethod(object Object)
        {
			string json;
            if (Object is string jsonString)
            {
                json = jsonString;
            }
            else if (Object is JsonElement jsonElement)
            {
                json = jsonElement.GetRawText();
            }
            else if (Object is JToken jToken)
            {
                json = jToken.ToString(Formatting.None);
            }
            else
            {
                json = JsonConvert.SerializeObject(Object);
            }

            CYookassa_Payment_Method_v2? payment;

            try
            {
                payment = JsonConvert.DeserializeObject<CYookassa_Payment_Method_v2>(json);
            }
            catch (Exception ex)
            {
				if (_log.IsEnabled(logLevel: LogLevel.Error))
                {
                    _log.LogError(ex, "Failed to deserialize payment_method result");
                }
                return null;
            }

			if (payment != null && (payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Pending) || payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Active) || payment.Status == HIDE_Enums.ToEnumString(EStatusNotifications.Inactive)))
			{
				return payment;
			}

			return null;
        }


        /// <summary>
        /// Создаёт платеж
        /// </summary>
        /// <param name="Summ">Сумма платежа</param>
        /// <param name="ReturnUrl">Обратная ссылка, на неё будет будет перекидывать пользователя в случае успешной оплаты</param>
        /// <param name="Payment_Method_ID">Идентификатор сохраненного способа оплаты</param>
        /// <param name="SavePayment">Нужно ли сохранить способ оплаты для будущих покупок</param>
        /// <param name="Description">Описание платежа</param>
        /// <param name="Metadata">Метаданные, пользователю они не видны</param>
        /// <returns>Ссылка для оплаты, или NULL</returns>
        public CYookassa_Payment_Result_v2? CreatePayment(string? Email, string? Phone, decimal Summ, string ReturnUrl, string? Payment_Method_ID, bool SavePayment, string Description, Dictionary<string, object>? Metadata)
		{
			if (Metadata != null && Metadata.Count != 0)
			{
				if (Metadata.Count > 16)
                {
					if (_log.IsEnabled(logLevel: LogLevel.Error))
					{
						_log.LogError("Максимальное количество ключей - 16");
					}
					return null;
				}

				foreach ((string key, object value) in Metadata)
				{
					if (key.Length > 32)
                    {
						if (_log.IsEnabled(logLevel: LogLevel.Error))
						{
							_log.LogError("Максимальная длина для ключа - 32 символа");
						}
						return null;
					}

					string val = value.ToString() ?? "";
					if (val.Length > 512)
                    {
						if (_log.IsEnabled(logLevel: LogLevel.Error))
						{
							_log.LogError("Максимальная длина для значения - 512 символов");
						}
						return null;
					}
				}
			}

			if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Phone))
            {
                if (_log.IsEnabled(logLevel: LogLevel.Error))
                {
                    _log.LogError("Не указаны данные покупателя");
                }
                return null;
            }

			CYookassa_Payment_v2 yookassaPayment = new()
			{
				Amount = new()
				{
					Value = Summ.ToString("#.00").ReHIDE(",", "."),
					Currency = "RUB",
				},
				Confirmation = new()
                {
                    Type = "redirect",
                    ReturnUrl = ReturnUrl,
				},
                Description = Description,
                Capture = true,
                SavePaymentMethod = SavePayment,
				Metadata = Metadata
			};

			if (string.IsNullOrWhiteSpace(Payment_Method_ID))
			{
				yookassaPayment.Payment_Method_Data = new()
				{
					Type = "bank_card",
				};
			}
			else
			{
				yookassaPayment.Payment_Method_ID = Payment_Method_ID;
			}

			if (AppConfig.Yookassa_SendReceipt)
			{
				yookassaPayment.Receipt = new()
				{
					Customer = new()
					{
						Email = Email,
						Phone = Phone,
					},
					Items =
					[
						new()
						{
							Description = Description,
							Amount = new()
							{
								Value = Summ.ToString("#.00").ReHIDE(",", "."),
								Currency = "RUB",
							},
							Vat_Code = 1,
							Quantity = 1,
							Payment_Subject = "service",
							Payment_Mode = "full_payment",
						}
					],
				};
			}

			string json = JsonConvert.SerializeObject(yookassaPayment);

			HttpClient client = new()
			{
				BaseAddress = new Uri("https://api.yookassa.ru/v3/")
			};
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;
			client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

			string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

			HttpRequestMessage requestMessage = new(HttpMethod.Post, "https://api.yookassa.ru/v3/payments");
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
			requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = client.Send(requestMessage);
			string response_str = response.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<CYookassa_Payment_Result_v2>(response_str);
		}


		/// <summary>
		/// Создаёт способ оплаты (для привязки на нулевую сумму)
		/// </summary>
		/// <param name="ReturnUrl">Обратная ссылка, на неё будет будет перекидывать пользователя в случае успешной оплаты</param>
		/// <param name="Metadata">Метаданные, пользователю они не видны</param>
		/// <returns>Ссылка для оплаты, или NULL</returns>
		public CYookassa_Payment_Method_v2? CreatePaymentMethod(string ReturnUrl, Dictionary<string, object>? Metadata)
		{
			if (Metadata != null && Metadata.Count != 0)
			{
				if (Metadata.Count > 16)
                {
					if (_log.IsEnabled(logLevel: LogLevel.Error))
					{
						_log.LogError("Максимальное количество ключей - 16");
					}
					return null;
				}

				foreach ((string key, object value) in Metadata)
				{
					if (key.Length > 32)
                    {
						if (_log.IsEnabled(logLevel: LogLevel.Error))
						{
							_log.LogError("Максимальная длина для ключа - 32 символа");
						}
						return null;
					}

					string val = value.ToString() ?? "";
					if (val.Length > 512)
                    {
						if (_log.IsEnabled(logLevel: LogLevel.Error))
						{
							_log.LogError("Максимальная длина для значения - 512 символов");
						}
						return null;
					}
				}
			}

			CYookassa_Payment_Method_Data_v2 yookassaPayment = new()
            {
                Type = "bank_card",
				Confirmation = new()
                {
                    Type = "redirect",
                    ReturnUrl = ReturnUrl,
				},
				Metadata = Metadata,
			};

			string json = JsonConvert.SerializeObject(yookassaPayment);

			HttpClient client = new()
			{
				BaseAddress = new Uri("https://api.yookassa.ru/v3/")
			};
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;
			client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

			string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

			HttpRequestMessage requestMessage = new(HttpMethod.Post, "https://api.yookassa.ru/v3/payment_methods");
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
			requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = client.Send(requestMessage);
			string response_str = response.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<CYookassa_Payment_Method_v2>(response_str);
		}
	}
}