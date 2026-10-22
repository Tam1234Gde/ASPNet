namespace HIDE.RestAndRant.Common.Consts
{
    /// <summary>
    /// Константы
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// Данные в невыбранном Combobox - строка
        /// </summary>
        public const int NoData_Int = -1;

        /// <summary>
        /// Данные в невыбранном Combobox - строка
        /// </summary>
        public const string NoData_Str = "-";

        /// <summary>
        /// Статус для API: OK
        /// </summary>
        public const string Status_OK = "OK";

        /// <summary>
        /// Статус для API: Err
        /// </summary>
        public const string Status_Err = "Err";

        /// <summary>
        /// Статус для API: OK
        /// </summary>
        public const int Status_Success = 200;

        /// <summary>
        /// Количество кружок доя бонуса
        /// </summary>
        public const int Bonus_Count = 10;

        /// <summary>
        /// Папка для хранения файлов
        /// </summary>
        public const string FileFolder = "_Files";

        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения заказа
        /// </summary>
        public const string SetupKey_Orders_DateChange = "Orders_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения заказа
        /// </summary>
        public const string SetupKey_OrdersFavorite_DateChange = "OrdersFavorite_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения меню
        /// </summary>
        public const string SetupKey_Dishes_DateChange = "Dishes_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения баннеров
        /// </summary>
        public const string SetupKey_Banners_DateChange = "Banners_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты дней работы
        /// </summary>
        public const string SetupKey_Restaurants_DefaultWorkHours = "Restaurants_DefaultWorkHours";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения ресторанов
        /// </summary>
        public const string SetupKey_Restaurants_DateChange = "Restaurants_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения городов
        /// </summary>
        public const string SetupKey_Cities_DateChange = "Cities_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения доступных типов оплаты в ресторане
        /// </summary>
        public const string SetupKey_Restaurant_PayTypes_DateChange = "Restaurant_PayTypes_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения столиков в ресторане
        /// </summary>
        public const string SetupKey_Restaurant_Tables_DateChange = "Restaurant_Tables_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения типов столика в ресторане
        /// </summary>
        public const string SetupKey_Restaurant_Table_Types_DateChange = "Restaurant_Table_Types_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения статусов столика в ресторане
        /// </summary>
        public const string SetupKey_Restaurant_Table_Statuses_DateChange = "Restaurant_Table_Statuses_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения цветов для блюд
        /// </summary>
        public const string SetupKey_Colors_DateChange = "Colors_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения ингредиентов
        /// </summary>
        public const string SetupKey_Ingridients_DateChange = "Ingridients_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения объёмов
        /// </summary>
        public const string SetupKey_Volumes_DateChange = "Volumes_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения клиентов
        /// </summary>
        public const string SetupKey_Clients_DateChange = "Clients_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения карты клиента
        /// </summary>
        public const string SetupKey_Cards_DateChange = "Cards_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения количества бонусов клиента
        /// </summary>
        public const string SetupBonuses_DateChange = "Bonuses_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения столов в ресторане
        /// </summary>
        public const string SetupRestaurant_Tables_DateChange = "Restaurant_Tables_DateChange";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения бронирования столиков\залов
        /// </summary>
        public const string SetupBookings_DateChange = "Bookings_DateChange";


        /// <summary>
        /// Папка для хранения изображений блюд
        /// </summary>
        public const string ImageFolder_Dishes = "_Images/Dishes";

        /// <summary>
        /// Папка для хранения изображений блюд
        /// </summary>
        public const string ImageFolder_Banners = "_Images/Banners";


        /// <summary>
        /// Папка для хранения изображений ингредиентов
        /// </summary>
        public const string ImageFolder_Ingridients = "_Images/Ingridients";


        /// <summary>
        /// Папка для хранения изображений клиента
        /// </summary>
        public const string ImageFolder_Clients = "_Images/Clients";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для даты изменения диапазона заказа(Значение в минутах)
        /// </summary>
        public const string SetupKey_Range_Order = "Range_Order";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для дополнительного времени при приготовлении заказа (Значение в минутах)
        /// </summary>
        public const string SetupKey_Dop_CookingTime = "Dop_CookingTime";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для количества блюд, при котором добавляется дополнительное время
        /// </summary>
        public const string SetupKey_Dop_Cooking_DishCount = "Dop_Cooking_DishCount";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для стоимости доставки
        /// </summary>
        public const string SetupKey_Dop_Delivery_Price = "Dop_Delivery_Price";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для суммы при которой доставка становится бесплатной
        /// </summary>
        public const string SetupKey_Dop_Delivery_PriceFree = "Dop_Delivery_PriceFree";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для проверки работоспособности приложения
        /// </summary>
        public const string SetupKey_Dop_CheckApp = "Dop_CheckApp";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для всех отключенных городов
        /// </summary>
        public const string SetupKey_Dop_HiddenAllCities = "Dop_HiddenAllCities";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для максимального количества минут для одного заказа с большим количеством позиций
        /// </summary>
        public const string SetupKey_Dop_MaxTime_ForOrder = "Dop_MaxTime_ForOrder";


        /// <summary>
        /// Часы работы по умолчанию для родительских категорий
        /// </summary>
        public const string SetupKey_DefaultWorkHours = "DefaultWorkHours";


        /// <summary>
        /// Номер телефона по умолчанию
        /// </summary>
        public const string SetupKey_DefaultPhoneNumber = "DefaultPhoneNumber";


        /// <summary>
        /// Использовать часы работы по умолчанию
        /// </summary>
        public const string SetupKey_UseDefaultWorkHours = "UseDefaultWorkHours";


        /// <summary>
        /// Ресторан - Название cookies
        /// </summary>
        public const string Cookies_Restaurant = "Cookies_Restaurant";


        /// <summary>
        /// Столик в ресторане - Название cookies
        /// </summary>
        public const string Cookies_RestaurantTable = "Cookies_RestaurantTable";


        /// <summary>
        /// Ресторан - Название cookies
        /// </summary>
        public const string Cookies_Cart = "Cookies_Cart";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для текста в окно "Требуется обновить приложение"
        /// </summary>
        public const string SetupKey_Dop_AppUpdate = "Dop_AppUpdate";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для версии приложения IOS
        /// </summary>
        public const string SetupKey_Dop_AppUpdateVersionIOS = "Dop_AppUpdateVersionIOS";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для версии приложения Android
        /// </summary>
        public const string SetupKey_Dop_AppUpdateVersionAndroid = "Dop_AppUpdateVersionAndroid";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для стоимости предоплаты брони
        /// </summary>
        public const string SetupKey_Dop_BookingPrice = "Dop_BookingPrice";


        /// <summary>
        /// Ключ для таблицы настроек (Setup) - Ключ для текста при бронировании столиков(Бронировать можно на ближайшие N месяцев)
        /// </summary>
        public const string SetupKey_Dop_BookingText = "Dop_BookingText";


        /// <summary>
        /// Ключ для даты изменения меню
        /// </summary>
        public const string SetupKey_Menu_DateChange = "Menu_DateChange";
    }
}