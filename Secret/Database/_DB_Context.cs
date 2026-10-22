using Microsoft.EntityFrameworkCore;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database
{
    public class DBContext : DbContext
    {

        #region MenuUpdate

        /// <summary>
        /// Меню
        /// </summary>
        public DbSet<Menu_Def> Menus { get; set; } = null!;


        /// <summary>
        /// Категории меню
        /// </summary>
        public DbSet<Menu_Category_Def> Menu_Categories { get; set; } = null!;


        /// <summary>
        /// Блюда меню
        /// </summary>
        public DbSet<Menu_Dish_Def> Menu_Dishes { get; set; } = null!;


        /// <summary>
        /// Ингридиенты блюд меню
        /// </summary>
        public DbSet<Menu_Dish_Ingridient_Def> Menu_Dish_Ingridients { get; set; } = null!;


        /// <summary>
        /// Ингредиенты меню
        /// </summary>
        public DbSet<Menu_Ingridient_Def> Menu_Ingridients { get; set; } = null!;


        /// <summary>
        /// Группы ингредиентов меню
        /// </summary>
        public DbSet<Menu_Ingridient_Group_Def> Menu_Ingridient_Groups { get; set; } = null!;


        /// <summary>
        /// Словарь состояния активности для таблицы настроек нового меню
        /// </summary>
        /// </summary>
        public DbSet<Menu_OptionState_Def> Menu_OptionStates { get; set; } = null!;


        /// <summary>
        /// Меню ресторана
        /// </summary>
        public DbSet<Menu_Restaurant_Def> Menu_Restaurants { get; set; } = null!;


        /// <summary>
        /// Опции меню ресторана
        /// </summary>
        public DbSet<Menu_Restaurant_Option_Def> Menu_Restaurant_Options { get; set; } = null!;


        /// <summary>
        /// Подкатегории меню
        /// </summary>
        public DbSet<Menu_Subcategory_Def> Menu_Subcategories { get; set; } = null!;

        #endregion MenuUpdate


        public DbSet<Booking> Bookings { get; set; } = null!;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<Bonus> Bonuses { get; set; } = null!;


        /// <summary>
        /// Общая Таблица пуш-уведомлений (и отправленные и запланированные)
        /// </summary>
        public DbSet<Client_Notification> Client_Notifications { get; set; } = null!;


        /// <summary>
        /// Таблица с записями запланированных пушей
        /// </summary>
        public DbSet<Client_Notification_Planned> Client_Notification_Planned { get; set; } = null!;


        /// <summary>
        /// Таблица с запланированными получаетелями пуш-уведомлений (если отправка конкретным пользователям через таблицу)
        /// </summary>
        public DbSet<Client_Notification_Planned_Data> Client_Notification_Planned_Data { get; set; } = null!;


        /// <summary>
        /// Таблица с записями статистики отправлений по токенам
        /// </summary>
        //public DbSet<Client_Notification_Stats> Client_Notification_Stats { get; set; } = null!;


        /// <summary>
        /// Таблица с токенами для отправки пуш-уведомлений
        /// </summary>
        public DbSet<Client_Notification_Token> Client_Notification_Tokens { get; set; } = null!;


        /// <summary>
        /// Таблица с шаблонами пуш-уведомлений
        /// </summary>
        public DbSet<Client_Notification_Shablon> Client_Notification_Shablons { get; set; } = null!;


        public DbSet<Dish> Dishes { get; set; } = null!;

        public DbSet<Dish_Category> Dish_Categories { get; set; } = null!;

        public DbSet<Dish_Volume> Dish_Volumes { get; set; } = null!;

        public DbSet<Ingridient> Ingridients { get; set; } = null!;


        /// <summary>
        /// Категории ингридиентов
        /// </summary>
        public DbSet<Dish_Ingridient_Category> Dish_Ingridient_Categories { get; set; } = null!;

        public DbSet<Rating> Ratings { get; set; } = null!;

        public DbSet<Volume> Volumes { get; set; } = null!;

        public DbSet<Restaurant> Restaurants { get; set; } = null!;

        public DbSet<Color> Colors { get; set; } = null!;

        public DbSet<Dish_Ingridient> Dish_Ingridients { get; set; } = null!;

        public DbSet<Dish_Color> Dish_Colors { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Order_Data> Order_Datas { get; set; } = null!;

        public DbSet<Order_Data_Ingridient> Order_Data_Ingridients { get; set; } = null!;

        public DbSet<Order_Favorite> Order_Favorites { get; set; } = null!;

        public DbSet<Order_Favorite_Data> Order_Favorite_Datas { get; set; } = null!;

        public DbSet<Order_Favorite_Data_Ingridient> Order_Favorites_Data_Ingridients { get; set; } = null!;

        public DbSet<Order_Status> Order_Statuses { get; set; } = null!;

        public DbSet<Order_Type> Order_Types { get; set; } = null!;

        public DbSet<Restaurant_Table> Restaurant_Tables { get; set; } = null!;

        public DbSet<Restaurant_Table_Type> Restaurant_Table_Types { get; set; } = null!;

        public DbSet<Restaurant_Table_Status> Restaurant_Table_Statuses { get; set; } = null!;

        public DbSet<Restaurant_Hall> Restaurant_Halls { get; set; } = null!;

        public DbSet<Setup> Setups { get; set; } = null!;

        public DbSet<Sms_Log> Sms_Logs { get; set; } = null!;

        public DbSet<Order_Pay_Status> Order_Pay_Statuses { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Restaurant_Menu> Restaurant_Menus { get; set; } = null!;
        public DbSet<Restaurant_Menu_Dish> Restaurant_Menu_Dishes { get; set; } = null!;
        public DbSet<Restaurant_Menu_Dish_Ingredient> Restaurant_Menu_Dish_Ingredients { get; set; } = null!;
        public DbSet<Restaurant_Menu_Dish_Volume> Restaurant_Menu_Dish_Volumes { get; set; } = null!;
        public DbSet<Restaurant_PayType> Restaurant_PayTypes { get; set; } = null!;

        public DbSet<Order_Delivery> Order_Deliveries { get; set; } = null!;

        public DbSet<Pay_Type> Pay_Types { get; set; } = null!;

        public DbSet<ClientPay_Card_Pay> ClientPay_Card_Pays { get; set; } = null!;

        public DbSet<ClientPay_Card_Data> ClientPay_Card_Datas { get; set; } = null!;

        public DbSet<ClientPay_Card_Type> ClientPay_Card_Types { get; set; } = null!;

        public DbSet<Banner> Banners { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (string.IsNullOrWhiteSpace(AppConfig.ConnectionString))
            {
#if DEBUG
                AppConfig.ConnectionString = Consts_Debug.DB_ConnectionString;
#else
                throw new KeyNotFoundException("Не указан ConnectionString");
#endif
            }

            optionsBuilder.UseSqlServer(AppConfig.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // НАСТРОЙКА НОВЫХ ТАБЛИЦ МЕНЮ
            // ============================================

            #region MenuUpdate - Настройка связей новых таблиц меню

            // Настройка связей с Menu_OptionState_Def
            // Используем Restrict чтобы избежать множественных каскадных путей

            // Menu_Def -> Menu_OptionState_Def
            modelBuilder.Entity<Menu_Def>()
                .HasOne(m => m.Menu_OptionState)
                .WithMany()
                .HasForeignKey(m => m.Menu_OptionStateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Dish_Def -> Menu_OptionState_Def
            modelBuilder.Entity<Menu_Dish_Def>()
                .HasOne(m => m.Menu_OptionState)
                .WithMany()
                .HasForeignKey(m => m.Menu_OptionStateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Ingridient_Def -> Menu_OptionState_Def
            modelBuilder.Entity<Menu_Ingridient_Def>()
                .HasOne(m => m.Menu_OptionState)
                .WithMany()
                .HasForeignKey(m => m.Menu_OptionStateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Ingridient_Group_Def -> Menu_OptionState_Def
            modelBuilder.Entity<Menu_Ingridient_Group_Def>()
                .HasOne(m => m.Menu_OptionState)
                .WithMany()
                .HasForeignKey(m => m.Menu_OptionStateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Def -> Menu_OptionState_Def
            modelBuilder.Entity<Menu_Restaurant_Def>()
                .HasOne(m => m.Menu_OptionState)
                .WithMany()
                .HasForeignKey(m => m.Menu_OptionStateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей с Menu_Def

            // Menu_Category_Def -> Menu_Def
            modelBuilder.Entity<Menu_Category_Def>()
                .HasOne(m => m.Menu)
                .WithMany()
                .HasForeignKey(m => m.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Subcategory_Def -> Menu_Category_Def
            modelBuilder.Entity<Menu_Subcategory_Def>()
                .HasOne(m => m.Menu_Category)
                .WithMany()
                .HasForeignKey(m => m.Menu_CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Subcategory_Def -> Menu_Subcategory_Def (Self-reference для Parent)
            modelBuilder.Entity<Menu_Subcategory_Def>()
                .HasOne(m => m.Parent)
                .WithMany()
                .HasForeignKey(m => m.ParentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Dish_Def -> Menu_Subcategory_Def
            modelBuilder.Entity<Menu_Dish_Def>()
                .HasOne(m => m.Menu_Subcategory)
                .WithMany()
                .HasForeignKey(m => m.Menu_SubcategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Ingridient_Def -> Menu_Def
            modelBuilder.Entity<Menu_Ingridient_Def>()
                .HasOne(m => m.Menu)
                .WithMany()
                .HasForeignKey(m => m.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Ingridient_Def -> Menu_Ingridient_Group_Def
            modelBuilder.Entity<Menu_Ingridient_Def>()
                .HasOne(m => m.Menu_Ingridient_Group)
                .WithMany()
                .HasForeignKey(m => m.Menu_Ingridient_GroupID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Ingridient_Group_Def -> Menu_Def
            modelBuilder.Entity<Menu_Ingridient_Group_Def>()
                .HasOne(m => m.Menu)
                .WithMany()
                .HasForeignKey(m => m.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Dish_Ingridient_Def -> Menu_Dish_Def
            modelBuilder.Entity<Menu_Dish_Ingridient_Def>()
                .HasOne(m => m.Menu_Dish)
                .WithMany()
                .HasForeignKey(m => m.Menu_DishID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Dish_Ingridient_Def -> Menu_Ingridient_Def
            modelBuilder.Entity<Menu_Dish_Ingridient_Def>()
                .HasOne(m => m.Menu_Ingridient)
                .WithMany()
                .HasForeignKey(m => m.Menu_IngridientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Def -> Menu_Def
            modelBuilder.Entity<Menu_Restaurant_Def>()
                .HasOne(m => m.Menu)
                .WithMany()
                .HasForeignKey(m => m.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Def -> Restaurant
            modelBuilder.Entity<Menu_Restaurant_Def>()
                .HasOne(m => m.Restaurant)
                .WithMany()
                .HasForeignKey(m => m.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Option_Def -> Menu_Restaurant_Def
            modelBuilder.Entity<Menu_Restaurant_Option_Def>()
                .HasOne(m => m.Menu_Restaurant)
                .WithMany()
                .HasForeignKey(m => m.Menu_RestaurantID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Option_Def -> Menu_Dish_Def
            modelBuilder.Entity<Menu_Restaurant_Option_Def>()
                .HasOne(m => m.Menu_Dish)
                .WithMany()
                .HasForeignKey(m => m.Menu_DishID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Option_Def -> Menu_Ingridient_Def
            modelBuilder.Entity<Menu_Restaurant_Option_Def>()
                .HasOne(m => m.Menu_Ingridient)
                .WithMany()
                .HasForeignKey(m => m.Menu_IngridientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Option_Def -> Menu_Ingridient_Group_Def
            modelBuilder.Entity<Menu_Restaurant_Option_Def>()
                .HasOne(m => m.Menu_Ingridient_Group)
                .WithMany()
                .HasForeignKey(m => m.Menu_Ingridient_GroupID)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu_Restaurant_Option_Def -> Menu_Def
            modelBuilder.Entity<Menu_Restaurant_Option_Def>()
                .HasOne(m => m.Menu)
                .WithMany()
                .HasForeignKey(m => m.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка для свойств с точностью decimal в Menu_Dish_Def
            modelBuilder.Entity<Menu_Dish_Def>()
                .Property(m => m.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Menu_Dish_Def>()
                .Property(m => m.Bonuses)
                .HasPrecision(8, 2);

            #endregion MenuUpdate - Настройка связей новых таблиц меню


            #region Client - клиент

            Client client;
            int client_id = 0;

            client = new()
            {
                ID = ++client_id,
                Phone = "79282000000",
                Guid = "395df047-314d-4111-8532-be3bcbba6e99",
                Name = "Тест HIDEerv",
                Date_LastAction = new DateTime(year: 2000, month: 01, day: 02),
                Code_Phone = "1111",
            };
            modelBuilder.Entity<Client>().HasData(client);

            client = new()
            {
                ID = ++client_id,
                Phone = "79282000001",
                Guid = "395df047-314d-4111-8532-be3bcbba6e98",
                Name = "Тест HIDEerv",
                Date_LastAction = new DateTime(year: 2000, month: 01, day: 02),
                Code_Phone = "1111",
            };
            modelBuilder.Entity<Client>().HasData(client);

            #endregion Client - клиент


            #region User - операторы

            User user;
            int user_id = 0;

            user = new()
            {
                ID = ++user_id,
                Name = "HIDEerv",
                Login = "support@HIDEerv.ru",
                Password = "12345!",
                Flag_Admin = true,
            };
            modelBuilder.Entity<User>().HasData(user);

            #endregion User - операторы


            #region Setup - настройки

            Setup setup;

            setup = new()
            {
                Key = Consts.SetupKey_Dishes_DateChange,
                Val = new DateTime(year: 2023, month: 12, day: 25).ToString(),
            };
            modelBuilder.Entity<Setup>().HasData(setup);

            setup = new()
            {
                Key = Consts.SetupKey_DefaultWorkHours,
                Val_Comment = "Часы работы по умолчанию для родительских категорий (через запятую)"
            };
            setup = new()
            {
                Key = Consts.SetupKey_Restaurants_DefaultWorkHours,
                Val_Comment = "Часы работы по умолчанию для ресторанов"
            };
            setup = new()
            {
                Key = Consts.SetupKey_DefaultPhoneNumber,
                Val_Comment = "Номер телефона по умолчанию"
            };
            setup = new()
            {
                Key = Consts.SetupKey_UseDefaultWorkHours,
                Val_Comment = "Использовать часы работы по умолчанию для родительских категорий"
            };


            #endregion Setup - настройки


            #region Pay_Type - вид оплаты

            Pay_Type pay_Type;

            pay_Type = new()
            {
                ID = EPayType.Money,
                Name = "Наличные"
            };
            modelBuilder.Entity<Pay_Type>().HasData(pay_Type);

            pay_Type = new()
            {
                ID = EPayType.CardInApp,
                Name = "Карта в приложении"
            };
            modelBuilder.Entity<Pay_Type>().HasData(pay_Type);

            pay_Type = new()
            {
                ID = EPayType.CardByTerminal,
                Name = "Оплата через терминал"
            };
            modelBuilder.Entity<Pay_Type>().HasData(pay_Type);

            #endregion Pay_Type - вид оплаты


            #region Color - цвет

            Color color;
            int color_id = 0;


            color = new()
            {
                ID = ++color_id,
                Name = "Красный",
                Name_Eng = "Red",
                HexColor = "FF0000",
            };
            modelBuilder.Entity<Color>().HasData(color);

            color = new()
            {
                ID = ++color_id,
                Name = "Зеленый",
                Name_Eng = "Green",
                HexColor = "00FF00",
            };
            modelBuilder.Entity<Color>().HasData(color);

            color = new()
            {
                ID = ++color_id,
                Name = "Синий",
                Name_Eng = "Blue",
                HexColor = "0000FF",
            };
            modelBuilder.Entity<Color>().HasData(color);

            #endregion Color - цвет


            #region Ingridient - Ингридиенты

            Ingridient ingridient;
            int ingridient_id = 0;

            ingridient = new()
            {
                ID = ++ingridient_id,
                Name = "Клубника"
            };
            modelBuilder.Entity<Ingridient>().HasData(ingridient);


            ingridient = new()
            {
                ID = ++ingridient_id,
                Name = "Смородина"
            };
            modelBuilder.Entity<Ingridient>().HasData(ingridient);

            #endregion Ingridient - Ингридиенты


            #region Order_Status - статусы заказа
            Order_Status order_Status;

            order_Status = new()
            {
                ID = EOrder_Status.New,
                Name = "Новый заказ",
                OrderNum = 1,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);

            order_Status = new()
            {
                ID = EOrder_Status.Cooking,
                Name = "Заказ готовится",
                OrderNum = 3,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);


            order_Status = new()
            {
                ID = EOrder_Status.ReadyToClient,
                Name = "Заказ готов к выдаче",
                OrderNum = 5,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);


            order_Status = new()
            {
                ID = EOrder_Status.Delivery,
                Name = "Заказ передан в доставку",
                OrderNum = 7,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);


            order_Status = new()
            {
                ID = EOrder_Status.Complete,
                Name = "Заказ завершен",
                OrderNum = 9,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);


            order_Status = new()
            {
                ID = EOrder_Status.Cancelled,
                Name = "Заказ отменен",
                OrderNum = 11,
            };
            modelBuilder.Entity<Order_Status>().HasData(order_Status);


            #endregion Order_Status - статусы заказа


            #region Order_Type - Тип заказа(С собой\Поесть в кафе\Доставка)

            Order_Type order_Type;

            order_Type = new()
            {
                ID = EOrder_Type.Myself,
                Name = "С собой",
            };
            modelBuilder.Entity<Order_Type>().HasData(order_Type);

            order_Type = new()
            {
                ID = EOrder_Type.Incafe,
                Name = "Поесть в кафе",
            };
            modelBuilder.Entity<Order_Type>().HasData(order_Type);

            order_Type = new()
            {
                ID = EOrder_Type.Delivery,
                Name = "Доставка",
            };
            modelBuilder.Entity<Order_Type>().HasData(order_Type);

            #endregion Тип заказа(С собой\Поесть в кафе\Доставка)


            #region Restaurant_Table_Type - Тип столика(Обычный\VIP\Веранда)

            Restaurant_Table_Type restaurant_Table_Type;

            restaurant_Table_Type = new()
            {
                ID = ERestaurant_Table_Type.Ordinary,
                Name = "Обычный",
            };
            modelBuilder.Entity<Restaurant_Table_Type>().HasData(restaurant_Table_Type);

            restaurant_Table_Type = new()
            {
                ID = ERestaurant_Table_Type.VIP,
                Name = "VIP",
            };
            modelBuilder.Entity<Restaurant_Table_Type>().HasData(restaurant_Table_Type);

            restaurant_Table_Type = new()
            {
                ID = ERestaurant_Table_Type.Veranda,
                Name = "Веранда",
            };
            modelBuilder.Entity<Restaurant_Table_Type>().HasData(restaurant_Table_Type);

            #endregion Restaurant_Table_Type - Тип столика(Обычный\VIP\Веранда)


            #region Restaurant_Table_Status - Статус столика(Не забронирован\Забронирован\Недоступен)

            Restaurant_Table_Status restaurant_Table_Status;

            restaurant_Table_Status = new()
            {
                ID = ERestaurant_Table_Status.Available,
                Name = "Не забронирован",
            };
            modelBuilder.Entity<Restaurant_Table_Status>().HasData(restaurant_Table_Status);

            restaurant_Table_Status = new()
            {
                ID = ERestaurant_Table_Status.Unavailable,
                Name = "Забронирован",
            };
            modelBuilder.Entity<Restaurant_Table_Status>().HasData(restaurant_Table_Status);

            restaurant_Table_Status = new()
            {
                ID = ERestaurant_Table_Status.Disabled,
                Name = "Недоступен",
            };
            modelBuilder.Entity<Restaurant_Table_Status>().HasData(restaurant_Table_Status);

            #endregion Restaurant_Table_Type - Статус столика(Не забронирован\Забронирован\Недоступен)


            #region Restaurant - рестораны

            Restaurant restaurant;
            int restaurant_id = 0;

            restaurant = new()
            {
                ID = ++restaurant_id,
                Adress = "Грозный, проспект Путина, 34",
                Latitude = "43.318333",
                Longitude = "45.690265",
                Hours_Work = "09:00;22:00",
                Hours_WorkApp = "12:00;15:00"
            };
            modelBuilder.Entity<Restaurant>().HasData(restaurant);

            #endregion Restaurant - рестораны


            #region Volume - Объёмы

            Volume volume;
            int volume_id = 0;

            volume = new()
            {
                ID = ++volume_id,
                Name = "",
                Value = 250
            };
            modelBuilder.Entity<Volume>().HasData(volume);


            volume = new()
            {
                ID = ++volume_id,
                Name = "",
                Value = 350
            };
            modelBuilder.Entity<Volume>().HasData(volume);

            volume = new()
            {
                ID = ++volume_id,
                Name = "",
                Value = 450
            };
            modelBuilder.Entity<Volume>().HasData(volume);

            #endregion Volume - Объёмы


            #region Dish, Dish_Category, Dish_Volume  - Продукты, категории продуктов, обьемы продуктов

            Dish dish;
            int dish_id = 0;

            Dish_Category dish_Category;
            int dish_Category_id = 0;

            Dish_Volume dish_Volume;

            Dish_Ingridient dish_Ingridient;
            int dish_Ingridient_id = 0;
            int dish_Volume_id = 0;

            dish_Category = new()
            {
                ID = ++dish_Category_id,
                Name = "Кофе",
                Flag_AllowColors = true,
            };
            modelBuilder.Entity<Dish_Category>().HasData(dish_Category);

            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Кофе Первое",
                Flag_AllowColors = true,
                Price = 50m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 1,
                VolumeID = 1,
                Price = 100.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Кофе второе",
                Flag_AllowColors = true,
                Price = 60m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 2,
                VolumeID = 1,
                Price = 100.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);

            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 2,
                VolumeID = 2,
                Price = 150.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Кофе третье",
                Flag_AllowColors = true,
                Price = 70m
            };
            modelBuilder.Entity<Dish>().HasData(dish);

            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 3,
                VolumeID = 1,
                Price = 100.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);

            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 3,
                VolumeID = 2,
                Price = 150.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);

            dish_Volume = new()
            {
                ID = ++dish_Volume_id,
                DishID = 3,
                VolumeID = 3,
                Price = 200.0m,
            };
            modelBuilder.Entity<Dish_Volume>().HasData(dish_Volume);

            dish_Category = new()
            {
                ID = ++dish_Category_id,
                Name = "Пироженные",
            };
            modelBuilder.Entity<Dish_Category>().HasData(dish_Category);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Пироженное первое",
                Price = 100m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Пироженное второе",
                Price = 200m
            };
            modelBuilder.Entity<Dish>().HasData(dish);

            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Пироженное третье",
                Price = 300m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish_Category = new()
            {
                ID = ++dish_Category_id,
                Name = "Мороженное",
            };
            modelBuilder.Entity<Dish_Category>().HasData(dish_Category);

            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Мороженное первое",
                Price = 100m
            };
            modelBuilder.Entity<Dish>().HasData(dish);

            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Мороженное второе",
                Price = 150m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Мороженное третье",
                Price = 200m
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish_Category = new()
            {
                ID = ++dish_Category_id,
                Name = "Каша",
            };
            modelBuilder.Entity<Dish_Category>().HasData(dish_Category);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Каша без ингридиентов",
            };
            modelBuilder.Entity<Dish>().HasData(dish);


            dish = new()
            {
                ID = ++dish_id,
                Dish_CategoryID = dish_Category_id,
                Name = "Каша с ингридиентами",
                Price = 100m
            };
            modelBuilder.Entity<Dish>().HasData(dish);




            dish_Ingridient = new()
            {
                ID = ++dish_Ingridient_id,
                DishID = dish_id,
                IngridientID = ingridient_id - 1,
                Price = 100.0m,
            };
            modelBuilder.Entity<Dish_Ingridient>().HasData(dish_Ingridient);


            dish_Ingridient = new()
            {
                ID = ++dish_Ingridient_id,
                DishID = dish_id,
                IngridientID = ingridient_id,
                Price = 1200.0m,
            };
            modelBuilder.Entity<Dish_Ingridient>().HasData(dish_Ingridient);

            #endregion Dish, Dish_Category, Dish_Volume  - Продукты, категории продуктов, обьемы продуктов


            #region Order_Pay_Status - Статус оплаты заказа

            Order_Pay_Status order_Pay_Status;

            order_Pay_Status = new()
            {
                ID = EOrder_Pay_Status.WaitPay,
                Name = "Ожидает оплаты"
            };
            modelBuilder.Entity<Order_Pay_Status>().HasData(order_Pay_Status);

            order_Pay_Status = new()
            {
                ID = EOrder_Pay_Status.Payed,
                Name = "Оплачен"
            };
            modelBuilder.Entity<Order_Pay_Status>().HasData(order_Pay_Status);

            order_Pay_Status = new()
            {
                ID = EOrder_Pay_Status.NeedRetry,
                Name = "Попытка повторной оплаты"
            };
            modelBuilder.Entity<Order_Pay_Status>().HasData(order_Pay_Status);


            order_Pay_Status = new()
            {
                ID = EOrder_Pay_Status.Error,
                Name = "Ошибка при оплате"
            };
            modelBuilder.Entity<Order_Pay_Status>().HasData(order_Pay_Status);


            order_Pay_Status = new()
            {
                ID = EOrder_Pay_Status.Canceled,
                Name = "Заказ отменен"
            };
            modelBuilder.Entity<Order_Pay_Status>().HasData(order_Pay_Status);

            #endregion Order_Pay_Status - Статус оплаты заказа


            #region ClientPay_Card_Pay_Type - Типы платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)

            ClientPay_Card_Pay_Type clientPay_Card_Pay_Type;

            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.AddInitialPay,
                Name = "Инициализационный платеж (при привязке карты)"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);

            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.ReturnInitialPay,
                Name = "Возврат инициализационного платежа (при привязке карты)"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);


            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.AddPay,
                Name = "Платеж"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);

            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.ReturnPay,
                Name = "Возврат платежа"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);


            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.AddRecurrentPay,
                Name = "Рекурентный платеж"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);

            clientPay_Card_Pay_Type = new()
            {
                ID = EClientPay_Card_Pay_Type.ReturnRecurrentPay,
                Name = "Возврат рекурентного платежа"
            };
            modelBuilder.Entity<ClientPay_Card_Pay_Type>().HasData(clientPay_Card_Pay_Type);

            #endregion ClientPay_Card_Pay_Type - Типы платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)


            #region  ClientPay_Card_Type - типы карт:  MasterCard, Visa, Mir, UnionPay, JCB, AmericanExpress, DinersClub,...

            #region DEL By MAE - 14.05.2026 - Временно заккоментированно, разобраться
            //TODO: MAE - нет комментария что это временно законментировано, и не меняй стиль комментирования блоков

            //ClientPay_Card_Type clientPay_Card_Type;

            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Unknown,
            //    SName = "Unknown",
            //    Name = "Прочая",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.MasterCard,
            //    SName = "Mastercard",
            //    Name = "Mastercard",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Visa,
            //    SName = "Visa",
            //    Name = "Visa",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Mir,
            //    SName = "Mir",
            //    Name = "Mir",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.UnionPay,
            //    SName = "UnionPay",
            //    Name = "UnionPay",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.JCB,
            //    SName = "JCB",
            //    Name = "JCB",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.AmericanExpress,
            //    SName = "AmericanExpress",
            //    Name = "AmericanExpress",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.DinersClub,
            //    SName = "DinersClub",
            //    Name = "DinersClub",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.DiscoverCard,
            //    SName = "DiscoverCard",
            //    Name = "DiscoverCard",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.InstaPayment,
            //    SName = "InstaPayment",
            //    Name = "InstaPayment",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Laser,
            //    SName = "Laser",
            //    Name = "Laser",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Dankort,
            //    SName = "Dankort",
            //    Name = "Dankort",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Solo,
            //    SName = "Solo",
            //    Name = "Solo",
            //};
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type);


            //clientPay_Card_Type = new()
            //{
            //    ID = EClientPay_Card_Type.Switch,
            //    SName = "Switch",
            //    Name = "Switch",
            //modelBuilder.Entity<ClientPay_Card_Type>().HasData(clientPay_Card_Type); 
            #endregion DEL By MAE - 14.05.2026 - Временно заккоментированно, разобраться

            #endregion  ClientPay_Card_Type - типы карт:  MasterCard, Visa, Mir, UnionPay, JCB, AmericanExpress, DinersClub,...


            #region  Client_Notification_Shablon - шаблоны клиентских нотификаций

            Client_Notification_Shablon client_Notification_Shablon;

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.Auth,
                Name = "Успешный вход!",
                Content = "Поздравляю, вы успешно авторизовались."
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.New,
                Name = "Новый заказ!",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) оформлен.",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.Cancelled,
                Name = "Заказ отменен!",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) отменен.",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.Cooking,
                Name = "Заказ готовится!",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) готовится.",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.ReadyToClient,
                Name = "Заказ готов!",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) готов к выдаче. Ждем вас в кофейне по адресу: ((###HIDEERV2###)).",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.Delivery,
                Name = "Заказ передан в доставку",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) передан в доставку.",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            client_Notification_Shablon = new()
            {
                ID = EClient_Notification_Shablon.Complete,
                Name = "Заказ завершен!",
                Content = "Ваш заказ под номером ((###HIDEERV1###)) завершен.",
                Push_Need = true
            };
            modelBuilder.Entity<Client_Notification_Shablon>().HasData(client_Notification_Shablon);

            #endregion  Client_Notification_Shablon - шаблоны клиентских нотификаций


            #region MenuUpdate - Начальные данные

            // Menu_OptionState - состояния активности
            modelBuilder.Entity<Menu_OptionState_Def>().HasData(
                new Menu_OptionState_Def
                {
                    ID = 1,
                    Value = EMenu_OptionStates.Enabled
                },
                new Menu_OptionState_Def
                {
                    ID = 2,
                    Value = EMenu_OptionStates.Optional
                },
                new Menu_OptionState_Def
                {
                    ID = 3,
                    Value = EMenu_OptionStates.Disabled
                }
            );

            #endregion MenuUpdate - Начальные данные
        }
    }
}