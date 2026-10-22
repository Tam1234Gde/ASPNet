using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Restaurant_PayTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YouKassaLogin",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Юкасса > Логин");

            migrationBuilder.AddColumn<string>(
                name: "YouKassaPassword",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Юкасса > Пароль");

            migrationBuilder.CreateTable(
                name: "Restaurant_PayTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false, comment: "Ресторан - код"),
                    Pay_TypeID = table.Column<int>(type: "int", nullable: false, comment: "Вид оплаты (картой, нал, ...) - Код"),
                    Order_TypeID = table.Column<int>(type: "int", nullable: false, comment: "Тип заказа(С собой\\Поесть в кафе\\Доставка\\К машине) - Код"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_PayTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_PayTypes_Order_Types_Order_TypeID",
                        column: x => x.Order_TypeID,
                        principalTable: "Order_Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_PayTypes_Pay_Types_Pay_TypeID",
                        column: x => x.Pay_TypeID,
                        principalTable: "Pay_Types",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_PayTypes_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Доступные типы оплаты в ресторане");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "YouKassaLogin", "YouKassaPassword" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_PayTypes_Order_TypeID",
                table: "Restaurant_PayTypes",
                column: "Order_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_PayTypes_Pay_TypeID",
                table: "Restaurant_PayTypes",
                column: "Pay_TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_PayTypes_RestaurantID",
                table: "Restaurant_PayTypes",
                column: "RestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restaurant_PayTypes");

            migrationBuilder.DropColumn(
                name: "YouKassaLogin",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "YouKassaPassword",
                table: "Restaurants");
        }
    }
}
