using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Client_Notification_ShablonID_To_Client_Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Client_Notification_ShablonID",
                table: "Client_Notifications",
                type: "int",
                nullable: true,
                comment: "Код шаблона");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client_Notification_ShablonID",
                table: "Client_Notifications");
        }
    }
}
