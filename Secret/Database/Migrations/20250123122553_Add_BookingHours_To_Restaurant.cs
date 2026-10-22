using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_BookingHours_To_Restaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingHour",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "На сколько можно забронировать VIP столик(в часах)");

            migrationBuilder.AddColumn<string>(
                name: "BookingPhone",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Телефон для переноса времени брони");

            migrationBuilder.AddColumn<string>(
                name: "Hours_BookingWork",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Часы работы ресторана для заказа бронирования через ;");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "BookingHour", "BookingPhone", "Hours_BookingWork" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingHour",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "BookingPhone",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Hours_BookingWork",
                table: "Restaurants");
        }
    }
}
