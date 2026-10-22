using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_DishIDs_To_Dishes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecommendDishIDs",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Коды рекомендуемых блюд");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 1,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 2,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 3,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 4,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 5,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 6,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 7,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 8,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 9,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 10,
                column: "RecommendDishIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 11,
                column: "RecommendDishIDs",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendDishIDs",
                table: "Dishes");
        }
    }
}
