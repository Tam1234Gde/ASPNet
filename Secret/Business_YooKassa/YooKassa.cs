using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HIDE.RestAndRant.Business_YooKassa.Definitions;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business_YooKassa
{
    /// <summary>
    /// Работа с Юкассой
    /// </summary>
    public class Yookassa
    {
        private const string DEFAULT_CURRENCY = "RUB";
        private const string CONFIRMATION_TYPE_REDIRECT = "redirect";
        private const string PAYMENT_METHOD_TYPE_BANK_CARD = "bank_card";
        private const string CARD_HOLDER_DEFAULT = "TEST USER";
        private const string RECEIPT_ITEM_NAME_SUBSCRIPTION = "Оплата подписки";
        private const int MAX_METADATA_KEYS = 16;
        private const int MAX_METADATA_KEY_LENGTH = 32;
        private const int MAX_METADATA_VALUE_LENGTH = 512;
        private const int DEFAULT_INITIAL_SLEEP = 5000;
        private const string TEST_PASSWORD_PREFIX = "test_";
        public const string OFD_URL = "https://consumer.1-ofd.ru/ticket";

        private const string LOG_TEMPLATE_CONSTRUCTOR = "Constructor";
        private const string LOG_TEMPLATE_DEBUG = "{0}";
        private const string LOG_TEMPLATE_DEBUG_JSON = "JSON: {0} ## {1}";
        private const string LOG_TEMPLATE_INFO_RETURN_SUCCESS = "Возврат проведен успешно, данные возврата: {0}";
        private const string LOG_TEMPLATE_WARNING_RETURN_FAILED = "Возврат не произведен, данные возврата: {0} ## {1}";
        private const string LOG_TEMPLATE_ERROR_RETURN_DATA_FAILED = "Не удалось получить данные о возврате с API Yookassa # {0} ## {1}";
        private const string LOG_TEMPLATE_INFO_RECURENT = "{0}";
        private const string LOG_TEMPLATE_ERROR_METADATA_KEYS = "Максимальное количество ключей - 16";
        private const string LOG_TEMPLATE_ERROR_METADATA_KEY_LENGTH = "Максимальная длина для ключа - 32 символа";
        private const string LOG_TEMPLATE_ERROR_METADATA_VALUE_LENGTH = "Максимальная длина для значения - 512 символов";
        private const string LOG_TEMPLATE_WARNING_REGISTER_CARD_PROD = "Невозможно зарегистрировать карту на проде";
        private const string LOG_TEMPLATE_WARNING_RECEIPT_TEST = "Невозможно получить данные чека в тестовом магазине (не поддерживается)";


        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Логин
        /// </summary>
        private readonly string _login;


        /// <summary>
        /// Пароль
        /// </summary>
        private readonly string _password;


        /// <summary>
        /// Параметры сериалайзера
        /// </summary>
        private static readonly JsonSerializerOptions _jsonserializer_options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        public Yookassa(string Login, string Password)
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace(message: LOG_TEMPLATE_CONSTRUCTOR);

            _login = Login;
            _password = Password;
        }


        /// <summary>
        /// Возврат денежных средств
        /// </summary>
        /// <param name="Payment_id">Идентификатор платежа</param>
        /// <param name="Summ">Сумма возврата</param>
        /// <param name="RestaurantLogin">Логин ресторана</param>
        /// <param name="RestaurantPassw">Пароль ресторана</param>
        /// <param name="YooKassaID">Идентификатор платежа по финалу возврата</param>
        /// <param name="Log">Логи ошибки</param>
        /// <returns>true - если платеж возвращён, false - в противном случае</returns>
        public bool Refund(string Payment_id, decimal Summ, string? RestaurantLogin, string? RestaurantPassw, out string? YooKassaID, out string? Log)
        {
            string loginfo = $"Payment_id='{Payment_id}' Summ='{Summ}' ";

            string loginYoukassa = _login;
            string passwYoukassa = _password;

            if (!string.IsNullOrWhiteSpace(RestaurantLogin) && !string.IsNullOrWhiteSpace(RestaurantPassw))
            {
                loginYoukassa = RestaurantLogin;
                passwYoukassa = RestaurantPassw;
            }

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: LOG_TEMPLATE_DEBUG, args: loginfo);
            }

            CYokassa_Refund refund = new CYokassa_Refund
            {
                Amount = new CYokassa_Amount
                {
                    Value = Summ.ToString("#.00").ReHIDE(",", "."),
                    Currency = DEFAULT_CURRENCY
                },
                Payment_id = Payment_id
            };

            string json = JsonConvert.SerializeObject(refund);

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: LOG_TEMPLATE_DEBUG_JSON, args: [json, loginfo]);
            }

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{loginYoukassa}:{passwYoukassa}"));

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AppConfig.Yookassa_Url}refunds");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            StringBuilder sb = new StringBuilder();
            sb.Append($"StatusCode='{response.StatusCode}'");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    CYokassa_Refund_Out? cYokassa_Refund_Out = System.Text.Json.JsonSerializer.Deserialize<CYokassa_Refund_Out>(response_str, _jsonserializer_options);

                    if (cYokassa_Refund_Out != null)
                    {
                        Log = null;

                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: LOG_TEMPLATE_INFO_RETURN_SUCCESS, args: response_str);
                        }

                        YooKassaID = cYokassa_Refund_Out.ID;
                        return true;
                    }
                    else
                    {
                        _log.LogError(message: LOG_TEMPLATE_ERROR_RETURN_DATA_FAILED, args: [response_str, loginfo]);
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
                _log.LogWarning(message: LOG_TEMPLATE_WARNING_RETURN_FAILED, args: [json, response_str]);
                sb.Append(response_str);
                Log = sb.ToString();
                YooKassaID = null;
                return false;
            }
        }


        /// <summary>
        /// Возврат денежных средств
        /// </summary>
        /// <param name="Payment_Id">Идентификатор платежа</param>
        /// <param name="Summ">Сумма возврата</param>
        /// <returns>Результат возврата</returns>
        public CYokassa_Refund_Result Refund(string Payment_Id, decimal Summ)
        {
            string loginfo = $"Payment_id='{Payment_Id}' Summ='{Summ}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: LOG_TEMPLATE_DEBUG, args: loginfo);
            }

            CYokassa_Refund refund = new CYokassa_Refund
            {
                Amount = new CYokassa_Amount
                {
                    Value = Summ.ToString("#.00").ReHIDE(",", "."),
                    Currency = DEFAULT_CURRENCY
                },
                Payment_id = Payment_Id
            };

            string json = JsonConvert.SerializeObject(refund);

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: LOG_TEMPLATE_DEBUG_JSON, args: [json, loginfo]);
            }

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AppConfig.Yookassa_Url}refunds");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            CYokassa_Refund_Result? res = System.Text.Json.JsonSerializer.Deserialize<CYokassa_Refund_Result>(response_str);
            return res ?? new CYokassa_Refund_Result();
        }


        /// <summary>
        /// Рекурентный платеж
        /// </summary>
        /// <param name="Card_Id">Идентификатор карты</param>
        /// <param name="Summ">Сумма платежа</param>
        /// <param name="Description">Описание платежа</param>
        /// <param name="email">Email клиента</param>
        /// <param name="phone">Телефон клиента</param>
        /// <param name="initialSleep">Задержка при запуска платежа, ЮКасса может выкинуть 500 или too_many_requests если часто вызывать</param>
        /// <returns>Результат платежа</returns>
        public CYokassa_Pay_Result? Recurent(string Card_Id, decimal Summ, string Description, string email, string phone, int initialSleep = DEFAULT_INITIAL_SLEEP)
        {
            if (initialSleep > 0)
            {
                Thread.Sleep(initialSleep);
            }

            CYokassa_Amount amount = new CYokassa_Amount
            {
                Value = Summ.ToString("#.00").ReHIDE(",", "."),
                Currency = DEFAULT_CURRENCY
            };

            CYokassa_Recurent refund = new CYokassa_Recurent
            {
                Amount = amount,
                Capture = true,
                CardIdent = Card_Id,
                Description = Description
            };

            if (_password.Contains("live"))
            {
                List<CYokassa_ReceiptItemInfo> receiptItems = new List<CYokassa_ReceiptItemInfo>
                {
                    new CYokassa_ReceiptItemInfo(RECEIPT_ITEM_NAME_SUBSCRIPTION, amount)
                };

                refund.Receipt = new CYokassa_Receipt
                {
                    Customer = new CYokassa_ReceiptCustomer(email, phone),
                    Items = receiptItems
                };
            }

            string json = JsonConvert.SerializeObject(refund);

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AppConfig.Yookassa_Url}payments");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: LOG_TEMPLATE_INFO_RECURENT, args: response_str);
            }

            CYokassa_Pay_Result? result = JsonConvert.DeserializeObject<CYokassa_Pay_Result>(response_str);
            return result;
        }


        /// <summary>
        /// Создаёт ссылку для оплаты
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
                if (Metadata.Count > MAX_METADATA_KEYS)
                {
                    _log.LogError(message: LOG_TEMPLATE_ERROR_METADATA_KEYS);
                    return null;
                }

                foreach (KeyValuePair<string, object> item in Metadata)
                {
                    string key = item.Key;
                    object value = item.Value;

                    if (key.Length > MAX_METADATA_KEY_LENGTH)
                    {
                        _log.LogError(message: LOG_TEMPLATE_ERROR_METADATA_KEY_LENGTH);
                        return null;
                    }

                    string val = value.ToString() ?? string.Empty;
                    if (val.Length > MAX_METADATA_VALUE_LENGTH)
                    {
                        _log.LogError(message: LOG_TEMPLATE_ERROR_METADATA_VALUE_LENGTH);
                        return null;
                    }
                }
            }

            CYokassa_Buy yokassaBuy = new CYokassa_Buy
            {
                Amount = new CYokassa_Amount
                {
                    Value = Summ.ToString("#.00").ReHIDE(",", "."),
                    Currency = DEFAULT_CURRENCY
                },
                Capture = true,
                Description = Descrition,
                Confirmation = new CYokassa_Confirmation
                {
                    ReturnUrl = ReturnUrl,
                    Type = CONFIRMATION_TYPE_REDIRECT
                },
                SavePaymentMethod = SavePayment,
                Metadata = Metadata
            };

            string json = JsonConvert.SerializeObject(yokassaBuy);

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AppConfig.Yookassa_Url}payments");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            CYokassa_BuyResult? result = JsonConvert.DeserializeObject<CYokassa_BuyResult>(response_str);

            return result?.Confirmation?.ConfirmationUrl;
        }


        /// <summary>
        /// Регистрирует новую карту в ЮКассе, использовать только в DEBUG режиме и только для тестов
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
            CYokassa_AddCardBody body = new CYokassa_AddCardBody
            {
                Amount = new CYokassa_Amount
                {
                    Value = Summ.ToString("#.00").ReHIDE(",", "."),
                    Currency = DEFAULT_CURRENCY
                },
                Capture = true,
                Description = string.Empty,
                Confirmation = new CYokassa_Confirmation
                {
                    ReturnUrl = ReturnUrl,
                    Type = CONFIRMATION_TYPE_REDIRECT
                },
                SavePaymentMethod = true,
                PaymentMethodData = new CYokassa_PaymentMethodData
                {
                    Type = PAYMENT_METHOD_TYPE_BANK_CARD,
                    CardData = new CYokassa_CardData
                    {
                        CardHolder = CARD_HOLDER_DEFAULT,
                        Csc = CVV,
                        Number = CardNumber,
                        ExpiryMonth = MonthExp,
                        ExpiryYear = YearExp
                    }
                }
            };

            string json = JsonConvert.SerializeObject(body);

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            string baseString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_login}:{_password}"));

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{AppConfig.Yookassa_Url}payments");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", baseString);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.Send(requestMessage);
            string response_str = response.Content.ReadAsStringAsync().Result;

            CYokassa_BuyResult? result = JsonConvert.DeserializeObject<CYokassa_BuyResult>(response_str);

            return result?.PaymentMethod?.Id;
#else
            _log.LogWarning(message: LOG_TEMPLATE_WARNING_REGISTER_CARD_PROD);
            return null;
#endif
        }


#if DEBUG
        /// <summary>
        /// <para>Генерирует ссылку на чек через 1ОФД</para>
        /// <para><see cref="IsRefund"/> отвечает за получение чека, если перепутать тип платежа то чек невозможно будет получить</para>
        /// </summary>
        /// <param name="Pay_Ident">Идентификатор платежа по ЮКассе</param>
        /// <param name="IsRefund">Является ли платеж возвратом</param>
        /// <returns>Ссылка на чек или null</returns>
#pragma warning disable IDE0060 // Remove unused parameter
        public string? GenerateReceiptUrl(string Pay_Ident, bool IsRefund)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            return null;
        }
#else
        /// <summary>
        /// <para>Генерирует ссылку на чек через 1ОФД</para>
        /// <para><see cref="IsRefund"/> отвечает за получение чека, если перепутать тип платежа то чек невозможно будет получить</para>
        /// </summary>
        /// <param name="Pay_Ident">Идентификатор платежа по ЮКассе</param>
        /// <param name="IsRefund">Является ли платеж возвратом</param>
        /// <returns>Ссылка на чек или null</returns>
        public string? GenerateReceiptUrl(string Pay_Ident, bool IsRefund)
        {
            if (_password.StartsWith(TEST_PASSWORD_PREFIX))
            {
                _log.LogWarning(message: LOG_TEMPLATE_WARNING_RECEIPT_TEST);
                return null;
            }

            if (string.IsNullOrEmpty(Pay_Ident))
            {
                return null;
            }

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.Yookassa_Url)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;

            string baseUrl = $"{AppConfig.Yookassa_Url}receipts?payment_id={Pay_Ident}";
            if (IsRefund)
            {
                baseUrl = $"{AppConfig.Yookassa_Url}receipts?refund_id={Pay_Ident}";
            }

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, baseUrl);
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

            string fiscalAttribute = item.Fiscal_Attribute ?? string.Empty;
            string fiscalDocumentNumber = item.Fiscal_Document_Number ?? string.Empty;
            string fiscalStorageNumber = item.Fiscal_Storage_Number ?? string.Empty;

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
        }
#endif
    }
}