using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDE.RestAndRant.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Client_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Car_Brand",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Марка машины");

            migrationBuilder.AddColumn<string>(
                name: "Car_Color",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Цвет машины");

            migrationBuilder.AddColumn<string>(
                name: "Car_RegNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Госномера машины");

            migrationBuilder.AddColumn<string>(
                name: "Delivery_Address",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Адрес доставки");

            migrationBuilder.AddColumn<string>(
                name: "Delivery_Apartment",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Квартира");

            migrationBuilder.AddColumn<string>(
                name: "Delivery_EntranceCode",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Код подъезда");

            migrationBuilder.AddColumn<string>(
                name: "Delivery_EntranceNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Домофон");

            migrationBuilder.AddColumn<string>(
                name: "Delivery_Floor",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Номер этажа");

            migrationBuilder.AddColumn<string>(
                name: "Name_Another",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Имя - Другому человеку");

            migrationBuilder.AddColumn<string>(
                name: "Phone_Another",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Телефон - Другому человеку");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Car_Brand", "Car_Color", "Car_RegNumber", "Delivery_Address", "Delivery_Apartment", "Delivery_EntranceCode", "Delivery_EntranceNumber", "Delivery_Floor", "Name_Another", "Phone_Another" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Car_Brand", "Car_Color", "Car_RegNumber", "Delivery_Address", "Delivery_Apartment", "Delivery_EntranceCode", "Delivery_EntranceNumber", "Delivery_Floor", "Name_Another", "Phone_Another" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Car_Brand",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Car_Color",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Car_RegNumber",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Delivery_Address",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Delivery_Apartment",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Delivery_EntranceCode",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Delivery_EntranceNumber",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Delivery_Floor",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Name_Another",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Phone_Another",
                table: "Clients");
        }
    }
}
