using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_FlagNew_To_Dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Restaurant_Menu_Dishes",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                comment: "Стоимость блюда",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true,
                oldComment: "Стоимость блюда");

            migrationBuilder.AddColumn<bool>(
                name: "FlagNew",
                table: "Restaurant_Menu_Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Новинка");

            migrationBuilder.AddColumn<int>(
                name: "OrderNum",
                table: "Restaurant_Menu_Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Порядковый номер");

            migrationBuilder.AddColumn<bool>(
                name: "FlagNew",
                table: "Dishes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Новинка");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlagNew",
                table: "Restaurant_Menu_Dishes");

            migrationBuilder.DropColumn(
                name: "OrderNum",
                table: "Restaurant_Menu_Dishes");

            migrationBuilder.DropColumn(
                name: "FlagNew",
                table: "Dishes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Restaurant_Menu_Dishes",
                type: "decimal(18,2)",
                nullable: true,
                comment: "Стоимость блюда",
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Стоимость блюда");
        }
    }
}
