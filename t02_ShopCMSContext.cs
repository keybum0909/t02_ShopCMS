using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using t02_ShopCMS.Models;

namespace t02_ShopCMS.Data
{
    public partial class t02_ShopCMSContext : DbContext
    {
        public t02_ShopCMSContext (DbContextOptions<t02_ShopCMSContext> options)
            : base(options)
        {
        }

        //Table代理人
        public DbSet<t02_ShopCMS.Models.Product> Product { get; set; }
        public DbSet<t02_ShopCMS.Models.Category> Category { get; set; }

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
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("產品介紹");

                entity.Property(e => e.Content)
                    .IsRequired()
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

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
