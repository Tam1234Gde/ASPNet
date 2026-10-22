using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_NumberQuests_To_Booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberQuests",
                table: "Bookings",
                type: "int",
                nullable: true,
                comment: "Количество гостей");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberQuests",
                table: "Bookings");
        }
    }
}
