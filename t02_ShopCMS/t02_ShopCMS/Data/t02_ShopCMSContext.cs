using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using t02_ShopCMS.Entity;

namespace t02_ShopCMS.Data
{
    public partial class t02_ShopCMSContext : DbContext
    {
        public t02_ShopCMSContext (DbContextOptions<t02_ShopCMSContext> options)
            : base(options)
        {
        }

        //Table代理人
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
        public DbSet<ShipmentList> ShipmentList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasComment("產品序號");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("產品名稱");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasComment("產品說明");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasComment("產品內容");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasComment("產品價格");

                entity.Property(e => e.Stock)
                    .IsRequired()
                    .HasComment("產品庫存");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasConversion<byte[]>()
                    .HasComment("產品圖片");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasConversion<DateTime>()
                    .HasComment("產品新增時間");

                entity.Property(e => e.CanOrder)
                    .IsRequired()
                    .HasConversion<bool>()
                    .HasComment("產品是否上架");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_Category");

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasComment("類別編號");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("類別名稱");
            });

            modelBuilder.Entity<OrderList>(entity =>
            {
                entity.ToTable("OrderList");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasComment("下單編號");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("下單商品名稱");

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasComment("下單商品數量");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("下單商品類別");

                entity.Property(e => e.CreateTime)
                    .IsRequired()
                    .HasConversion<DateTime>()
                    .HasComment("下單時間");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_ShipmentList");
            });

            modelBuilder.Entity<ShipmentList>(entity =>
            {
                entity.ToTable("ShipmentList");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasComment("出貨編號");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("出貨商品名稱");

                entity.Property(e => e.ShipNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("出貨商品編號");

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasComment("出貨商品數量");

                entity.Property(e => e.OrderTime)
                    .IsRequired()
                    .HasConversion<DateTime>()
                    .HasComment("出貨時間");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
