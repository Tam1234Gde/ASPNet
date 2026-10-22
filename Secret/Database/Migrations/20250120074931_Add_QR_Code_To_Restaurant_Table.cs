using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_QR_Code_To_Restaurant_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QR_Code",
                table: "Restaurant_Tables",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Уникальный код столика для QR-кода");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QR_Code",
                table: "Restaurant_Tables");
        }
    }
}
