using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFood.Migrations
{
    /// <inheritdoc />
    public partial class CategoryOfMealAddedRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Meals",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "Meals",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "CategoriesOfMeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesOfMeals_RestaurantId",
                table: "CategoriesOfMeals",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesOfMeals_Restaurants_RestaurantId",
                table: "CategoriesOfMeals",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesOfMeals_Restaurants_RestaurantId",
                table: "CategoriesOfMeals");

            migrationBuilder.DropIndex(
                name: "IX_CategoriesOfMeals_RestaurantId",
                table: "CategoriesOfMeals");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "CategoriesOfMeals");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Meals",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "Meals",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
