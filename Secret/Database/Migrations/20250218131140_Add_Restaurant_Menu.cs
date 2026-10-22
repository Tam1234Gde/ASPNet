using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Restaurant_Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityID",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "Город - Код");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Название города"),
                    OrderNum = table.Column<int>(type: "int", nullable: false, comment: "Порядок отображения"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                },
                comment: "Город>");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                column: "CityID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CityID",
                table: "Restaurants",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Cities_CityID",
                table: "Restaurants",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Cities_CityID",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CityID",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CityID",
                table: "Restaurants");
        }
    }
}
