using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Delivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Volumes",
                comment: "Объёмы");

            migrationBuilder.AlterTable(
                name: "Users",
                comment: "Пользователи");

            migrationBuilder.AlterTable(
                name: "Sms_Logs",
                comment: "Логи отправки СМС");

            migrationBuilder.AlterTable(
                name: "Setups",
                comment: "Установки");

            migrationBuilder.AlterTable(
                name: "Restaurants",
                comment: "Рестораны");

            migrationBuilder.AlterTable(
                name: "Restaurant_Tables",
                comment: "Столы в ресторане");

            migrationBuilder.AlterTable(
                name: "Pay_Types",
                comment: "Виды оплаты");

            migrationBuilder.AlterTable(
                name: "Orders",
                comment: "Заказы");

            migrationBuilder.AlterTable(
                name: "Order_Statuses",
                comment: "Статусы заказа");

            migrationBuilder.AlterTable(
                name: "Order_Favorites_Data_Ingridients",
                comment: "Детали содержимого избранного заказа - ингредиенты");

            migrationBuilder.AlterTable(
                name: "Order_Favorites",
                comment: "Избранные заказы");

            migrationBuilder.AlterTable(
                name: "Order_Favorite_Datas",
                comment: "Содержимое избранного заказа");

            migrationBuilder.AlterTable(
                name: "Order_Datas",
                comment: "Содержимое заказа");

            migrationBuilder.AlterTable(
                name: "Order_Data_Ingridients",
                comment: "Детали содержимого заказа - ингредиенты");

            migrationBuilder.AlterTable(
                name: "Ingridients",
                comment: "Ингредиенты для блюда");

            migrationBuilder.AlterTable(
                name: "Dishes",
                comment: "Блюда>");

            migrationBuilder.AlterTable(
                name: "Dish_Volumes",
                comment: "Объёмы блюда");

            migrationBuilder.AlterTable(
                name: "Dish_Ingridients",
                comment: "Ингридиенты блюд");

            migrationBuilder.AlterTable(
                name: "Dish_Colors",
                comment: "Цвета блюда");

            migrationBuilder.AlterTable(
                name: "Dish_Categories",
                comment: "Категории блюд");

            migrationBuilder.AlterTable(
                name: "Colors",
                comment: "Цвет стакана");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Pays",
                comment: "Данные о платежах");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Pay_Type",
                comment: "Типы платежа");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Datas",
                comment: "Данные карты клиента");

            migrationBuilder.AlterTable(
                name: "Client_Notifications",
                comment: "Уведомления клиента");

            migrationBuilder.AlterTable(
                name: "Client_Notification_Shablons",
                comment: "Шаблоны уведомлений");

            migrationBuilder.AlterTable(
                name: "Bookings",
                comment: "Бронирование");

            migrationBuilder.AlterTable(
                name: "Bonuses",
                comment: "Бонусы клиента");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Volumes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Значение",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Volumes",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Volumes",
                type: "nvarchar(max)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Значение",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Volumes",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Volumes",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Volumes",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Users",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Пароль",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "ФИО пользователя",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Логин",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Root",
                table: "Users",
                type: "bit",
                nullable: false,
                comment: "Флаг суперадмин",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Oper",
                table: "Users",
                type: "bit",
                nullable: false,
                comment: "Флаг оператор",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Users",
                type: "bit",
                nullable: false,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Admin",
                table: "Users",
                type: "bit",
                nullable: false,
                comment: "Флаг админ",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Users",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Users",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "Users",
                type: "int",
                nullable: true,
                comment: "Код ресторана");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Sms_Logs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Текст, который был отправлен в смс",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Sms_Logs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Телефон на который отправили СМС",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Sms_Logs",
                type: "datetime2",
                nullable: false,
                comment: "Дата отправки",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Sms_Logs",
                type: "int",
                nullable: false,
                comment: "Клиент, которому отправлено СМС - код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Sms_Logs",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Val4",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи 4",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи");

            migrationBuilder.AlterColumn<string>(
                name: "Val3",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи 3",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи");

            migrationBuilder.AlterColumn<string>(
                name: "Val2",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи 2",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи");

            migrationBuilder.AlterColumn<string>(
                name: "Val",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи 1",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Setups",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Ключ",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Долгота",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Широта",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Hours_WorkApp",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Часы работы ресторана для заказа в приложении через ;",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hours_Work",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Часы работы ресторана через ;",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Restaurants",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Адрес",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Restaurants",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "Delivery_Price",
                table: "Restaurants",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Стоимость доставки");

            migrationBuilder.AddColumn<int>(
                name: "Delivery_Time",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "Время доставки в минутах");

            migrationBuilder.AddColumn<int>(
                name: "Express_Delivery_Time",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "Время срочной доставки в минутах");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Car",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Есть возможность вынести к машине");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Delivery",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Возможна доставка");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Incafe",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Есть возможность поесть в кафе");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Myself",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Есть возможность забрать с собой");

            migrationBuilder.AddColumn<string>(
                name: "Hours_HardWork",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Часы загрузки работы ресторана через ;");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Restaurant_Tables",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: false,
                comment: "Ресторан - код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование стола",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsAvailableApp",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                comment: "Флаг доступности к заказу через приложение",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsAvailable",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                comment: "Флаг доступности к заказу через официанта",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Restaurant_Tables",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Coordinates",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Координаты стола для админки",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Restaurant_Table_TypeID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: true,
                comment: "Тип столика - код");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pay_Types",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Pay_Types",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Ресторан, куда совершается заказ - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Телефон, на кого заказ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Pay_TypeID",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Вид оплаты - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order_StatusID",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Статус заказа - код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order_FavoriteID",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Код избранного заказа",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Имя, на кого заказ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Payed",
                table: "Orders",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что заказ оплачен",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Inhouse",
                table: "Orders",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что клиент будет кушать в кафе, если false - то на вынос",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Orders",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_To",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                comment: "Дата и время к которому приготовить заказ",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastOrderStatusChange",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                comment: "Дата последнего изменения статуса заказа",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                comment: "Дата заказа",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Комментарий к заказу",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_PayID",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Операция по карте, прошедшая по этой подписке последней - код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BookingID",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Бронирование - код");

            migrationBuilder.AddColumn<decimal>(
                name: "Delivery_Price",
                table: "Orders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Стоимость доставки");

            migrationBuilder.AddColumn<decimal>(
                name: "Handing",
                table: "Orders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Дать сдачи с ...");

            migrationBuilder.AddColumn<int>(
                name: "Order_TypeID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Тип заказа(С собой\\Поесть в кафе\\Доставка) - код");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Order_Statuses",
                type: "int",
                nullable: false,
                comment: "Порядок следования",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Order_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Statuses",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Order_Pay_Statuses",
                type: "int",
                nullable: true,
                comment: "Пользователь, установивший пометку на удаление - код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order_Favorite_DataID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Ссылка на позицию заказа - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Summ",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Общая сумма ингредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Price",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Цена ингредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Ingridient_Name",
                table: "Order_Favorites_Data_Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование ингредиента в момент оформления",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Count",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Количество игредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Ингредиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Order_Favorites",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                comment: "Ресторан, куда совершается заказ - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Order_Favorites",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Телефон, на кого заказ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Pay_TypeID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                comment: "Вид оплаты - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order_StatusID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                comment: "Статус заказа - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Order_Favorites",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Имя, на кого заказ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Payed",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что заказ оплачен",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Inhouse",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что клиент будет кушать в кафе, если false - то на вынос",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_To",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: true,
                comment: "Дата и время к которому приготовить заказ",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastEdit",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: false,
                comment: "Дата последнего изменения избранного заказа",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: false,
                comment: "Дата добавления заказа в избранное",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderFavoriteID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                comment: "Избранный заказ - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumeValue",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Объём",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumePrice",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Цена",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Dish_VolumeID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: true,
                comment: "Объем - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Summ",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Общая сумма позиции",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Price",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Стоимость позиции",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Dish_Name",
                table: "Order_Favorite_Datas",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование товара в момент оформления",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Count",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Количество",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Dish_ColorID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: true,
                comment: "Цвет - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                comment: "Блюдо - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                comment: "Заказ - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumeValue",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Объём",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumePrice",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Цена",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Dish_VolumeID",
                table: "Order_Datas",
                type: "int",
                nullable: true,
                comment: "Код объёма",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Summ",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Общая сумма позиции",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Price",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Стоимость позиции",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Dish_Name",
                table: "Order_Datas",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование товара в момент оформления",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Count",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Количество",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Dish_ColorID",
                table: "Order_Datas",
                type: "int",
                nullable: true,
                comment: "Цвет - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                comment: "Блюдо - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses_Plus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Количество начисленных бонусов",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses_Minus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Количество списанных бонусов",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Order_DataID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Ссылка на позицию заказа - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Summ",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Общая сумма ингредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Price",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Цена ингредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Ingridient_Name",
                table: "Order_Data_Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование ингредиента в момент оформления",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Count",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Количество игредиента",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Ингредиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Ingridients",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование ингредиента",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Image_ChangeDate",
                table: "Ingridients",
                type: "datetime2",
                nullable: true,
                comment: "Дата изменения изображения",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Ingridients",
                type: "varbinary(max)",
                nullable: true,
                comment: "Изображение ингредиента",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Ingridients",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Ingridients",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Ingridients",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dishes",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Полное описание блюда",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Краткое описание блюда",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Стоимость",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dishes",
                type: "int",
                nullable: false,
                comment: "Порядковый номер",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование товара",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Image_ChangeDate",
                table: "Dishes",
                type: "datetime2",
                nullable: true,
                comment: "Дата изменения изображения",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Dishes",
                type: "varbinary(max)",
                nullable: true,
                comment: "Изображение блюда",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dishes",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Bonus",
                table: "Dishes",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что блюдо можно купить за бонусы",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_AllowColors",
                table: "Dishes",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_CategoryID",
                table: "Dishes",
                type: "int",
                nullable: false,
                comment: "Категория блюда - код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dishes",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dishes",
                type: "int",
                nullable: true,
                comment: "Количество минут для приготовления блюда",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses",
                table: "Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Количество бонусов, начисляемых за покупку",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dishes",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "VolumeID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                comment: "Объем - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Volumes",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish_Volumes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Цена за этот обьем",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                comment: "Порядковый номер",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Volumes",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                comment: "Блюдо - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Volumes",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Стоимость",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                comment: "Ингридиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IngridientGroupID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                comment: "Группа",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                comment: "Блюдо - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                comment: "Количество минут для приготовления",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Colors",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Colors",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                comment: "Блюдо - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Colors",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                comment: "Цвет ID",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Categories",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dish_Categories",
                type: "int",
                nullable: false,
                comment: "Порядковый номер",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dish_Categories",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование категории",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_AllowColors",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что в категории у товаров разрешены цвета",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Categories",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Categories",
                type: "int",
                nullable: true,
                comment: "Количество минут для приготовления блюд в данной категории",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Categories",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Colors",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name_Eng",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование цвета - англ",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование цвета",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HexColor",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Цвет стакана",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Colors",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Colors",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Colors",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Clients",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true,
                comment: "Аватар пользователя",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Телефон",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Имя пользователя",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Clients",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Guid",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsPolicy",
                table: "Clients",
                type: "bit",
                nullable: false,
                comment: "Политика принята",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Clients",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Registration",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                comment: "Дата регистрации",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_PhotoUpdate",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                comment: "Дата обновления фотографии",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastZakaz",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                comment: "Дата последнего заказа",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastUpdate",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                comment: "Дата последнего изменения информации клиента",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastAction",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                comment: "Дата последнего входа в приложение",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Birth",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                comment: "Дата рождения",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code_Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Смс код",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Clients",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Summ",
                table: "ClientPay_Card_Pays",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                comment: "Сумма платежа",
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentIdent",
                table: "ClientPay_Card_Pays",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Идентификатор платежа, генерируемый платежной системой",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: true,
                comment: "Заказ - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Complete",
                table: "ClientPay_Card_Pays",
                type: "bit",
                nullable: false,
                comment: "Флаг того, что операция завершена успешно, если он не стоит - то нужно повторять попытки на API",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Create",
                table: "ClientPay_Card_Pays",
                type: "datetime2",
                nullable: false,
                comment: "Дата создания платежа",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Complete",
                table: "ClientPay_Card_Pays",
                type: "datetime2",
                nullable: true,
                comment: "Дата удаления",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_Pay_TypeID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                comment: "Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...) - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_DataID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                comment: "Карта, которой совершался платеж - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: true,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClientPay_Card_Pay_Type",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Pay_Type",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "ClientPay_Card_Datas",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Update",
                table: "ClientPay_Card_Datas",
                type: "datetime2",
                nullable: false,
                comment: "Дата обновления записи",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "ClientPay_Card_Datas",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "ClientPay_Card_Datas",
                type: "int",
                nullable: false,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CardLastNum",
                table: "ClientPay_Card_Datas",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Последние 4 номера карты",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CardIdent",
                table: "ClientPay_Card_Datas",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Идентификатор карты, полученный от платежного сервиса",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Datas",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<bool>(
                name: "Push_Need",
                table: "Client_Notifications",
                type: "bit",
                nullable: false,
                comment: "Пуш-уведомления: Флаг того, что нужно отправлять пуш",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Push_DateSend",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: true,
                comment: "Пуш-уведомления: Дата отправки",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Client_Notifications",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование нотификации",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notifications",
                type: "bit",
                nullable: false,
                comment: "Флаг помеки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Read",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: true,
                comment: "Дата прочтения нотификации",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: false,
                comment: "Дата генерации нотификации",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Client_Notifications",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Содержимое нотификации",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notifications",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "TokenTypeID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                comment: "Идентификатор системы облачных уведомлений",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Client_Notification_Tokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "Токен",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notification_Tokens",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceID",
                table: "Client_Notification_Tokens",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Идентификатор устройства",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Client_Notification_Tokens",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                comment: "Код клиента",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<bool>(
                name: "Push_Need",
                table: "Client_Notification_Shablons",
                type: "bit",
                nullable: false,
                comment: "Пуш-уведомления: Флаг того, что нужно отправлять пуш",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Push_DateSend",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: true,
                comment: "Пуш-уведомления: Дата отправки",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Client_Notification_Shablons",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Наименование нотификации",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notification_Shablons",
                type: "bit",
                nullable: false,
                comment: "Флаг помеки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Read",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: true,
                comment: "Дата прочтения нотификации",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: false,
                comment: "Дата генерации нотификации",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Client_Notification_Shablons",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Содержимое нотификации",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notification_Shablons",
                type: "int",
                nullable: false,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notification_Shablons",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Bookings",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Restaurant_TableID",
                table: "Bookings",
                type: "int",
                nullable: false,
                comment: "Стол - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Телефон",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Имя",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Bookings",
                type: "bit",
                nullable: false,
                comment: "Флаг пометки на удаление",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Start",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                comment: "Дата начала",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_End",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                comment: "Дата окончания",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Bookings",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Action",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                comment: "Дата добавления записи",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Bookings",
                type: "int",
                nullable: true,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Bookings",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                comment: "Заказ - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Action",
                table: "Bonuses",
                type: "datetime2",
                nullable: false,
                comment: "Дата добавления записи",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                comment: "Клиент - Код",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses",
                table: "Bonuses",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                comment: "Количество бонусов",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Order_Deliveries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false, comment: "Заказ - Код"),
                    Delivery_Address = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Адрес доставки"),
                    Delivery_Apartment = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Квартира"),
                    Delivery_EntranceCode = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Код подъезда"),
                    Delivery_EntranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Домофон"),
                    Delivery_Floor = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Номер этажа"),
                    Delivery_Comment = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Комментарий к доставке"),
                    Delivery_Handing = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true, comment: "Дать сдачи с ..."),
                    Delivery_To = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Предзаказ доставки(Доставить к ...)"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Deliveries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Deliveries_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Доставка");

            migrationBuilder.CreateTable(
                name: "Order_Types",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Types", x => x.ID);
                },
                comment: "Типы заказа");

            migrationBuilder.CreateTable(
                name: "Restaurant_Halls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false, comment: "Код ресторана"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Halls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Halls_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Залы в ресторане");

            migrationBuilder.CreateTable(
                name: "Restaurant_Table_Types",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Table_Types", x => x.ID);
                },
                comment: "Типы столика");

            migrationBuilder.InsertData(
                table: "Order_Types",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "С собой" },
                    { 6, "Поесть в кафе" },
                    { 9, "Доставка" }
                });

            migrationBuilder.InsertData(
                table: "Pay_Types",
                columns: new[] { "ID", "Name" },
                values: new object[] { 9, "Оплата через терминал" });

            migrationBuilder.InsertData(
                table: "Restaurant_Table_Types",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "Обычный" },
                    { 6, "VIP" },
                    { 9, "Веранда" }
                });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Delivery_Price", "Delivery_Time", "Express_Delivery_Time", "Flag_Car", "Flag_Delivery", "Flag_Incafe", "Flag_Myself", "Hours_HardWork" },
                values: new object[] { null, null, null, false, false, false, false, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "RestaurantID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RestaurantID",
                table: "Users",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Tables_Restaurant_Table_TypeID",
                table: "Restaurant_Tables",
                column: "Restaurant_Table_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Deliveries_OrderID",
                table: "Order_Deliveries",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Halls_RestaurantID",
                table: "Restaurant_Halls",
                column: "RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Table_Types_Restaurant_Table_TypeID",
                table: "Restaurant_Tables",
                column: "Restaurant_Table_TypeID",
                principalTable: "Restaurant_Table_Types",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_RestaurantID",
                table: "Users",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Table_Types_Restaurant_Table_TypeID",
                table: "Restaurant_Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_RestaurantID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Order_Deliveries");

            migrationBuilder.DropTable(
                name: "Order_Types");

            migrationBuilder.DropTable(
                name: "Restaurant_Halls");

            migrationBuilder.DropTable(
                name: "Restaurant_Table_Types");

            migrationBuilder.DropIndex(
                name: "IX_Users_RestaurantID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_Tables_Restaurant_Table_TypeID",
                table: "Restaurant_Tables");

            migrationBuilder.DeleteData(
                table: "Pay_Types",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Delivery_Price",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Delivery_Time",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Express_Delivery_Time",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Flag_Car",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Flag_Delivery",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Flag_Incafe",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Flag_Myself",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Hours_HardWork",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Restaurant_Table_TypeID",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "BookingID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Delivery_Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Handing",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Order_TypeID",
                table: "Orders");

            migrationBuilder.AlterTable(
                name: "Volumes",
                oldComment: "Объёмы");

            migrationBuilder.AlterTable(
                name: "Users",
                oldComment: "Пользователи");

            migrationBuilder.AlterTable(
                name: "Sms_Logs",
                oldComment: "Логи отправки СМС");

            migrationBuilder.AlterTable(
                name: "Setups",
                oldComment: "Установки");

            migrationBuilder.AlterTable(
                name: "Restaurants",
                oldComment: "Рестораны");

            migrationBuilder.AlterTable(
                name: "Restaurant_Tables",
                oldComment: "Столы в ресторане");

            migrationBuilder.AlterTable(
                name: "Pay_Types",
                oldComment: "Виды оплаты");

            migrationBuilder.AlterTable(
                name: "Orders",
                oldComment: "Заказы");

            migrationBuilder.AlterTable(
                name: "Order_Statuses",
                oldComment: "Статусы заказа");

            migrationBuilder.AlterTable(
                name: "Order_Favorites_Data_Ingridients",
                oldComment: "Детали содержимого избранного заказа - ингредиенты");

            migrationBuilder.AlterTable(
                name: "Order_Favorites",
                oldComment: "Избранные заказы");

            migrationBuilder.AlterTable(
                name: "Order_Favorite_Datas",
                oldComment: "Содержимое избранного заказа");

            migrationBuilder.AlterTable(
                name: "Order_Datas",
                oldComment: "Содержимое заказа");

            migrationBuilder.AlterTable(
                name: "Order_Data_Ingridients",
                oldComment: "Детали содержимого заказа - ингредиенты");

            migrationBuilder.AlterTable(
                name: "Ingridients",
                oldComment: "Ингредиенты для блюда");

            migrationBuilder.AlterTable(
                name: "Dishes",
                oldComment: "Блюда>");

            migrationBuilder.AlterTable(
                name: "Dish_Volumes",
                oldComment: "Объёмы блюда");

            migrationBuilder.AlterTable(
                name: "Dish_Ingridients",
                oldComment: "Ингридиенты блюд");

            migrationBuilder.AlterTable(
                name: "Dish_Colors",
                oldComment: "Цвета блюда");

            migrationBuilder.AlterTable(
                name: "Dish_Categories",
                oldComment: "Категории блюд");

            migrationBuilder.AlterTable(
                name: "Colors",
                oldComment: "Цвет стакана");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Pays",
                oldComment: "Данные о платежах");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Pay_Type",
                oldComment: "Типы платежа");

            migrationBuilder.AlterTable(
                name: "ClientPay_Card_Datas",
                oldComment: "Данные карты клиента");

            migrationBuilder.AlterTable(
                name: "Client_Notifications",
                oldComment: "Уведомления клиента");

            migrationBuilder.AlterTable(
                name: "Client_Notification_Shablons",
                oldComment: "Шаблоны уведомлений");

            migrationBuilder.AlterTable(
                name: "Bookings",
                oldComment: "Бронирование");

            migrationBuilder.AlterTable(
                name: "Bonuses",
                oldComment: "Бонусы клиента");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Volumes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Значение");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Volumes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Volumes",
                type: "nvarchar(max)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Значение");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Volumes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Volumes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Volumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Пароль");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "ФИО пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Логин");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Root",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг суперадмин");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Oper",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг оператор");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Admin",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг админ");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Sms_Logs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Текст, который был отправлен в смс");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Sms_Logs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Телефон на который отправили СМС");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Sms_Logs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата отправки");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Sms_Logs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент, которому отправлено СМС - код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Sms_Logs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Val4",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи 4");

            migrationBuilder.AlterColumn<string>(
                name: "Val3",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи 3");

            migrationBuilder.AlterColumn<string>(
                name: "Val2",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи 2");

            migrationBuilder.AlterColumn<string>(
                name: "Val",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Значение записи 1");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Setups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Ключ");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Restaurants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Долгота");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Широта");

            migrationBuilder.AlterColumn<string>(
                name: "Hours_WorkApp",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Часы работы ресторана для заказа в приложении через ;");

            migrationBuilder.AlterColumn<string>(
                name: "Hours_Work",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Часы работы ресторана через ;");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Restaurants",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Адрес");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Restaurants",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Restaurant_Tables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ресторан - код");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование стола");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsAvailableApp",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг доступности к заказу через приложение");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsAvailable",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг доступности к заказу через официанта");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Restaurant_Tables",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Restaurant_Tables",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Coordinates",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Координаты стола для админки");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pay_Types",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Pay_Types",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ресторан, куда совершается заказ - Код");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Телефон, на кого заказ");

            migrationBuilder.AlterColumn<int>(
                name: "Pay_TypeID",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Вид оплаты - Код");

            migrationBuilder.AlterColumn<int>(
                name: "Order_StatusID",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Статус заказа - код");

            migrationBuilder.AlterColumn<int>(
                name: "Order_FavoriteID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Код избранного заказа");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Имя, на кого заказ");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Payed",
                table: "Orders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что заказ оплачен");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Inhouse",
                table: "Orders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что клиент будет кушать в кафе, если false - то на вынос");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Orders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_To",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата и время к которому приготовить заказ");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastOrderStatusChange",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата последнего изменения статуса заказа");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата заказа");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Комментарий к заказу");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_PayID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Операция по карте, прошедшая по этой подписке последней - код");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Order_Statuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Порядок следования");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Order_Statuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Statuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Order_Pay_Statuses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, установивший пометку на удаление - код");

            migrationBuilder.AlterColumn<int>(
                name: "Order_Favorite_DataID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ссылка на позицию заказа - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Summ",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Общая сумма ингредиента");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Price",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Цена ингредиента");

            migrationBuilder.AlterColumn<string>(
                name: "Ingridient_Name",
                table: "Order_Favorites_Data_Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование ингредиента в момент оформления");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Count",
                table: "Order_Favorites_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Количество игредиента");

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ингредиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Order_Favorites",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ресторан, куда совершается заказ - Код");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Order_Favorites",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Телефон, на кого заказ");

            migrationBuilder.AlterColumn<int>(
                name: "Pay_TypeID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Вид оплаты - Код");

            migrationBuilder.AlterColumn<int>(
                name: "Order_StatusID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Статус заказа - Код");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Order_Favorites",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Имя, на кого заказ");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Payed",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что заказ оплачен");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Inhouse",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что клиент будет кушать в кафе, если false - то на вынос");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Order_Favorites",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_To",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата и время к которому приготовить заказ");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastEdit",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата последнего изменения избранного заказа");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата добавления заказа в избранное");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderFavoriteID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Избранный заказ - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumeValue",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Объём");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumePrice",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Цена");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_VolumeID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Объем - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Summ",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Общая сумма позиции");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Price",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Стоимость позиции");

            migrationBuilder.AlterColumn<string>(
                name: "Dish_Name",
                table: "Order_Favorite_Datas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование товара в момент оформления");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Count",
                table: "Order_Favorite_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Количество");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_ColorID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Цвет - Код");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Блюдо - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Заказ - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumeValue",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Объём");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_VolumePrice",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Цена");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_VolumeID",
                table: "Order_Datas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Код объёма");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Summ",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Общая сумма позиции");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Price",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Стоимость позиции");

            migrationBuilder.AlterColumn<string>(
                name: "Dish_Name",
                table: "Order_Datas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование товара в момент оформления");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dish_Count",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Количество");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_ColorID",
                table: "Order_Datas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Цвет - Код");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Блюдо - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses_Plus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Количество начисленных бонусов");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses_Minus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Количество списанных бонусов");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Order_DataID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ссылка на позицию заказа - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Summ",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Общая сумма ингредиента");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Price",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Цена ингредиента");

            migrationBuilder.AlterColumn<string>(
                name: "Ingridient_Name",
                table: "Order_Data_Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование ингредиента в момент оформления");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ingridient_Count",
                table: "Order_Data_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Количество игредиента");

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ингредиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Order_Data_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Ingridients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingridients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование ингредиента");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Image_ChangeDate",
                table: "Ingridients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата изменения изображения");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Ingridients",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true,
                oldComment: "Изображение ингредиента");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Ingridients",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Ingridients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dishes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Полное описание блюда");

            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Краткое описание блюда");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Стоимость");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dishes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Порядковый номер");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование товара");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Image_ChangeDate",
                table: "Dishes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата изменения изображения");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Dishes",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true,
                oldComment: "Изображение блюда");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dishes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Bonus",
                table: "Dishes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что блюдо можно купить за бонусы");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_AllowColors",
                table: "Dishes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана");

            migrationBuilder.AlterColumn<int>(
                name: "Dish_CategoryID",
                table: "Dishes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Категория блюда - код");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dishes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dishes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Количество минут для приготовления блюда");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses",
                table: "Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Количество бонусов, начисляемых за покупку");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dishes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "VolumeID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Объем - Код");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Volumes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish_Volumes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Цена за этот обьем");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Порядковый номер");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Volumes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Блюдо - Код");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Volumes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Volumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Dish_Ingridients",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Стоимость");

            migrationBuilder.AlterColumn<int>(
                name: "IngridientID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Ингридиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "IngridientGroupID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Группа");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Блюдо - Код");

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Количество минут для приготовления");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Colors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Colors",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "DishID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Блюдо - Код");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Colors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Цвет ID");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Colors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Dish_Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNum",
                table: "Dish_Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Порядковый номер");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dish_Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование категории");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_AllowColors",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что в категории у товаров разрешены цвета");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Categories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Количество минут для приготовления блюд в данной категории");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Dish_Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Colors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "Name_Eng",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование цвета - англ");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование цвета");

            migrationBuilder.AlterColumn<string>(
                name: "HexColor",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Цвет стакана");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Colors",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Colors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Colors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Clients",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true,
                oldComment: "Аватар пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Телефон");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Имя пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Clients",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Guid");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_IsPolicy",
                table: "Clients",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Политика принята");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Clients",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Registration",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата регистрации");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_PhotoUpdate",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата обновления фотографии");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastZakaz",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата последнего заказа");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastUpdate",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата последнего изменения информации клиента");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_LastAction",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата последнего входа в приложение");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Birth",
                table: "Clients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата рождения");

            migrationBuilder.AlterColumn<string>(
                name: "Code_Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Смс код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Summ",
                table: "ClientPay_Card_Pays",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Сумма платежа");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentIdent",
                table: "ClientPay_Card_Pays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Идентификатор платежа, генерируемый платежной системой");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Заказ - Код");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Complete",
                table: "ClientPay_Card_Pays",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг того, что операция завершена успешно, если он не стоит - то нужно повторять попытки на API");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Create",
                table: "ClientPay_Card_Pays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата создания платежа");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Complete",
                table: "ClientPay_Card_Pays",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата удаления");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_Pay_TypeID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...) - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPay_Card_DataID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Карта, которой совершался платеж - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Pays",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClientPay_Card_Pay_Type",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Pay_Type",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "ClientPay_Card_Datas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Update",
                table: "ClientPay_Card_Datas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата обновления записи");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "ClientPay_Card_Datas",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "ClientPay_Card_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<string>(
                name: "CardLastNum",
                table: "ClientPay_Card_Datas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Последние 4 номера карты");

            migrationBuilder.AlterColumn<string>(
                name: "CardIdent",
                table: "ClientPay_Card_Datas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Идентификатор карты, полученный от платежного сервиса");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ClientPay_Card_Datas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<bool>(
                name: "Push_Need",
                table: "Client_Notifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Пуш-уведомления: Флаг того, что нужно отправлять пуш");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Push_DateSend",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Пуш-уведомления: Дата отправки");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Client_Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование нотификации");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг помеки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Read",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата прочтения нотификации");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата генерации нотификации");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Client_Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Содержимое нотификации");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "TokenTypeID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Идентификатор системы облачных уведомлений");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Client_Notification_Tokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "Токен");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notification_Tokens",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceID",
                table: "Client_Notification_Tokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Идентификатор устройства");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Client_Notification_Tokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код клиента");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notification_Tokens",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<bool>(
                name: "Push_Need",
                table: "Client_Notification_Shablons",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Пуш-уведомления: Флаг того, что нужно отправлять пуш");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Push_DateSend",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Пуш-уведомления: Дата отправки");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Client_Notification_Shablons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Наименование нотификации");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Client_Notification_Shablons",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг помеки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Read",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата прочтения нотификации");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Act",
                table: "Client_Notification_Shablons",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата генерации нотификации");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Client_Notification_Shablons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Содержимое нотификации");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notification_Shablons",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Client_Notification_Shablons",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AlterColumn<int>(
                name: "UserID_Del",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AlterColumn<int>(
                name: "Restaurant_TableID",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Стол - Код");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Телефон");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Имя");

            migrationBuilder.AlterColumn<bool>(
                name: "Flag_Del",
                table: "Bookings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Флаг пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Start",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата начала");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_End",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата окончания");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Del",
                table: "Bookings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "Дата пометки на удаление");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Action",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата добавления записи");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Заказ - Код");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Action",
                table: "Bonuses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата добавления записи");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Клиент - Код");

            migrationBuilder.AlterColumn<decimal>(
                name: "Bonuses",
                table: "Bonuses",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldComment: "Количество бонусов");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Bonuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
