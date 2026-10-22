using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Restaurant_Halls");

            migrationBuilder.AddColumn<decimal>(
                name: "Coordinates_Rotation",
                table: "Restaurant_Tables",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Ротация стола для приложения");

            migrationBuilder.AddColumn<decimal>(
                name: "Coordinates_X",
                table: "Restaurant_Tables",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Координаты X стола для приложения");

            migrationBuilder.AddColumn<decimal>(
                name: "Coordinates_Y",
                table: "Restaurant_Tables",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Координаты Y стола для приложения");

            migrationBuilder.AddColumn<string>(
                name: "Dop_Text",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Ротация стола для приложения");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Restaurant_Tables",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Стоимость бронирования");

            migrationBuilder.AddColumn<int>(
                name: "Restaurant_Table_StatusID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: true,
                comment: "Статус столика - код");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Active",
                table: "Restaurant_Halls",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Активен");

            migrationBuilder.AddColumn<byte[]>(
                name: "Restaurant_Image",
                table: "Restaurant_Halls",
                type: "varbinary(max)",
                nullable: true,
                comment: "Планировка");

            migrationBuilder.AddColumn<int>(
                name: "Restaurant_Table_TypeID",
                table: "Restaurant_Halls",
                type: "int",
                nullable: true,
                comment: "Тип столика - код");

            migrationBuilder.AddColumn<decimal>(
                name: "Booking_Price",
                table: "Orders",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Стоимость брони");

            migrationBuilder.AddColumn<int>(
                name: "ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas",
                type: "int",
                nullable: true,
                comment: "Тип карты - код");

            migrationBuilder.CreateTable(
                name: "ClientPay_Card_Types",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPay_Card_Types", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false, comment: "Клиент - Код"),
                    Grade = table.Column<int>(type: "int", nullable: false, comment: "Оценка"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Текст"),
                    Date_Action = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата оценки"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ratings_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Оценка приложения");

            migrationBuilder.CreateTable(
                name: "Restaurant_Table_Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    RestaurantTable_Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Изображение стола")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Table_Statuses", x => x.ID);
                },
                comment: "Статусы столика");

            migrationBuilder.InsertData(
                table: "Restaurant_Table_Statuses",
                columns: new[] { "ID", "Name", "RestaurantTable_Image" },
                values: new object[,]
                {
                    { 3, "Не забронирован", null },
                    { 6, "Забронирован", null },
                    { 9, "Недоступен", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Tables_Restaurant_Table_StatusID",
                table: "Restaurant_Tables",
                column: "Restaurant_Table_StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Halls_Restaurant_Table_TypeID",
                table: "Restaurant_Halls",
                column: "Restaurant_Table_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookingID",
                table: "Orders",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPay_Card_Datas_ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas",
                column: "ClientPay_Card_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ClientID",
                table: "Ratings",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPay_Card_Datas_ClientPay_Card_Types_ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas",
                column: "ClientPay_Card_TypeID",
                principalTable: "ClientPay_Card_Types",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bookings_BookingID",
                table: "Orders",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Halls_Restaurant_Table_Types_Restaurant_Table_TypeID",
                table: "Restaurant_Halls",
                column: "Restaurant_Table_TypeID",
                principalTable: "Restaurant_Table_Types",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Table_Statuses_Restaurant_Table_StatusID",
                table: "Restaurant_Tables",
                column: "Restaurant_Table_StatusID",
                principalTable: "Restaurant_Table_Statuses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPay_Card_Datas_ClientPay_Card_Types_ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bookings_BookingID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Halls_Restaurant_Table_Types_Restaurant_Table_TypeID",
                table: "Restaurant_Halls");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Table_Statuses_Restaurant_Table_StatusID",
                table: "Restaurant_Tables");

            migrationBuilder.DropTable(
                name: "ClientPay_Card_Types");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Restaurant_Table_Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_Tables_Restaurant_Table_StatusID",
                table: "Restaurant_Tables");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_Halls_Restaurant_Table_TypeID",
                table: "Restaurant_Halls");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BookingID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_ClientPay_Card_Datas_ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas");

            migrationBuilder.DropColumn(
                name: "Coordinates_Rotation",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Coordinates_X",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Coordinates_Y",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Dop_Text",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Restaurant_Table_StatusID",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Flag_Active",
                table: "Restaurant_Halls");

            migrationBuilder.DropColumn(
                name: "Restaurant_Image",
                table: "Restaurant_Halls");

            migrationBuilder.DropColumn(
                name: "Restaurant_Table_TypeID",
                table: "Restaurant_Halls");

            migrationBuilder.DropColumn(
                name: "Booking_Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClientPay_Card_TypeID",
                table: "ClientPay_Card_Datas");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Restaurant_Halls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Название");
        }
    }
}
