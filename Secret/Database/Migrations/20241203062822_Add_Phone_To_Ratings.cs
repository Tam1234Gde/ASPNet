using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Phone_To_Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Номер телефона");

            migrationBuilder.AddColumn<int>(
                name: "Restaurant_TableID",
                table: "Ratings",
                type: "int",
                nullable: true,
                comment: "Код столика");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Restaurant_TableID",
                table: "Ratings");
        }
    }
}
