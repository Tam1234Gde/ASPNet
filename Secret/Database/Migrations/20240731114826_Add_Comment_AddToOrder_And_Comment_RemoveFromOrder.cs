using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Comment_AddToOrder_And_Comment_RemoveFromOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TgChatID",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Telegram - Код чата");

            migrationBuilder.AddColumn<string>(
                name: "Comment_AddToOrder",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Добавить к заказу");

            migrationBuilder.AddColumn<string>(
                name: "Comment_RemoveFromOrder",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Убрать из заказа");

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "Order_Statuses",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Цвет");

            migrationBuilder.AlterColumn<string>(
                name: "Delivery_EntranceNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Номер подъезда",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Домофон");

            migrationBuilder.AlterColumn<string>(
                name: "Delivery_EntranceCode",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Домофон",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Код подъезда");

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 3,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 5,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 7,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 9,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 11,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 13,
                column: "ColorHex",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                column: "TgChatID",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TgChatID",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Comment_AddToOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Comment_RemoveFromOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "Order_Statuses");

            migrationBuilder.AlterColumn<string>(
                name: "Delivery_EntranceNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Домофон",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Номер подъезда");

            migrationBuilder.AlterColumn<string>(
                name: "Delivery_EntranceCode",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Код подъезда",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Домофон");
        }
    }
}
