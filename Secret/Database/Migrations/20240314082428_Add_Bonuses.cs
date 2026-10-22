using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Bonuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonuses",
                table: "Clients");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_Del",
                table: "Order_Favorites_Data_Ingridients",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Del",
                table: "Order_Favorites_Data_Ingridients",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Флаг пометки на удаление");

            migrationBuilder.AddColumn<int>(
                name: "UserID_Del",
                table: "Order_Favorites_Data_Ingridients",
                type: "int",
                nullable: true,
                comment: "Пользователь, установивший пометку на удаление");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_Del",
                table: "Order_Favorite_Datas",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Del",
                table: "Order_Favorite_Datas",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Флаг пометки на удаление");

            migrationBuilder.AddColumn<int>(
                name: "UserID_Del",
                table: "Order_Favorite_Datas",
                type: "int",
                nullable: true,
                comment: "Пользователь, установивший пометку на удаление");

            migrationBuilder.AddColumn<decimal>(
                name: "Bonuses_Minus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Bonuses_Plus",
                table: "Order_Datas",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Bonuses",
                table: "Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Bonus",
                table: "Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Teaser",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngridientGroupID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Flag_IsPolicy",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceID",
                table: "Client_Notification_Tokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Bonuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Bonuses = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Date_Action = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bonuses_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Notification_Tokens_DeviceID",
                table: "Client_Notification_Tokens",
                column: "DeviceID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bonuses_ClientID",
                table: "Bonuses",
                column: "ClientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonuses");

            migrationBuilder.DropIndex(
                name: "IX_Client_Notification_Tokens_DeviceID",
                table: "Client_Notification_Tokens");

            migrationBuilder.DropColumn(
                name: "Date_Del",
                table: "Order_Favorites_Data_Ingridients");

            migrationBuilder.DropColumn(
                name: "Flag_Del",
                table: "Order_Favorites_Data_Ingridients");

            migrationBuilder.DropColumn(
                name: "UserID_Del",
                table: "Order_Favorites_Data_Ingridients");

            migrationBuilder.DropColumn(
                name: "Date_Del",
                table: "Order_Favorite_Datas");

            migrationBuilder.DropColumn(
                name: "Flag_Del",
                table: "Order_Favorite_Datas");

            migrationBuilder.DropColumn(
                name: "UserID_Del",
                table: "Order_Favorite_Datas");

            migrationBuilder.DropColumn(
                name: "Bonuses_Minus",
                table: "Order_Datas");

            migrationBuilder.DropColumn(
                name: "Bonuses_Plus",
                table: "Order_Datas");

            migrationBuilder.DropColumn(
                name: "Bonuses",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Flag_Bonus",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Teaser",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "IngridientGroupID",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Flag_IsPolicy",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Bonuses",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceID",
                table: "Client_Notification_Tokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

        }
    }
}
