using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class MenuCategoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngridientGroupID",
                table: "Dish_Ingridients");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Ingridients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category_ID",
                table: "Ingridients",
                type: "int",
                nullable: true,
                comment: "ID категории, к которой принадлежит");

            migrationBuilder.AddColumn<DateTime>(
                name: "New_DateFrom",
                table: "Dishes",
                type: "datetime2",
                nullable: true,
                comment: "Дата до которой отображается новинка");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_Del",
                table: "Dish_Ingridients",
                type: "datetime2",
                nullable: true,
                comment: "Дата пометки на удаление");

            migrationBuilder.AddColumn<int>(
                name: "Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Flag_Del",
                table: "Dish_Ingridients",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Флаг пометки на удаление");

            migrationBuilder.AddColumn<int>(
                name: "Ingridient_Category_ID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                comment: "Категория ингридиента");

            migrationBuilder.AddColumn<int>(
                name: "UserID_Del",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                comment: "Пользователь, который поставил запись на удаление");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_IsParent",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Флаг - Родитель");

            migrationBuilder.AddColumn<bool>(
                name: "Flag_UseDefaultWorkHours",
                table: "Dish_Categories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Флаг - использовать ли общие часы работы из настроек");

            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Dish_Categories",
                type: "int",
                nullable: true,
                comment: "Идентификатор родителя");

            migrationBuilder.AddColumn<string>(
                name: "Work_Hours",
                table: "Dish_Categories",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Часы работы");

            migrationBuilder.CreateTable(
                name: "Dish_Ingridient_Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Код")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Наименование"),
                    Flag_Del = table.Column<bool>(type: "bit", nullable: false, comment: "Флаг пометки на удаление"),
                    Date_Del = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата пометки на удаление"),
                    UserID_Del = table.Column<int>(type: "int", nullable: true, comment: "Пользователь, который поставил запись на удаление")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish_Ingridient_Categories", x => x.ID);
                },
                comment: "Категории ингредиентов>");

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Flag_IsParent", "Flag_UseDefaultWorkHours", "ParentID", "Work_Hours" },
                values: new object[] { false, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Flag_IsParent", "Flag_UseDefaultWorkHours", "ParentID", "Work_Hours" },
                values: new object[] { false, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Flag_IsParent", "Flag_UseDefaultWorkHours", "ParentID", "Work_Hours" },
                values: new object[] { false, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dish_Categories",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "Flag_IsParent", "Flag_UseDefaultWorkHours", "ParentID", "Work_Hours" },
                values: new object[] { false, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Date_Del", "Dish_Ingridient_CategoryID", "Flag_Del", "Ingridient_Category_ID", "UserID_Del" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Date_Del", "Dish_Ingridient_CategoryID", "Flag_Del", "Ingridient_Category_ID", "UserID_Del" },
                values: new object[] { null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 1,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 2,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 3,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 4,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 5,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 6,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 7,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 8,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 9,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 10,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "ID",
                keyValue: 11,
                column: "New_DateFrom",
                value: null);

            migrationBuilder.UpdateData(
                table: "Ingridients",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CategoryID", "Category_ID" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Ingridients",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CategoryID", "Category_ID" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Ingridients_CategoryID",
                table: "Ingridients",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Ingridients_Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients",
                column: "Dish_Ingridient_CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_Categories_ParentID",
                table: "Dish_Categories",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Categories_Dish_Categories_ParentID",
                table: "Dish_Categories",
                column: "ParentID",
                principalTable: "Dish_Categories",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Ingridients_Dish_Ingridient_Categories_Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients",
                column: "Dish_Ingridient_CategoryID",
                principalTable: "Dish_Ingridient_Categories",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingridients_Dish_Ingridient_Categories_CategoryID",
                table: "Ingridients",
                column: "CategoryID",
                principalTable: "Dish_Ingridient_Categories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Categories_Dish_Categories_ParentID",
                table: "Dish_Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Ingridients_Dish_Ingridient_Categories_Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingridients_Dish_Ingridient_Categories_CategoryID",
                table: "Ingridients");

            migrationBuilder.DropTable(
                name: "Dish_Ingridient_Categories");

            migrationBuilder.DropIndex(
                name: "IX_Ingridients_CategoryID",
                table: "Ingridients");

            migrationBuilder.DropIndex(
                name: "IX_Dish_Ingridients_Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients");

            migrationBuilder.DropIndex(
                name: "IX_Dish_Categories_ParentID",
                table: "Dish_Categories");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Ingridients");

            migrationBuilder.DropColumn(
                name: "Category_ID",
                table: "Ingridients");

            migrationBuilder.DropColumn(
                name: "New_DateFrom",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "Date_Del",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Dish_Ingridient_CategoryID",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Flag_Del",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Ingridient_Category_ID",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "UserID_Del",
                table: "Dish_Ingridients");

            migrationBuilder.DropColumn(
                name: "Flag_IsParent",
                table: "Dish_Categories");

            migrationBuilder.DropColumn(
                name: "Flag_UseDefaultWorkHours",
                table: "Dish_Categories");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Dish_Categories");

            migrationBuilder.DropColumn(
                name: "Work_Hours",
                table: "Dish_Categories");

            migrationBuilder.AddColumn<int>(
                name: "IngridientGroupID",
                table: "Dish_Ingridients",
                type: "int",
                nullable: true,
                comment: "Группа");

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 1,
                column: "IngridientGroupID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dish_Ingridients",
                keyColumn: "ID",
                keyValue: 2,
                column: "IngridientGroupID",
                value: null);
        }
    }
}
