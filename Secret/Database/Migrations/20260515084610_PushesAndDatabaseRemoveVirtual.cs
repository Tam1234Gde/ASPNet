using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class PushesAndDatabaseRemoveVirtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientNotification_Histories");

            migrationBuilder.DropColumn(
                name: "Date_ToPush",
                table: "Client_Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: true,
                comment: "Код клиента",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Код");

            migrationBuilder.CreateTable(
                name: "Client_Notification_Planned",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Заголовок уведомления"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Текст уведомления"),
                    Date_Created = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата создания"),
                    Date_Send = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата отправки"),
                    Flag_Selected = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг отправка выбранным из Client_Notification_Planned_Data"),
                    Client_Notification_ShablonID = table.Column<int>(type: "int", nullable: true, comment: "Код шаблона"),
                    Flag_Sended = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг отправлено"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг помеки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Notification_Planned", x => x.ID);
                },
                comment: "Запланированные уведомления");

            migrationBuilder.CreateTable(
                name: "Client_Notification_Planned_Data",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_Notification_Planned_ID = table.Column<int>(type: "int", nullable: false, comment: "Заказ - Код"),
                    ClientID = table.Column<int>(type: "int", nullable: false, comment: "Клиент - Код"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг помеки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Notification_Planned_Data", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_Notification_Planned_Data_Client_Notification_Planned_Client_Notification_Planned_ID",
                        column: x => x.Client_Notification_Planned_ID,
                        principalTable: "Client_Notification_Planned",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_Notification_Planned_Data_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Запланированные уведомления данные отправки выборочно.");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Notification_Planned_Data_Client_Notification_Planned_ID",
                table: "Client_Notification_Planned_Data",
                column: "Client_Notification_Planned_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Notification_Planned_Data_ClientID",
                table: "Client_Notification_Planned_Data",
                column: "ClientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client_Notification_Planned_Data");

            migrationBuilder.DropTable(
                name: "Client_Notification_Planned");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Client_Notifications",
                type: "int",
                nullable: true,
                comment: "Код",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Код клиента");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_ToPush",
                table: "Client_Notifications",
                type: "datetime2",
                nullable: true,
                comment: "Дата запланированной отправки");

            migrationBuilder.CreateTable(
                name: "ClientNotification_Histories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Текст уведомления"),
                    DateAct = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата отправки"),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false, comment: "Файл"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Заголовок уведомления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNotification_Histories", x => x.ID);
                },
                comment: "История отправки кастомных уведомлений");
        }
    }
}
