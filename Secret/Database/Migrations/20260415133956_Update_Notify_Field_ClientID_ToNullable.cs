using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Notify_Field_ClientID_ToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Notifications_Clients_ClientID",
                table: "Client_Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: true,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Код");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Notifications_Clients_ClientID",
                table: "Client_Notifications",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Notifications_Clients_ClientID",
                table: "Client_Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Код");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Notifications_Clients_ClientID",
                table: "Client_Notifications",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
