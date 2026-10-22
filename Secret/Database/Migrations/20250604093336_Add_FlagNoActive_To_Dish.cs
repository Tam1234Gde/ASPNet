using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_FlagNoActive_To_Dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FlagNoActive",
                table: "Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Отключить отображение блюда в основном меню");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlagNoActive",
                table: "Dishes");
        }
    }
}
