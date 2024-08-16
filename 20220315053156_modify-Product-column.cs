using Microsoft.EntityFrameworkCore.Migrations;

namespace t02_ShopCMS.Migrations
{
    public partial class modifyProductcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategorydId",
                table: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Category",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategorydId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
