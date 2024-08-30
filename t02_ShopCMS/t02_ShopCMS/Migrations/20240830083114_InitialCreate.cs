using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t02_ShopCMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "類別編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "類別名稱")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "出貨編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "出貨商品名稱"),
                    ShipNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "出貨商品編號"),
                    Amount = table.Column<int>(type: "int", nullable: false, comment: "出貨商品數量"),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "出貨時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "產品序號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "產品名稱"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "產品說明"),
                    Content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "產品內容"),
                    Price = table.Column<int>(type: "int", nullable: false, comment: "產品價格"),
                    Stock = table.Column<int>(type: "int", nullable: false, comment: "產品庫存"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false, comment: "產品圖片"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "產品新增時間"),
                    CanOrder = table.Column<bool>(type: "bit", nullable: false, comment: "產品是否上架"),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "下單編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "下單商品名稱"),
                    Amount = table.Column<int>(type: "int", nullable: false, comment: "下單商品數量"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "下單商品類別"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "下單時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ShipmentList",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderList_ProductId",
                table: "OrderList",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderList");

            migrationBuilder.DropTable(
                name: "ShipmentList");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
