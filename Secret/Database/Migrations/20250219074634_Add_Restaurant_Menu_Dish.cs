using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Restaurant_Menu_Dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurant_Menus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false, comment: "Код ресторана"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название меню"),
                    Flag_Active = table.Column<bool>(type: "bit", nullable: false, comment: "Активен"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Menus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menus_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Меню ресторана>");

            migrationBuilder.CreateTable(
                name: "Restaurant_Menu_Dishes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Restaurant_MenuID = table.Column<int>(type: "int", nullable: false, comment: "Код меню"),
                    DishID = table.Column<int>(type: "int", nullable: false, comment: "Код блюда"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Стоимость блюда"),
                    Flag_Available = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг доступности"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Menu_Dishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dishes_Dishes_DishID",
                        column: x => x.DishID,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dishes_Restaurant_Menus_Restaurant_MenuID",
                        column: x => x.Restaurant_MenuID,
                        principalTable: "Restaurant_Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Меню ресторана>");

            migrationBuilder.CreateTable(
                name: "Restaurant_Menu_Dish_Ingredients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Restaurant_Menu_DishID = table.Column<int>(type: "int", nullable: false, comment: "Блюдо - Код"),
                    IngridientID = table.Column<int>(type: "int", nullable: false, comment: "Ингридиент - Код"),
                    Cooking_Minutes = table.Column<int>(type: "int", nullable: true, comment: "Количество минут для приготовления"),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, comment: "Стоимость"),
                    IngridientGroupID = table.Column<int>(type: "int", nullable: true, comment: "Группа"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Menu_Dish_Ingredients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dish_Ingredients_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dish_Ingredients_Restaurant_Menu_Dishes_Restaurant_Menu_DishID",
                        column: x => x.Restaurant_Menu_DishID,
                        principalTable: "Restaurant_Menu_Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Ингредиенты блюда в меню ресторана");

            migrationBuilder.CreateTable(
                name: "Restaurant_Menu_Dish_Volumes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Restaurant_Menu_DishID = table.Column<int>(type: "int", nullable: false, comment: "Блюдо - Код"),
                    VolumeID = table.Column<int>(type: "int", nullable: false, comment: "Объем - Код"),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, comment: "Цена за этот обьем"),
                    OrderNum = table.Column<int>(type: "int", nullable: false, comment: "Порядковый номер"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant_Menu_Dish_Volumes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dish_Volumes_Restaurant_Menu_Dishes_Restaurant_Menu_DishID",
                        column: x => x.Restaurant_Menu_DishID,
                        principalTable: "Restaurant_Menu_Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_Menu_Dish_Volumes_Volumes_VolumeID",
                        column: x => x.VolumeID,
                        principalTable: "Volumes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Объемы блюда в меню ресторана");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dish_Ingredients_IngridientID",
                table: "Restaurant_Menu_Dish_Ingredients",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dish_Ingredients_Restaurant_Menu_DishID",
                table: "Restaurant_Menu_Dish_Ingredients",
                column: "Restaurant_Menu_DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dish_Volumes_Restaurant_Menu_DishID",
                table: "Restaurant_Menu_Dish_Volumes",
                column: "Restaurant_Menu_DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dish_Volumes_VolumeID",
                table: "Restaurant_Menu_Dish_Volumes",
                column: "VolumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dishes_DishID",
                table: "Restaurant_Menu_Dishes",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menu_Dishes_Restaurant_MenuID",
                table: "Restaurant_Menu_Dishes",
                column: "Restaurant_MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Menus_RestaurantID",
                table: "Restaurant_Menus",
                column: "RestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restaurant_Menu_Dish_Ingredients");

            migrationBuilder.DropTable(
                name: "Restaurant_Menu_Dish_Volumes");

            migrationBuilder.DropTable(
                name: "Restaurant_Menu_Dishes");

            migrationBuilder.DropTable(
                name: "Restaurant_Menus");
        }
    }
}
