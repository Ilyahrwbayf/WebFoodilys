using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFood.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantType_Categories_TypeId",
                table: "RestaurantType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "TypesOfRestaurants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesOfRestaurants",
                table: "TypesOfRestaurants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantType_TypesOfRestaurants_TypeId",
                table: "RestaurantType",
                column: "TypeId",
                principalTable: "TypesOfRestaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantType_TypesOfRestaurants_TypeId",
                table: "RestaurantType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesOfRestaurants",
                table: "TypesOfRestaurants");

            migrationBuilder.RenameTable(
                name: "TypesOfRestaurants",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantType_Categories_TypeId",
                table: "RestaurantType",
                column: "TypeId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
