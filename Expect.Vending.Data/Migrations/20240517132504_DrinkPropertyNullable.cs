using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expect.Vending.Data.Migrations
{
    /// <inheritdoc />
    public partial class DrinkPropertyNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Drinks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "Drinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Images_ImageId",
                table: "Drinks",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
