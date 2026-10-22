using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;
using HIDE.RestAndRant.Common;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend
{
	/// <summary>
	/// Работа с Юкассой (старый класс, используется для возвратов и реккуррентных платежей)
	/// </summary>
	public class Yookassa
	{
#if !DEBUG
        public const string OFD_URL = "https://consumer.1-ofd.ru/ticket";
#endif
		private readonly ILogger _log;

		private readonly string _login;
		private readonly string _password;


		public Yookassa(string Login, string Password)
		{
			_log = AppLog.CreateLogger(this.GetType());
			_log.LogTrace("Constructor");

			_login = Login;
			_password = Password;
		}


		/// <summary>
		/// Возврат денежных средств
		/// </summary>
		/// <param name="Payment_id">Идентификатор платежа</param>
		/// <param name="Summ">Сумма возврата</param>
		/// <param name="YooKassaID">Идентификатор платежа по финалу возврата</param>
		/// <param name="Log">Логи ошибки</param>
		/// <returns>true - если платеж возвращён, false - в противном случае</returns>
		public bool Refund(string Payment_id, decimal Summ, out string? YooKassaID, out string? Log)
		{
			string loginfo = $"Payment_id='{Payment_id}' Summ='{Summ}' ";

#pragma warning disable CA2254 // Template should be a static expression
			_log.LogDebug(message: loginfo);
#pragma warning restore CA2254 // Template should be a static expression

			CYokassa_Refund refund = new()
			{
				Amount = new()
				{
					Value = Summ.ToString("#.00").ReHIDE(",", "."),
					Currency = "RUB"
				},
				Payment_id = Payment_id
			};

			string json = JsonConvert.SerializeObject(refund);

#pragma warning disable CA2254 // Template should be a static expression
			_log.LogDebug(message: $"JSON: {json} ## {loginfo}");
#pragma warning restore CA2254 // Template should be a static expression

			HttpClient client = new()
			{
				BaseAddress = new Uri("https://api.yookassa.ru/v3/")
			};
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;
			client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

			string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

			HttpRequestMessage requestMessage = new(HttpMethod.Post, "https://api.yookassa.ru/v3/refunds");
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
			requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

			//make the request
			HttpResponseMessage response = client.Send(requestMessage);
			string response_str = response.Content.ReadAsStringAsync().Result;

			StringBuilder sb = new();
			sb.Append($"StatusCode='{response.StatusCode}'");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				// Ответ от api Yookassa успешный
				try
				{
					JsonSerializerOptions options = new()
					{
						PropertyNameCaseInsensitive = true
					};

					CYokassa_Refund_Out? cYokassa_Refund_Out = JsonSerializer.Deserialize<CYokassa_Refund_Out>(response_str, options);

					if (cYokassa_Refund_Out != null)
					{
						Log = null;
#pragma warning disable CA2254 // Template should be a static expression
						_log.LogInformation(message: $"Возврат проведен успешно, данные возврата: {response_str} ");
#pragma warning restore CA2254 // Template should be a static expression
						YooKassaID = cYokassa_Refund_Out.ID;
						return true;
					}
					else
					{
#pragma warning disable CA2254 // Template should be a static expression
						_log.LogError(message: $"Не удалось получить данные о возврате с API Yookassa # {response_str} ## {loginfo}");
#pragma warning restore CA2254 // Template should be a static expression
						Log = sb.ToString();
						YooKassaID = null;
						return false;
					}
				}
				catch (Exception ex)
				{
					sb.Append($"Exception: {ex.InnerException}");
					Log = sb.ToString();
					YooKassaID = null;
					return false;
				}
			}
			else
			{
				//Неуспешный ответ от api Yookassa 

#pragma warning disable CA2254 // Template should be a static expression
				_log.LogWarning(message: $"Возврат не произведен, данные возврата: {response_str} ## {loginfo}");
#pragma warning restore CA2254 // Template should be a static expression

				sb.Append(response_str);
				Log = sb.ToString();
				YooKassaID = null;
				return false;
			}
		}

		/// <summary>
		/// Возврат денежных средств (не используется сейчас нигде)
		/// </summary>
		/// <param name="Payment_Id">Идентификатор платежа</param>
		/// <param name="Summ">Сумма возврата</param>
		/// <returns></returns>
		public CYokassa_Refund_Result Refund(string Payment_Id, decimal Summ)
		{
			string loginfo = $"Payment_id='{Payment_Id}' Summ='{Summ}' ";

#pragma warning disable CA2254 // Template should be a static expression
			_log.LogDebug(message: loginfo);
#pragma warning restore CA2254 // Template should be a static expression

			CYokassa_Refund refund = new()
			{
				Amount = new CYokassa_Amount
				{
					Value = Summ.ToString("#.00").ReHIDE(",", "."),
					Currency = "RUB"
				},
				Payment_id = Payment_Id
			};

			string json = JsonConvert.SerializeObject(refund);

#pragma warning disable CA2254 // Template should be a static expression
			_log.LogDebug(message: $"JSON: {json} ## {loginfo}");
#pragma warning restore CA2254 // Template should be a static expression

			HttpClient client = new()
			{
				BaseAddress = new Uri("https://api.yookassa.ru/v3/")
			};
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;
			client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

			string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

			HttpRequestMessage requestMessage = new(HttpMethod.Post, "https://api.yookassa.ru/v3/refunds");
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
			requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

			//make the request
			HttpResponseMessage response = client.Send(requestMessage);
			string response_str = response.Content.ReadAsStringAsync().Result;

			CYokassa_Refund_Result? res = JsonSerializer.Deserialize<CYokassa_Refund_Result>(response_str);
			return res ?? new CYokassa_Refund_Result();
		}


		/// <summary>
		/// Рекурентный платеж
		/// </summary>
		/// <param name="Card_Id">Идентификатор карты</param>
		/// <param name="Summ">Сумма платежа</param>
		/// <param name="Description">Описание платежа</param>
		/// <param name="initialSleep">Задержка при запуске платежа, ЮКасса может выкинуть 500 или too_many_requests если часто вызывать</param>
		/// <param name="email">Email для генерации чека</param>
		/// <param name="phone">Номер телефона для генерации чека</param>
		/// <returns></returns>
		public CYokassa_Pay_Result? Recurent(string Card_Id, decimal Summ, string Description, string email, string phone, int initialSleep = 5000)
		{
			/*#if DEBUG
                        if (string.IsNullOrEmpty(Card_Id) || Summ < 0)
                        {
                            return null;
                        }

                        if (string.IsNullOrWhiteSpace(Description) && (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(phone)))
                        {
                            return null;
                        }

                        if (initialSleep > 0 && initialSleep < 10_000)
                        {
                            Thread.Sleep(initialSleep);
                        }

                        return new CYokassa_Pay_Result
                        {
                            Status = "succeeded"
                        };
            #endif*/
			if (initialSleep > 0)
			{
				Thread.Sleep(initialSleep);
			}

			CYokassa_Amount amount = new()
			{
				Value = Summ.ToString("#.00").ReHIDE(",", "."),
				Currency = "RUB"
			};

			CYokassa_Recurent refund = new()
			{
				Amount = amount,
				Capture = true,
				CardIdent = Card_Id,
				Description = Description
			};

			if (_password.Contains("live"))
			{
				List<CYokassa_ReceiptItemInfo> receiptItems = [new CYokassa_ReceiptItemInfo("Оплата подписки", amount)];

				refund.Receipt = new CYokassa_Receipt
				{
					Customer = new CYokassa_ReceiptCustomer(email, phone),
					Items = receiptItems
				};
			}

			string json = JsonConvert.SerializeObject(refund, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});

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

			//make the request
			HttpResponseMessage response = client.Send(requestMessage);
			string response_str = response.Content.ReadAsStringAsync().Result;
#pragma warning disable CA2254
			_log.LogInformation(response_str);
#pragma warning restore CA2254

			CYokassa_Pay_Result? result = JsonConvert.DeserializeObject<CYokassa_Pay_Result>(response_str);
			return result;
		}

        /// <summary>
        /// Создаёт ссылку для оплаты (не используется сейчас нигде)
        /// </summary>
        /// <param name="Summ">Сумма платежа</param>
        /// <param name="ReturnUrl">Обратная ссылка, на неё будет будет перекидывать пользователья в случае успешной оплаты</param>
        /// <param name="SavePayment">Нужно ли сохранить способ оплаты для будущих покупок</param>
        /// <param name="Descrition">Описание платежа</param>
        /// <param name="Metadata">Метаданные, пользователю они не видны</param>
        /// <returns>Ссылка для оплаты, или NULL</returns>
        public string? CreatePayUrl(decimal Summ, string ReturnUrl, bool SavePayment, string Descrition, Dictionary<string, object>? Metadata)
		{
			if (Metadata != null && Metadata.Count != 0)
			{
				if (Metadata.Count > 16)
				{
					_log.LogError("Максимальное количество ключей - 16");
					return null;
				}

				foreach ((string key, object value) in Metadata)
				{
					if (key.Length > 32)
					{
						_log.LogError("Максимальная длина для ключа - 32 символа");
						return null;
					}

					string val = value.ToString() ?? "";
					if (val.Length > 512)
					{
						_log.LogError("Максимальная длина для значения - 512 символов");
						return null;
					}
				}
			}

			CYokassa_Buy yokassaBuy = new()
			{
				Amount = new CYokassa_Amount
				{
					Value = Summ.ToString("#.00").ReHIDE(",", "."),
					Currency = "RUB"
				},
				Capture = true,
				Description = Descrition,
				Confirmation = new CYokassa_Confirmation
				{
					ReturnUrl = ReturnUrl,
					Type = "redirect"
				},
				SavePaymentMethod = SavePayment,
				Metadata = Metadata
			};

			string json = JsonConvert.SerializeObject(yokassaBuy);

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

			CYokassa_BuyResult? result = JsonConvert.DeserializeObject<CYokassa_BuyResult>(response_str);

			return result?.Confirmation?.ConfirmationUrl;
		}

        /// <summary>
        /// Регистрирует новую карту в ЮКассе, использовать только в DEBUG режиме и только для тестов (не используется сейчас нигде)
        /// </summary>
        /// <param name="Summ">Сумма которую нужно снять с карты</param>
        /// <param name="CardNumber">Номер карты</param>
        /// <param name="CVV">Код безопасности</param>
        /// <param name="MonthExp">Месяц в котором истекает карта</param>
        /// <param name="YearExp">Год в котором истекает карта</param>
        /// <param name="ReturnUrl">Ссылка куда нужно перекинуть пользователя после оплаты</param>
        /// <returns>Идентификатор карты или NULL</returns>
        public string? RegisterCard(decimal Summ, string CardNumber, string CVV, string MonthExp, string YearExp, string ReturnUrl)
		{
#if DEBUG
			CYokassa_AddCardBody body = new()
			{
				Amount = new CYokassa_Amount
				{
					Value = Summ.ToString("#.00").ReHIDE(",", "."),
					Currency = "RUB"
				},
				Capture = true,
				Description = "",
				Confirmation = new CYokassa_Confirmation
				{
					ReturnUrl = ReturnUrl,
					Type = "redirect"
				},
				SavePaymentMethod = true,
				PaymentMethodData = new CYokassa_PaymentMethodData
				{
					Type = "bank_card",
					CardData = new CYokassa_CardData
					{
						CardHolder = "TEST USER",
						Csc = CVV,
						Number = CardNumber,
						ExpiryMonth = MonthExp,
						ExpiryYear = YearExp
					}
				}
			};

			string json = JsonConvert.SerializeObject(body);

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

			CYokassa_BuyResult? result = JsonConvert.DeserializeObject<CYokassa_BuyResult>(response_str);

			return result?.PaymentMethod?.Id;
#else
            _log.LogWarning(message: "Невозможно зарегистрировать карту на проде");
            return null;
#endif
		}

        /// <summary>
        /// <para>Генерирует ссылку на чек через 1ОФД (не используется сейчас нигде)</para>
        /// <para><see cref="IsRefund"/> отвечает за получение чека, если перепутать тип платежа то чек невозможно будет получить</para>
        /// </summary>
        /// <param name="Pay_Ident">Идентификатор платежа по ЮКассе</param>
        /// <param name="IsRefund">Является ли платеж позвратом</param>
        /// <returns>Ссылка на чек или null</returns>
        public string? GenerateReceiptUrl(string Pay_Ident, bool IsRefund)
		{
#if DEBUG
			return null;
#else
            if (_password.StartsWith("test_"))
            {
                _log.LogWarning(message: "Невозможно получить данные чека в тестовом магазине (не поддерживается)");
                return null;
            }

            if (string.IsNullOrEmpty(Pay_Ident))
            {
                return null;
            }

            HttpClient client = new()
            {
                BaseAddress = new Uri("https://api.yookassa.ru/v3/")
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;

            string baseUrl = $"https://api.yookassa.ru/v3/receipts?payment_id={Pay_Ident}";
            if (IsRefund)
            {
                baseUrl = $"https://api.yookassa.ru/v3/receipts?refund_id={Pay_Ident}";
            }

            HttpRequestMessage requestMessage = new(HttpMethod.Get, baseUrl);
            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            CYokassa_ReceiptData? result = JsonConvert.DeserializeObject<CYokassa_ReceiptData>(response_str);
            CYokassa_ReceiptItem? item = result?.Items?.FirstOrDefault();
            if (item == null)
            {
                return null;
            }

            string fiscalAttribute = item.Fiscal_Attribute ?? "";
            string fiscalDocumentNumber = item.Fiscal_Document_Number ?? "";
            string fiscalStorageNumber = item.Fiscal_Storage_Number ?? "";

            if (string.IsNullOrEmpty(fiscalAttribute) || string.IsNullOrEmpty(fiscalDocumentNumber) || string.IsNullOrEmpty(fiscalStorageNumber))
            {
                return null;
            }

            string url = OFD_URL;
            url += $"?fn={fiscalStorageNumber}";
            url += $"&i={fiscalDocumentNumber}";
            url += $"&fp={fiscalAttribute}";
            if (IsRefund)
            {
                url += "&n=2";
            }
            else
            {
                url += "&n=1";
            }

            return url;
#endif
		}

        /// <summary>
        /// Получает информацию о платеже в ЮКассе (не используется сейчас нигде)
        /// </summary>
        /// <param name="paymentId">Идентификатор платежа</param>
        /// <returns></returns>
        public CYokassa_Pay_Result? GetPaymentInfo(string paymentId)
		{
			if (string.IsNullOrWhiteSpace(paymentId))
			{
				return null;
			}

			string baseUrl = $"https://api.yookassa.ru/v3/payments/{paymentId}";

			HttpClient client = new()
			{
				BaseAddress = new Uri(baseUrl)
			};
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;
			string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

			HttpRequestMessage requestMessage = new(HttpMethod.Get, baseUrl);
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);

			HttpResponseMessage response = client.Send(requestMessage);
			string responseStr = response.Content.ReadAsStringAsync().Result;
			CYokassa_Pay_Result? result = JsonConvert.DeserializeObject<CYokassa_Pay_Result>(responseStr);
			return result;
		}
	}
}