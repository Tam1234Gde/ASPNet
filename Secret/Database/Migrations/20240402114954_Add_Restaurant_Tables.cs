using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Restaurant_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Val2",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи");

            migrationBuilder.AddColumn<string>(
                name: "Val2_Comment",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Комментарий к записи 2");

            migrationBuilder.AddColumn<string>(
                name: "Val3",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи");

            migrationBuilder.AddColumn<string>(
                name: "Val3_Comment",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Комментарий к записи 3");

            migrationBuilder.AddColumn<string>(
                name: "Val4",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Значение записи");

            migrationBuilder.AddColumn<string>(
                name: "Val4_Comment",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Комментарий к записи 4");

            migrationBuilder.AddColumn<string>(
                name: "Val_Comment",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Комментарий к записи 1");

            migrationBuilder.AddColumn<string>(
                name: "Hours_Work",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hours_WorkApp",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Restaurant_Tables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag_IsAvailableApp = table.Column<bool>(type: "bit", nullable: false),
                    Flag_IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Tables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Tables_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Action = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Restaurant_TableID = table.Column<int>(type: "int", nullable: false),
                    Date_Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID_Del = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bookings_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Bookings_Restaurant_Tables_Restaurant_TableID",
                        column: x => x.Restaurant_TableID,
                        principalTable: "Restaurant_Tables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Hours_Work", "Hours_WorkApp" },
                values: new object[] { "09:00;22:00", "12:00;15:00" });

            migrationBuilder.UpdateData(
                table: "Setups",
                keyColumn: "Key",
                keyValue: "Dishes_DateChange",
                columns: new[] { "Val2", "Val2_Comment", "Val3", "Val3_Comment", "Val4", "Val4_Comment", "Val_Comment" },
                values: new object[] { null, null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClientID",
                table: "Bookings",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Restaurant_TableID",
                table: "Bookings",
                column: "Restaurant_TableID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Tables_RestaurantID",
                table: "Restaurant_Tables",
                column: "RestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Restaurant_Tables");

            migrationBuilder.DropColumn(
                name: "Val2",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val2_Comment",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val3",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val3_Comment",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val4",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val4_Comment",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Val_Comment",
                table: "Setups");

            migrationBuilder.DropColumn(
                name: "Hours_Work",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Hours_WorkApp",
                table: "Restaurants");
        }
    }
}
