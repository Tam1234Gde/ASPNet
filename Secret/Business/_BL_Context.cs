using System.Reflection;
using HIDE.RestAndRant.Business.Endpoints;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Сборный класс для данных
    /// </summary>
    public class BL_Context
    {

        /// <summary>
        /// Бронирование
        /// </summary>
        public Banners Banners { get; } = new();

        /// <summary>
        /// Бронирование
        /// </summary>
        public Bookings Bookings { get; } = new();


        /// <summary>
        /// Бонусы клиента
        /// </summary>
        public Bonuses Bonuses { get; } = new();


        /// <summary>
        /// Города
        /// </summary>
        public Cities Cities { get; } = new();


        /// <summary>
        /// Меню ресторана
        /// </summary>
        public Restaurant_Menus Restaurant_Menus { get; } = new();


        /// <summary>
        /// Блюда меню ресторана
        /// </summary>
        public Restaurant_Menu_Dishes Restaurant_Menu_Dishes { get; } = new();


        /// <summary>
        /// Ингредиент блюда в меню ресторана
        /// </summary>
        public Restaurant_Menu_Dish_Ingredients Restaurant_Menu_Dish_Ingredients { get; } = new();


        /// <summary>
        /// Объем блюда в меню ресторана
        /// </summary>
        public Restaurant_Menu_Dish_Volumes Restaurant_Menu_Dish_Volumes { get; } = new();


        /// <summary>
        /// Доступные типы оплаты в ресторане
        /// </summary>
        public Restaurant_PayTypes Restaurant_PayTypes { get; } = new();


        /// <summary>
        /// Клиенты
        /// </summary>
        public Clients Clients { get; } = new();


        /// <summary>
        /// Нотификации клиентам
        /// </summary>
        public Client_Notifications Client_Notifications { get; set; } = new();


        /// <summary>
        /// Работа в Excel
        /// </summary>
        public Client_Notifications_Excel ClientNotificationsExcel { get; set; } = new();


        /// <summary>
        /// Регистрация токенов уведомлений клиентов
        /// </summary>
        public Client_Notification_Tokens Client_Notification_Tokens { get; set; } = new();


        /// <summary>
        /// Работы с запланированными пуш-уведомлениями
        /// </summary>
        public Client_Notifications_Planned Cllient_Notification_Planned { get; set; } = new();



        /// <summary>
        /// Платежи по картам
        /// </summary>
        public ClientPay_Cards ClientPay_Cards { get; set; } = new();


        /// <summary>
        /// Цвета
        /// </summary>
        public Colors Colors { get; } = new();


        /// <summary>
        /// Блюда
        /// </summary>
        public Dishes Dishes { get; } = new();


        /// <summary>
        /// Изображения
        /// </summary>
        public Images Images { get; } = new();


        /// <summary>
        /// Категории блюд
        /// </summary>
        public Dish_Categories Dish_Categories { get; } = new();


        /// <summary>
        /// Категории ингридиентов
        /// </summary>
        public Dish_IngridientCategory Dish_IngridientCategory { get; } = new();


        /// <summary>
        /// Обьемы блюда
        /// </summary>
        public Dish_Volumes Dish_Volumes { get; } = new();


        /// <summary>
        /// Цвета блюда
        /// </summary>
        public Dish_Colors Dish_Colors { get; } = new();


        /// <summary>
        /// Ингридиенты блюда
        /// </summary>
        public Dish_Ingridients Dish_Ingridients { get; } = new();


        /// <summary>
        /// Ингридиенты
        /// </summary>
        public Ingridients Ingridients { get; } = new();


        /// <summary>
        /// Оценка приложения
        /// </summary>
        public Ratings Ratings { get; } = new();


        /// <summary>
        /// Статусы столика
        /// </summary>
        public Restaurant_Table_Statuses Restaurant_Table_Statuses { get; } = new();


        /// <summary>
        /// Отправки смс
        /// </summary>
        public Smses Smses { get; } = new();


        /// <summary>
        /// Объёмы
        /// </summary>
        public Volumes Volumes { get; } = new();


        /// <summary>
        /// Работа с заказами
        /// </summary>
        public Orders Orders { get; } = new();


        /// <summary>
        /// Типы заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        public Order_Types Order_Types { get; } = new();


        /// <summary>
        /// Доставки
        /// </summary>
        public Order_Deliveries Order_Deliveries { get; } = new();


        /// <summary>
        /// Работа с избранными заказами
        /// </summary>
        public Order_Favorites Order_Favorites { get; } = new();


        /// <summary>
        /// Работа с заказами
        /// </summary>
        public OrderStatuses OrderStatuses { get; } = new();

        /// <summary>
        /// Типы оплаты
        /// </summary>
        public Pay_Types Pay_Types { get; } = new();


        /// <summary>
        /// Рестораны
        /// </summary>
        public Restaurants Restaurants { get; } = new();


        /// <summary>
        /// Столики в ресторане
        /// </summary>
        public Restaurant_Tables Restaurant_Tables { get; } = new();


        /// <summary>
        /// Типы столиков в ресторане
        /// </summary>
        public Restaurant_Table_Types Restaurant_Table_Types { get; } = new();


        /// <summary>
        /// Залы в ресторане
        /// </summary>
        public Restaurant_Halls Restaurant_Halls { get; } = new();


        /// <summary>
        /// Параметры
        /// </summary>
        public Setups Setups { get; } = new();


        /// <summary>
        /// Пользователи системы
        /// </summary>
        public Users Users { get; } = new();


        /// <summary>
        /// Сервисные методы
        /// </summary>
        public ZDeveloper ZDeveloper { get; } = new();


        /// <summary>
        /// Получение пути к библиотеке
        /// </summary>
        public static string GetAssemblyPath
        {
            get
            {
                return $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            }
        }


        public Endpoints_ForMenu.Menu Menu { get; } = new();
        public Endpoints_ForMenu.Menu_Category Menu_Category { get; } = new();
        public Endpoints_ForMenu.Menu_Subcategories Menu_Subcategory { get; } = new();
        public Endpoints_ForMenu.Menu_Dish Menu_Dish { get; } = new();
        public Endpoints_ForMenu.Menu_Ingridient Menu_Ingredient { get; } = new();
        public Endpoints_ForMenu.Menu_Ingridient_Group Menu_Ingredient_Group { get; } = new();
        public Endpoints_ForMenu.Menu_Restaurant Menu_Restaurant { get; } = new();
        public Endpoints_ForMenu.Menu_Dish_Ingridient Menu_Dish_Ingredient { get; } = new();
    }
}