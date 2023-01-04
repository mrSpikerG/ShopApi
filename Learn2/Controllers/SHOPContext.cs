using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Learn2.Models;

namespace Learn2.Controllers
{
    public partial class SHOPContext : DbContext
    {
        public SHOPContext()
        {
        }

        public SHOPContext(DbContextOptions<SHOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryConnection> CategoryConnections { get; set; } = null!;
        public virtual DbSet<ShopItem> ShopItems { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
             //   optionsBuilder.UseSqlServer("Data Source=DESKTOP-2I5L44I\\SQLEXPRESS;Initial Catalog=SHOP;Integrated Security=SSPI");
                optionsBuilder.UseSqlServer("Data Source=SQL5063.site4now.net;Initial Catalog=db_a9219e_shop;User Id=db_a9219e_shop_admin;Password=LF721)$lrF9]_");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategoryConnection>(entity =>
            {
                entity.ToTable("CategoryConnection");
            });

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.ToTable("ShopItem");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<UserInfo>(entity => {
                entity.ToTable("UserInfo");

                entity.Property(e => e.ApiKey)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APIKEY");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
