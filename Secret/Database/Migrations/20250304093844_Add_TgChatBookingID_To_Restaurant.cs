using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_TgChatBookingID_To_Restaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TgChatBookingID",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Telegram - Бронирование - Код чата");

            migrationBuilder.AddColumn<string>(
                name: "TgChatQrID",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Telegram - Qr - Код чата");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "TgChatBookingID", "TgChatQrID" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TgChatBookingID",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TgChatQrID",
                table: "Restaurants");
        }
    }
}
