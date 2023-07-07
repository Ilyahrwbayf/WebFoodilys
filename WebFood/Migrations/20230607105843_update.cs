using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFood.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersMeal_Meals_MealId",
                table: "OrdersMeal");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersMeal_Orders_OrderId",
                table: "OrdersMeal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersMeal",
                table: "OrdersMeal");

            migrationBuilder.RenameTable(
                name: "OrdersMeal",
                newName: "OrderMeal");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersMeal_OrderId",
                table: "OrderMeal",
                newName: "IX_OrderMeal_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersMeal_MealId",
                table: "OrderMeal",
                newName: "IX_OrderMeal_MealId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderMeal",
                table: "OrderMeal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderMeal_Meals_MealId",
                table: "OrderMeal",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderMeal_Orders_OrderId",
                table: "OrderMeal",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderMeal_Meals_MealId",
                table: "OrderMeal");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMeal_Orders_OrderId",
                table: "OrderMeal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderMeal",
                table: "OrderMeal");

            migrationBuilder.RenameTable(
                name: "OrderMeal",
                newName: "OrdersMeal");

            migrationBuilder.RenameIndex(
                name: "IX_OrderMeal_OrderId",
                table: "OrdersMeal",
                newName: "IX_OrdersMeal_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderMeal_MealId",
                table: "OrdersMeal",
                newName: "IX_OrdersMeal_MealId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersMeal",
                table: "OrdersMeal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersMeal_Meals_MealId",
                table: "OrdersMeal",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersMeal_Orders_OrderId",
                table: "OrdersMeal",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
