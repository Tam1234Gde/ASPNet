using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class PriceFieldIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Flag_Price_Overridden",
                table: "Menu_Ingridients",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Цена была переопределена вручную");

            migrationBuilder.AddColumn<int>(
                name: "Default_Price",
                table: "Menu_Ingridient_Groups",
                type: "int",
                nullable: true,
                comment: "Цена по умолчанию для новых ингредиентов группы");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag_Price_Overridden",
                table: "Menu_Ingridients");

            migrationBuilder.DropColumn(
                name: "Default_Price",
                table: "Menu_Ingridient_Groups");
        }
    }
}
