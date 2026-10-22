using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ClientNotification_Histories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientNotification_Histories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Заголовок уведомления"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Текст уведомления"),
                    DateAct = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата отправки"),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false, comment: "Файл")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNotification_Histories", x => x.ID);
                },
                comment: "История отправки кастомных уведомлений");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientNotification_Histories");
        }
    }
}
