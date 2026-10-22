using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Dish_Cooking_Minutes_And_User_Flags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Flag_Oper",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Root",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Cooking_Minutes",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cooking_Minutes",
                table: "Dish_Categories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 4,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 1,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 2,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 1,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 2,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 3,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 4,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 5,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 6,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 7,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 8,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 9,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 10,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 11,
                column: "Cooking_Minutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Flag_Oper", "Flag_Root" },
                values: new object[] { false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag_Oper",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Flag_Root",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cooking_Minutes",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Cooking_Minutes",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Cooking_Minutes",
                table: "Dish_Categories");
        }
    }
}
