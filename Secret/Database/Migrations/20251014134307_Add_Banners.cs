using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Banners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Название"),
                    Image_Big = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Баннер большой"),
                    Image_Small = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Баннер маленький"),
                    CityID = table.Column<int>(type: "int", nullable: false, comment: "Город - код"),
                    Category_ID = table.Column<int>(type: "int", nullable: true, comment: "Категория - ID (Для ссылки)"),
                    Product_ID = table.Column<int>(type: "int", nullable: true, comment: "Продукт - ID (Для ссылки)"),
                    Date_Start = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата начала показа"),
                    Date_End = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата окончания показа"),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата создания"),
                    OrderNum = table.Column<int>(type: "int", nullable: false, comment: "Порядок отображения"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Banners_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Банеры");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_CityID",
                table: "Banners",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
