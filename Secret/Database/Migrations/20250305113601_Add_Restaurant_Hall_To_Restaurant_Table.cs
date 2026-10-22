using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Restaurant_Hall_To_Restaurant_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Restaurant_HallID",
                table: "Restaurant_Tables",
                type: "int",
                nullable: true,
                comment: "Код планировки");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Restaurant_Halls",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Название");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Tables_Restaurant_HallID",
                table: "Restaurant_Tables",
                column: "Restaurant_HallID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Halls_Restaurant_HallID",
                table: "Restaurant_Tables",
                column: "Restaurant_HallID",
                principalTable: "Restaurant_Halls",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Tables_Restaurant_Halls_Restaurant_HallID",
                table: "Restaurant_Tables");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_Tables_Restaurant_HallID",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Restaurant_HallID",
                table: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Restaurant_Halls");
        }
    }
}
