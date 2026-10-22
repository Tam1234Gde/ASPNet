using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ClientID_To_Restaurants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientID",
                table: "Restaurants",
                type: "int",
                nullable: true,
                comment: "Системный клиент(заказы с сайта) - Код");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                column: "ClientID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ClientID",
                table: "Restaurants",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Clients_ClientID",
                table: "Restaurants",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Clients_ClientID",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_ClientID",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "Restaurants");
        }
    }
}
