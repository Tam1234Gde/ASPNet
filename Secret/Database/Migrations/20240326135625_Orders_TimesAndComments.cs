using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Orders_TimesAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_LastOrderStatusChange",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Order_Statuses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_LastEdit",
                table: "Order_Favorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 3,
                column: "OrderNum",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 5,
                column: "OrderNum",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 7,
                column: "OrderNum",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 9,
                column: "OrderNum",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 11,
                column: "OrderNum",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Order_Statuses",
                keyColumn: "ID",
                keyValue: 13,
                column: "OrderNum",
                value: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Date_LastOrderStatusChange",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Order_Statuses");

            migrationBuilder.DropColumn(
                name: "Date_LastEdit",
                table: "Order_Favorites");
        }
    }
}
