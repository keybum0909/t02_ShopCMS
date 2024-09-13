using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t02_ShopCMS.Migrations
{
    /// <inheritdoc />
    public partial class ShipmentList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShipNumber",
                table: "ShipmentList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "出貨產品編號",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "出貨商品編號");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ShipmentList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "出貨產品名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "出貨商品名稱");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "ShipmentList",
                type: "int",
                nullable: false,
                comment: "出貨產品數量",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "出貨商品數量");

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "ShipmentList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "總金額");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                comment: "產品說明",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "產品說明");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                comment: "產品內容",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "產品內容");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "下單產品名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "下單商品名稱");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "OrderList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "下單產品類別",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "下單商品類別");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "OrderList",
                type: "int",
                nullable: false,
                comment: "下單產品數量",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "下單商品數量");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShipmentList");

            migrationBuilder.AlterColumn<string>(
                name: "ShipNumber",
                table: "ShipmentList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "出貨商品編號",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "出貨產品編號");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ShipmentList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "出貨商品名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "出貨產品名稱");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "ShipmentList",
                type: "int",
                nullable: false,
                comment: "出貨商品數量",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "出貨產品數量");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "產品說明",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "產品說明");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "產品內容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "產品內容");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "下單商品名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "下單產品名稱");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "OrderList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "下單商品類別",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "下單產品類別");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "OrderList",
                type: "int",
                nullable: false,
                comment: "下單商品數量",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "下單產品數量");
        }
    }
}
