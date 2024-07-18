using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expect.Vending.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelationDrinkImageRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_ImageId",
                table: "Drinks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Drinks_ImageId",
                table: "Drinks",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
