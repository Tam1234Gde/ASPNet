using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class OrderNums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Menu_Subcategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Порядок сортировки");

            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Menu_Ingridients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Порядок сортировки");

            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Menu_Ingridient_Groups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Порядок сортировки");

            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Menu_Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Порядок сортировки");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Menu_Subcategories");

            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Menu_Ingridients");

            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Menu_Ingridient_Groups");

            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Menu_Categories");
        }
    }
}
