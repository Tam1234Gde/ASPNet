namespace HIDE.RestAndRant.Common
{
    /// <summary>
    /// Класс для хранения конфигурации приложения
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// Строка соединения с БД
        /// </summary>
        public static string? ConnectionString { get; set; }


        /// <summary>
        /// Guid для проверки при выполнении скрипт
        /// </summary>
        public static string Guid_Script { get; set; } = null!;


        /// <summary>
        /// Диапазон заказа, указан в минутах
        /// </summary>
        public static int RangeOrder { get; set; }


        /// <summary>
        /// Диапазон времени при бронировании для веранды, указан в минутах
        /// </summary>
        public static int RangeBookingVeranda { get; set; }


        /// <summary>
        /// Диапазон времени при бронировании для обычного столика, указан в минутах
        /// </summary>
        public static int RangeBookingVIP { get; set; }


        /// <summary>
        /// Системный клиент
        /// </summary>
        public static int SystemClient { get; set; }


        /// <summary>
        /// Ресторан по умолчанию
        /// </summary>
        public static int Restaurant_Default { get; set; }


        /// <summary>
        /// Разрешить выполнение скрипта
        /// </summary>
        public static bool Allow_Script { get; set; }


        /// <summary>
        /// Разрешить выполнение скрипта
        /// </summary>
        public static bool Yookassa_SendReceipt { get; set; }


        /// <summary>
        /// Путь к файлу Excel для коммерческих данных
        /// </summary>
        public static string Excel_CommercialPersonal { get; set; } = null!;


        #region Testers_Code - Коды SMS для тестирования

        /// <summary>
        /// Значение кода подтверждения, устанавливающегося тестировщикам
        /// </summary>
        public static string? Testers_Code_Val { get; set; }


        /// <summary>
        /// Коды пользователей, для которых не будут сбрасываться коды при входе (для тестировщиков)
        /// </summary>
        public static List<int>? Testers_Code_ClientIDs { get; set; }

        #endregion Testers_Code - Коды SMS для тестирования


        #region Настройки SMS

        /// <summary>
        /// Токен для отправки СМС кода
        /// </summary>
        public static string SMS_Token { get; set; } = null!;


        /// <summary>
        /// Шаблон для отправки СМС кода
        /// </summary>
        public static string SMS_Shablon { get; set; } = null!;


        /// <summary>
        /// Флаг того, что нужно отправлять СМС
        /// </summary>
        public static bool SMS_Send { get; set; }

        #endregion Настройки SMS


        #region Настройки почты

        /// <summary>
        /// Настройки почты - SMTP_Host
        /// </summary>
        public static string Mail_SMTP_Host { get; set; } = null!;


        /// <summary>
        /// Настройки почты - SMTP_Port
        /// </summary>
        public static int Mail_SMTP_Port { get; set; }


        /// <summary>
        /// Настройки почты - Flag_EnableSSL
        /// </summary>
        public static bool Mail_SMTP_FlagEnableSSL { get; set; }


        /// <summary>
        /// Настройки почты - SMTP_User
        /// </summary>
        public static string Mail_SMTP_User { get; set; } = null!;


        /// <summary>
        /// Настройки почты - SMTP_Passwd
        /// </summary>
        public static string Mail_SMTP_Passwd { get; set; } = null!;


        /// <summary>
        /// Настройки почты - MailFrom
        /// </summary>
        public static string Mail_MailFrom { get; set; } = null!;


        /// <summary>
        /// Настройки почты - MailReply
        /// </summary>
        public static string? Mail_MailReply { get; set; }


        /// <summary>
        /// Флаг того, что нужно отправлять Почту
        /// </summary>
        public static bool Mail_Send { get; set; }

        #endregion Настройки почты


        #region Настройка YooKassa

        /// <summary>
        /// Логин от api-сервиса Юкасса
        /// </summary>
        public static string Yookassa_Login { get; set; } = null!;


        /// <summary>
        /// Пароль от api-сервиса Юкасса
        /// </summary>
        public static string Yookassa_Password { get; set; } = null!;


        /// <summary>
        /// Адрес от api-сервиса Юкасса
        /// </summary>
        public static string Yookassa_Url { get; set; } = null!;

        #endregion Настройка YooKassa


        #region Настройка TelegramBot

        /// <summary>
        /// Токен
        /// </summary>
        public static string TG_Token { get; set; } = null!;


        /// <summary>
        /// Url
        /// </summary>
        public static string TG_ProxyUrl { get; set; } = null!;


        /// <summary>
        /// Код чат-бота
        /// </summary>
        public static string TG_ChatID { get; set; } = null!;


        /// <summary>
        /// Код чат-бота доставки
        /// </summary>
        public static string TG_ChatDeliveryID { get; set; } = null!;


        /// <summary>
        /// Код чат-бота для бронирования столиков
        /// </summary>
        public static string TG_ChatBookingID { get; set; } = null!;


        /// <summary>
        /// Код чат-бота
        /// </summary>
        public static string TG_ChatDopID { get; set; } = null!;


        /// <summary>
        /// True - Отправлять сообщения в чат-бот
        /// </summary>
        public static bool TG_Active { get; set; }


        #endregion Настройка TelegramBot


        #region Настройка Пуш-Уведомлений

        /// <summary>
        /// Допустимое количество дней последней активности для отправки пуш уведомлений
        /// </summary>
        public static int Allowed_Activity_Days_Notification { get; set; }

        #endregion Настройка Пуш-Уведомлений
    }
}