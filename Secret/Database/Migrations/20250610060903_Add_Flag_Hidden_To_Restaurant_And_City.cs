using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Flag_Hidden_To_Restaurant_And_City : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Flag_Hidden",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Отключен");

            migrationBuilder.AddColumn<string>(
                name: "HiddenText",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Тело сообщения при отключенном ресторане");

            migrationBuilder.AddColumn<string>(
                name: "HiddenTitle",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Заголовок сообщения при отключенном ресторане");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Hidden",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Отключен");

            migrationBuilder.AddColumn<string>(
                name: "HiddenText",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Тело сообщения при отключенном ресторане");

            migrationBuilder.AddColumn<string>(
                name: "HiddenTitle",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Заголовок сообщения при отключенном ресторане");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Flag_Hidden", "HiddenText", "HiddenTitle" },
                values: new object[] { false, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag_Hidden",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "HiddenText",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "HiddenTitle",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Flag_Hidden",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "HiddenText",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "HiddenTitle",
                table: "Cities");
        }
    }
}
