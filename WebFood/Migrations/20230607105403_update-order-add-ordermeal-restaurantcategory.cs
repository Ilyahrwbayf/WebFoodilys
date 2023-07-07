using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFood.Migrations
{
    /// <inheritdoc />
    public partial class updateorderaddordermealrestaurantcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Orders",
                newName: "GuestPhone");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Orders",
                newName: "GuestName");

            migrationBuilder.RenameColumn(
                name: "Adres",
                table: "Orders",
                newName: "GuestAdres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuestPhone",
                table: "Orders",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "GuestName",
                table: "Orders",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "GuestAdres",
                table: "Orders",
                newName: "Adres");
        }
    }
}
