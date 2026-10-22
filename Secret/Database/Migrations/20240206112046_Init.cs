using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client_Notification_Shablons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Date_Act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_Read = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Push_Need = table.Column<bool>(type: "bit", nullable: false),
                    Push_DateSend = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Notification_Shablons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Client_Notification_Tokens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    TokenTypeID = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Notification_Tokens", x => x.ID);
                },
                comment: "Токены уведомлений клиентов");

            migrationBuilder.CreateTable(
                name: "ClientPay_Card_Pay_Type",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPay_Card_Pay_Type", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code_Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Date_PhotoUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bonuses = table.Column<int>(type: "int", nullable: false),
                    Date_Registration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date_LastAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_LastZakaz = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date_LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                },
                comment: "Клиенты системы");

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HexColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Dish_Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNum = table.Column<int>(type: "int", nullable: false),
                    Flag_AllowColors = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image_ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingridients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Order_Pay_Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Наименование"),
                    Comment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "Комментарий"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Pay_Statuses", x => x.ID);
                },
                comment: "Статусы оплаты");

            migrationBuilder.CreateTable(
                name: "Order_Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Statuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pay_Types",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pay_Types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Setups",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Val = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setups", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Flag_Admin = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Volumes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", precision: 8, scale: 2, nullable: true),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volumes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Client_Notifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Date_Act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_Read = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Push_Need = table.Column<bool>(type: "bit", nullable: false),
                    Push_DateSend = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Notifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_Notifications_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPay_Card_Datas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    CardIdent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardLastNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPay_Card_Datas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClientPay_Card_Datas_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sms_Logs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sms_Logs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sms_Logs_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dish_CategoryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Flag_AllowColors = table.Column<bool>(type: "bit", nullable: false),
                    OrderNum = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Image_ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dishes_Dish_Categories_Dish_CategoryID",
                        column: x => x.Dish_CategoryID,
                        principalTable: "Dish_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Favorites",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_StatusID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pay_TypeID = table.Column<int>(type: "int", nullable: false),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Date_To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Inhouse = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Payed = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Favorites", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Order_Statuses_Order_StatusID",
                        column: x => x.Order_StatusID,
                        principalTable: "Order_Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Pay_Types_Pay_TypeID",
                        column: x => x.Pay_TypeID,
                        principalTable: "Pay_Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPay_Card_Pays",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    ClientPay_Card_Pay_TypeID = table.Column<int>(type: "int", nullable: false),
                    ClientPay_Card_DataID = table.Column<int>(type: "int", nullable: false),
                    PaymentIdent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summ = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    Flag_Complete = table.Column<bool>(type: "bit", nullable: false),
                    Date_Complete = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPay_Card_Pays", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClientPay_Card_Pays_ClientPay_Card_Datas_ClientPay_Card_DataID",
                        column: x => x.ClientPay_Card_DataID,
                        principalTable: "ClientPay_Card_Datas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPay_Card_Pays_ClientPay_Card_Pay_Type_ClientPay_Card_Pay_TypeID",
                        column: x => x.ClientPay_Card_Pay_TypeID,
                        principalTable: "ClientPay_Card_Pay_Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPay_Card_Pays_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Dish_Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<int>(type: "int", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish_Colors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dish_Colors_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dish_Colors_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dish_Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    IngridientID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish_Ingridients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dish_Ingridients_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dish_Ingridients_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dish_Volumes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    VolumeID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    OrderNum = table.Column<int>(type: "int", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish_Volumes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dish_Volumes_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dish_Volumes_Volumes_VolumeID",
                        column: x => x.VolumeID,
                        principalTable: "Volumes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_FavoriteID = table.Column<int>(type: "int", nullable: true),
                    Date_Act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_StatusID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pay_TypeID = table.Column<int>(type: "int", nullable: false),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Date_To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag_Inhouse = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Payed = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true),
                    ClientPay_Card_PayID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_ClientPay_Card_Pays_ClientPay_Card_PayID",
                        column: x => x.ClientPay_Card_PayID,
                        principalTable: "ClientPay_Card_Pays",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Orders_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Order_Statuses_Order_StatusID",
                        column: x => x.Order_StatusID,
                        principalTable: "Order_Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Pay_Types_Pay_TypeID",
                        column: x => x.Pay_TypeID,
                        principalTable: "Pay_Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Favorite_Datas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderFavoriteID = table.Column<int>(type: "int", nullable: false),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    Dish_ColorID = table.Column<int>(type: "int", nullable: true),
                    Dish_Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Count = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Summ = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dish_VolumeID = table.Column<int>(type: "int", nullable: true),
                    Dish_VolumeValue = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Dish_VolumePrice = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Favorite_Datas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Favorite_Datas_Dish_Colors_Dish_ColorID",
                        column: x => x.Dish_ColorID,
                        principalTable: "Dish_Colors",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Order_Favorite_Datas_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Favorite_Datas_Order_Favorites_OrderFavoriteID",
                        column: x => x.OrderFavoriteID,
                        principalTable: "Order_Favorites",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Datas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    Dish_ColorID = table.Column<int>(type: "int", nullable: true),
                    Dish_Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Count = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Summ = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Dish_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dish_VolumeID = table.Column<int>(type: "int", nullable: true),
                    Dish_VolumeValue = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Dish_VolumePrice = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Datas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Datas_Dish_Colors_Dish_ColorID",
                        column: x => x.Dish_ColorID,
                        principalTable: "Dish_Colors",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Order_Datas_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Datas_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Favorites_Data_Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Favorite_DataID = table.Column<int>(type: "int", nullable: false),
                    IngridientID = table.Column<int>(type: "int", nullable: false),
                    Ingridient_Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Count = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Summ = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Favorites_Data_Ingridients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Data_Ingridients_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Favorites_Data_Ingridients_Order_Favorite_Datas_Order_Favorite_DataID",
                        column: x => x.Order_Favorite_DataID,
                        principalTable: "Order_Favorite_Datas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Data_Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_DataID = table.Column<int>(type: "int", nullable: false),
                    IngridientID = table.Column<int>(type: "int", nullable: false),
                    Ingridient_Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Count = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Summ = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Ingridient_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Data_Ingridients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Data_Ingridients_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Data_Ingridients_Order_Datas_Order_DataID",
                        column: x => x.Order_DataID,
                        principalTable: "Order_Datas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClientPay_Card_Pay_Type",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Инициализационный платеж (при привязке карты)" },
                    { 3, "Возврат инициализационного платежа (при привязке карты)" },
                    { 7, "Платеж" },
                    { 9, "Возврат платежа" },
                    { 13, "Рекурентный платеж" },
                    { 15, "Возврат рекурентного платежа" }
                });

            migrationBuilder.InsertData(
                table: "Client_Notification_Shablons",
                columns: new[] { "ID", "ClientID", "Content", "Date_Act", "Date_Del", "Date_Read", "Flag_Del", "Name", "Push_DateSend", "Push_Need" },
                values: new object[,]
                {
                    { 3, 0, "Ваш заказ под номером ((###HIDEERV1###)) оформлен.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Новый заказ!", null, false },
                    { 5, 0, "Ваш заказ под номером ((###HIDEERV1###)) отменен.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Заказ отменен!", null, false },
                    { 7, 0, "Ваш заказ под номером ((###HIDEERV1###)) готовится.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Заказ готовится!", null, false },
                    { 9, 0, "Ваш заказ под номером ((###HIDEERV1###)) готов к выдаче. Ждем вас в кофейне по адресу: ((###HIDEERV2###)).", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Заказ готов!", null, false },
                    { 11, 0, "Ваш заказ под номером ((###HIDEERV1###)) передан в доставку.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Заказ передан в доставку", null, false },
                    { 13, 0, "Ваш заказ под номером ((###HIDEERV1###)) завершен.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, "Заказ завершен!", null, false }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ID", "Bonuses", "Code_Phone", "Date_Birth", "Date_Del", "Date_LastAction", "Date_LastUpdate", "Date_LastZakaz", "Date_PhotoUpdate", "Date_Registration", "Flag_Del", "Guid", "Name", "Phone", "Photo", "UserID_Del" },
                values: new object[,]
                {
                    { 1, 0, "1111", null, null, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, "395df047-314d-4111-8532-be3bcbba6e99", "Тест HIDEerv", "79282000000", null, null },
                    { 2, 0, "1111", null, null, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, "395df047-314d-4111-8532-be3bcbba6e98", "Тест HIDEerv", "79282000001", null, null }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "ID", "Date_Del", "Flag_Del", "HexColor", "Name", "Name_Eng", "UserID_Del" },
                values: new object[,]
                {
                    { 1, null, false, "FF0000", "Красный", "Red", null },
                    { 2, null, false, "00FF00", "Зеленый", "Green", null },
                    { 3, null, false, "0000FF", "Синий", "Blue", null }
                });

            migrationBuilder.InsertData(
                table: "Dish_Categories",
                columns: new[] { "ID", "Date_Del", "Flag_AllowColors", "Flag_Del", "Name", "OrderNum", "UserID_Del" },
                values: new object[,]
                {
                    { 1, null, true, false, "Кофе", 0, null },
                    { 2, null, false, false, "Пироженные", 0, null },
                    { 3, null, false, false, "Мороженное", 0, null },
                    { 4, null, false, false, "Каша", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Ingridients",
                columns: new[] { "ID", "Date_Del", "Flag_Del", "Image", "Image_ChangeDate", "Name", "UserID_Del" },
                values: new object[,]
                {
                    { 1, null, false, null, null, "Клубника", null },
                    { 2, null, false, null, null, "Смородина", null }
                });

            migrationBuilder.InsertData(
                table: "Order_Pay_Statuses",
                columns: new[] { "ID", "Comment", "Date_Del", "Flag_Del", "Name", "UserID_Del" },
                values: new object[,]
                {
                    { 1, null, null, false, "Ожидает оплаты", null },
                    { 3, null, null, false, "Оплачен", null },
                    { 5, null, null, false, "Попытка повторной оплаты", null },
                    { 7, null, null, false, "Ошибка при оплате", null },
                    { 9, null, null, false, "Заказ отменен", null }
                });

            migrationBuilder.InsertData(
                table: "Order_Statuses",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "Новый заказ" },
                    { 5, "Заказ отменен" },
                    { 7, "Заказ готовится" },
                    { 9, "Заказ готов к выдаче" },
                    { 11, "Заказ передан в доставку" },
                    { 13, "Заказ завершен" }
                });

            migrationBuilder.InsertData(
                table: "Pay_Types",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 3, "Наличные" },
                    { 6, "Карта в приложении" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "ID", "Adress", "Date_Del", "Flag_Del", "Latitude", "Longitude", "UserID_Del" },
                values: new object[] { 1, "Грозный, проспект Путина, 34", null, false, "43.318333", "45.690265", null });

            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "Key", "Val" },
                values: new object[] { "Dishes_DateChange", "25.12.2023 0:00:00" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Date_Del", "Flag_Admin", "Flag_Del", "Login", "Name", "Password", "UserID_Del" },
                values: new object[] { 1, null, true, false, "support@HIDEerv.ru", "HIDEerv", "12345!", null });

            migrationBuilder.InsertData(
                table: "Volumes",
                columns: new[] { "ID", "Date_Del", "Flag_Del", "Name", "UserID_Del", "Value" },
                values: new object[,]
                {
                    { 1, null, false, "", null, 250m },
                    { 2, null, false, "", null, 350m },
                    { 3, null, false, "", null, 450m }
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "ID", "Date_Del", "Dish_CategoryID", "Flag_AllowColors", "Flag_Del", "Image", "Image_ChangeDate", "Name", "OrderNum", "Price", "UserID_Del" },
                values: new object[,]
                {
                    { 1, null, 1, true, false, null, null, "Кофе Первое", 0, 0m, null },
                    { 2, null, 1, true, false, null, null, "Кофе второе", 0, 0m, null },
                    { 3, null, 1, true, false, null, null, "Кофе третье", 0, 0m, null },
                    { 4, null, 2, false, false, null, null, "Пироженное первое", 0, 0m, null },
                    { 5, null, 2, false, false, null, null, "Пироженное второе", 0, 0m, null },
                    { 6, null, 2, false, false, null, null, "Пироженное третье", 0, 0m, null },
                    { 7, null, 3, false, false, null, null, "Мороженное первое", 0, 0m, null },
                    { 8, null, 3, false, false, null, null, "Мороженное второе", 0, 0m, null },
                    { 9, null, 3, false, false, null, null, "Мороженное третье", 0, 0m, null },
                    { 10, null, 4, false, false, null, null, "Каша без ингридиентов", 0, 0m, null },
                    { 11, null, 4, false, false, null, null, "Каша с ингридиентами", 0, 0m, null }
                });

            migrationBuilder.InsertData(
                table: "Dish_Ingridients",
                columns: new[] { "ID", "DishID", "IngridientID", "Price" },
                values: new object[,]
                {
                    { 1, 11, 1, 100.0m },
                    { 2, 11, 2, 1200.0m }
                });

            migrationBuilder.InsertData(
                table: "Dish_Volumes",
                columns: new[] { "ID", "Date_Del", "DishID", "Flag_Del", "OrderNum", "Price", "UserID_Del", "VolumeID" },
                values: new object[] { 1, null, 1, false, 0, 100.0m, null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Notifications_ClientID",
                table: "Client_Notifications",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPay_Card_Datas_ClientID_CardIdent",
                table: "ClientPay_Card_Datas",
                columns: new[] { "ClientID", "CardIdent" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPay_Card_Pays_ClientID",
                table: "ClientPay_Card_Pays",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPay_Card_Pays_ClientPay_Card_DataID",
                table: "ClientPay_Card_Pays",
                column: "ClientPay_Card_DataID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPay_Card_Pays_ClientPay_Card_Pay_TypeID",
                table: "ClientPay_Card_Pays",
                column: "ClientPay_Card_Pay_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Colors_ColorID",
                table: "Dish_Colors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Colors_DishID",
                table: "Dish_Colors",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Ingridients_DishID",
                table: "Dish_Ingridients",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Ingridients_IngridientID",
                table: "Dish_Ingridients",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Volumes_DishID",
                table: "Dish_Volumes",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Volumes_VolumeID",
                table: "Dish_Volumes",
                column: "VolumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_Dish_CategoryID",
                table: "Dishes",
                column: "Dish_CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Data_Ingridients_IngridientID",
                table: "Order_Data_Ingridients",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Data_Ingridients_Order_DataID",
                table: "Order_Data_Ingridients",
                column: "Order_DataID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Datas_Dish_ColorID",
                table: "Order_Datas",
                column: "Dish_ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Datas_DishID",
                table: "Order_Datas",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Datas_OrderID",
                table: "Order_Datas",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorite_Datas_Dish_ColorID",
                table: "Order_Favorite_Datas",
                column: "Dish_ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorite_Datas_DishID",
                table: "Order_Favorite_Datas",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorite_Datas_OrderFavoriteID",
                table: "Order_Favorite_Datas",
                column: "OrderFavoriteID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_ClientID",
                table: "Order_Favorites",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_Order_StatusID",
                table: "Order_Favorites",
                column: "Order_StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_Pay_TypeID",
                table: "Order_Favorites",
                column: "Pay_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_RestaurantID",
                table: "Order_Favorites",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_Data_Ingridients_IngridientID",
                table: "Order_Favorites_Data_Ingridients",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Favorites_Data_Ingridients_Order_Favorite_DataID",
                table: "Order_Favorites_Data_Ingridients",
                column: "Order_Favorite_DataID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientID",
                table: "Orders",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientPay_Card_PayID",
                table: "Orders",
                column: "ClientPay_Card_PayID",
                unique: true,
                filter: "[ClientPay_Card_PayID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Order_StatusID",
                table: "Orders",
                column: "Order_StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Pay_TypeID",
                table: "Orders",
                column: "Pay_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantID",
                table: "Orders",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Sms_Logs_ClientID",
                table: "Sms_Logs",
                column: "ClientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client_Notification_Shablons");

            migrationBuilder.DropTable(
                name: "Client_Notification_Tokens");

            migrationBuilder.DropTable(
                name: "Client_Notifications");

            migrationBuilder.DropTable(
                name: "Dish_Ingridients");

            migrationBuilder.DropTable(
                name: "Dish_Volumes");

            migrationBuilder.DropTable(
                name: "Order_Data_Ingridients");

            migrationBuilder.DropTable(
                name: "Order_Favorites_Data_Ingridients");

            migrationBuilder.DropTable(
                name: "Order_Pay_Statuses");

            migrationBuilder.DropTable(
                name: "Setups");

            migrationBuilder.DropTable(
                name: "Sms_Logs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Volumes");

            migrationBuilder.DropTable(
                name: "Order_Datas");

            migrationBuilder.DropTable(
                name: "Ingridients");

            migrationBuilder.DropTable(
                name: "Order_Favorite_Datas");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Dish_Colors");

            migrationBuilder.DropTable(
                name: "Order_Favorites");

            migrationBuilder.DropTable(
                name: "ClientPay_Card_Pays");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Order_Statuses");

            migrationBuilder.DropTable(
                name: "Pay_Types");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "ClientPay_Card_Datas");

            migrationBuilder.DropTable(
                name: "ClientPay_Card_Pay_Type");

            migrationBuilder.DropTable(
                name: "Dish_Categories");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
