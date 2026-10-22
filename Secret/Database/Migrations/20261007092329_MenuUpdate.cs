using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class MenuUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu_OptionStates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_OptionStates", x => x.ID);
                },
                comment: "Словарь состояния активности для таблицы настроек нового меню");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    Menu_OptionStateID = table.Column<int>(type: "int", nullable: false, comment: "Код Состоянеие активности"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menus_Menu_OptionStates_Menu_OptionStateID",
                        column: x => x.Menu_OptionStateID,
                        principalTable: "Menu_OptionStates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Меню");

            migrationBuilder.CreateTable(
                name: "Menu_Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    MenuID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к меню"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Categories_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Категории меню");

            migrationBuilder.CreateTable(
                name: "Menu_Ingridient_Groups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    IsSelectable = table.Column<bool>(type: "bit", nullable: false, comment: "На выбор одна"),
                    MenuID = table.Column<int>(type: "int", nullable: true, comment: "Код Привязан к меню"),
                    Menu_OptionStateID = table.Column<int>(type: "int", nullable: false, comment: "Код Состоянеие активности")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Ingridient_Groups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Ingridient_Groups_Menu_OptionStates_Menu_OptionStateID",
                        column: x => x.Menu_OptionStateID,
                        principalTable: "Menu_OptionStates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Ingridient_Groups_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Группы ингредиентов меню");

            migrationBuilder.CreateTable(
                name: "Menu_Restaurants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к меню"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false, comment: "Код ресторана"),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата начала показа"),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата окончания показа"),
                    Menu_OptionStateID = table.Column<int>(type: "int", nullable: false, comment: "Код Состоянеие активности"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Restaurants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurants_Menu_OptionStates_Menu_OptionStateID",
                        column: x => x.Menu_OptionStateID,
                        principalTable: "Menu_OptionStates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurants_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurants_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Меню ресторана");

            migrationBuilder.CreateTable(
                name: "Menu_Subcategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    Menu_CategoryID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к категории"),
                    ParentID = table.Column<int>(type: "int", nullable: true, comment: "Код Родитель"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Subcategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Subcategories_Menu_Categories_Menu_CategoryID",
                        column: x => x.Menu_CategoryID,
                        principalTable: "Menu_Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Subcategories_Menu_Subcategories_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Menu_Subcategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Подкатегории меню");

            migrationBuilder.CreateTable(
                name: "Menu_Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование ингредиента"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Изображение ингредиента"),
                    Image_ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата изменения изображения"),
                    Price = table.Column<int>(type: "int", nullable: true, comment: "Цена"),
                    MenuID = table.Column<int>(type: "int", nullable: true, comment: "Код Привязан к меню"),
                    Menu_Ingridient_GroupID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к группе"),
                    Menu_OptionStateID = table.Column<int>(type: "int", nullable: false, comment: "Код Состоянеие активности"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Ingridients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Ingridients_Menu_Ingridient_Groups_Menu_Ingridient_GroupID",
                        column: x => x.Menu_Ingridient_GroupID,
                        principalTable: "Menu_Ingridient_Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Ingridients_Menu_OptionStates_Menu_OptionStateID",
                        column: x => x.Menu_OptionStateID,
                        principalTable: "Menu_OptionStates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Ingridients_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Ингредиенты меню");

            migrationBuilder.CreateTable(
                name: "Menu_Dishes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Menu_SubcategoryID = table.Column<int>(type: "int", nullable: false, comment: "Код Подкатегория"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование товара"),
                    Teaser = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Краткое описание блюда"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Полное описание блюда"),
                    Cooking_Minutes = table.Column<int>(type: "int", nullable: true, comment: "Количество минут для приготовления блюда"),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false, comment: "Стоимость"),
                    Flag_AllowColors = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана"),
                    Flag_Bonus = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг того, что блюдо можно купить за бонусы"),
                    Bonuses = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true, comment: "Количество бонусов, начисляемых за покупку"),
                    OrderNum = table.Column<int>(type: "int", nullable: false, comment: "Порядковый номер"),
                    FlagNew = table.Column<bool>(type: "bit", nullable: false, comment: "Новинка"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Изображение блюда"),
                    Image_ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата изменения изображения"),
                    RecommendDishIDs = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Коды рекомендуемых блюд"),
                    FlagNoActive = table.Column<bool>(type: "bit", nullable: false, comment: "Отключить отображение блюда в основном меню"),
                    New_DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата до которой отображается новинка"),
                    Vol1 = table.Column<int>(type: "int", nullable: true, comment: "Объем 1"),
                    Vol1_Price = table.Column<int>(type: "int", nullable: true, comment: "Объем 1 цена"),
                    Vol2 = table.Column<int>(type: "int", nullable: true, comment: "Объем 2"),
                    Vol2_Price = table.Column<int>(type: "int", nullable: true, comment: "Объем  цена"),
                    Vol3 = table.Column<int>(type: "int", nullable: true, comment: "Объем 3"),
                    Vol3_Price = table.Column<int>(type: "int", nullable: true, comment: "Объем 3 цена"),
                    Vol4 = table.Column<int>(type: "int", nullable: true, comment: "Объем 4"),
                    Vol4_Price = table.Column<int>(type: "int", nullable: true, comment: "Объем 4 цена"),
                    Vol5 = table.Column<int>(type: "int", nullable: true, comment: "Объем 5"),
                    Vol5_Price = table.Column<int>(type: "int", nullable: true, comment: "Объем 5 цена"),
                    Menu_OptionStateID = table.Column<int>(type: "int", nullable: false, comment: "Код Состоянеие активности"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата и время удаления записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Dishes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Dishes_Menu_OptionStates_Menu_OptionStateID",
                        column: x => x.Menu_OptionStateID,
                        principalTable: "Menu_OptionStates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Dishes_Menu_Subcategories_Menu_SubcategoryID",
                        column: x => x.Menu_SubcategoryID,
                        principalTable: "Menu_Subcategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Блюда меню");

            migrationBuilder.CreateTable(
                name: "Menu_Dish_Ingridients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Menu_DishID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к меню"),
                    Menu_IngridientID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к Ингридиенту")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Dish_Ingridients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Dish_Ingridients_Menu_Dishes_Menu_DishID",
                        column: x => x.Menu_DishID,
                        principalTable: "Menu_Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Dish_Ingridients_Menu_Ingridients_Menu_IngridientID",
                        column: x => x.Menu_IngridientID,
                        principalTable: "Menu_Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Ингридиенты блюд меню");

            migrationBuilder.CreateTable(
                name: "Menu_Restaurant_Options",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Menu_RestaurantID = table.Column<int>(type: "int", nullable: true, comment: "Код Соответствие меню ресторану"),
                    Menu_DishID = table.Column<int>(type: "int", nullable: true, comment: "Код Блюдо"),
                    Menu_IngridientID = table.Column<int>(type: "int", nullable: true, comment: "Код Ингридиент"),
                    Menu_Ingridient_GroupID = table.Column<int>(type: "int", nullable: true, comment: "Код Группа ингредиента"),
                    MenuID = table.Column<int>(type: "int", nullable: false, comment: "Код Привязан к меню")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Restaurant_Options", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_Options_Menu_Dishes_Menu_DishID",
                        column: x => x.Menu_DishID,
                        principalTable: "Menu_Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_Options_Menu_Ingridient_Groups_Menu_Ingridient_GroupID",
                        column: x => x.Menu_Ingridient_GroupID,
                        principalTable: "Menu_Ingridient_Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_Options_Menu_Ingridients_Menu_IngridientID",
                        column: x => x.Menu_IngridientID,
                        principalTable: "Menu_Ingridients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_Options_Menu_Restaurants_Menu_RestaurantID",
                        column: x => x.Menu_RestaurantID,
                        principalTable: "Menu_Restaurants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_Options_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Опции меню ресторана");

            migrationBuilder.InsertData(
                table: "Menu_OptionStates",
                columns: new[] { "ID", "Value" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Categories_MenuID",
                table: "Menu_Categories",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Dish_Ingridients_Menu_DishID",
                table: "Menu_Dish_Ingridients",
                column: "Menu_DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Dish_Ingridients_Menu_IngridientID",
                table: "Menu_Dish_Ingridients",
                column: "Menu_IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Dishes_Menu_OptionStateID",
                table: "Menu_Dishes",
                column: "Menu_OptionStateID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Dishes_Menu_SubcategoryID",
                table: "Menu_Dishes",
                column: "Menu_SubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Ingridient_Groups_Menu_OptionStateID",
                table: "Menu_Ingridient_Groups",
                column: "Menu_OptionStateID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Ingridient_Groups_MenuID",
                table: "Menu_Ingridient_Groups",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Ingridients_Menu_Ingridient_GroupID",
                table: "Menu_Ingridients",
                column: "Menu_Ingridient_GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Ingridients_Menu_OptionStateID",
                table: "Menu_Ingridients",
                column: "Menu_OptionStateID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Ingridients_MenuID",
                table: "Menu_Ingridients",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurant_Options_Menu_DishID",
                table: "Menu_Restaurant_Options",
                column: "Menu_DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurant_Options_Menu_Ingridient_GroupID",
                table: "Menu_Restaurant_Options",
                column: "Menu_Ingridient_GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurant_Options_Menu_IngridientID",
                table: "Menu_Restaurant_Options",
                column: "Menu_IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurant_Options_Menu_RestaurantID",
                table: "Menu_Restaurant_Options",
                column: "Menu_RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurant_Options_MenuID",
                table: "Menu_Restaurant_Options",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurants_Menu_OptionStateID",
                table: "Menu_Restaurants",
                column: "Menu_OptionStateID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurants_MenuID",
                table: "Menu_Restaurants",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Restaurants_RestaurantID",
                table: "Menu_Restaurants",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Subcategories_Menu_CategoryID",
                table: "Menu_Subcategories",
                column: "Menu_CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Subcategories_ParentID",
                table: "Menu_Subcategories",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Menu_OptionStateID",
                table: "Menus",
                column: "Menu_OptionStateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu_Dish_Ingridients");

            migrationBuilder.DropTable(
                name: "Menu_Restaurant_Options");

            migrationBuilder.DropTable(
                name: "Menu_Dishes");

            migrationBuilder.DropTable(
                name: "Menu_Ingridients");

            migrationBuilder.DropTable(
                name: "Menu_Restaurants");

            migrationBuilder.DropTable(
                name: "Menu_Subcategories");

            migrationBuilder.DropTable(
                name: "Menu_Ingridient_Groups");

            migrationBuilder.DropTable(
                name: "Menu_Categories");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Menu_OptionStates");
        }
    }
}
