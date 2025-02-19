﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace electro_shop_backend.Data;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductViewHistory> ProductViewHistories { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    public virtual DbSet<ReturnItem> ReturnItems { get; set; }

    public virtual DbSet<ReturnReason> ReturnReasons { get; set; }

    public virtual DbSet<VoucherDBContext> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" }
        }; 
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__10373C34A2F1E5F2");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__2EF52A277B4AE6E4");

            entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__user_id__48CFD27E");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__Cart_Ite__5D9A6C6EB6B31656");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK__Cart_Item__cart___4BAC3F29");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems).HasConstraintName("FK__Cart_Item__produ__4CA06362");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4C78B2D56");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK__Category__parent__3A81B327");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__BDBE9EF938A39A19");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__4659622958867F33");

            entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("FK__Order__user_id__5070F446");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__Order_It__3764B6BC03A32F00");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__Order_Ite__order__534D60F1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("FK__Order_Ite__produ__5441852A");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAE0EEEFC3");

            entity.Property(e => e.TransactionTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK__Payment__order_i__5812160E");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5BB06DC67");

            entity.Property(e => e.AverageRating).HasDefaultValue(0.0);
            entity.Property(e => e.RatingCount).HasDefaultValue(0);

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Product_C__categ__4222D4EF"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Product_C__produ__412EB0B6"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("PK__Product___1A56936EE0C9F88B");
                        j.ToTable("Product_Category");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });
        });

        modelBuilder.Entity<ProductDiscount>(entity =>
        {
            entity.HasKey(e => e.ProductDiscountId).HasName("PK__Product___7901250948A30B83");

            entity.HasOne(d => d.Discount).WithMany(p => p.ProductDiscounts).HasConstraintName("FK__Product_D__disco__72C60C4A");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDiscounts).HasConstraintName("FK__Product_D__produ__71D1E811");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId).HasName("PK__Product___0302EB4AD91F1FDC");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FK__Product_I__produ__44FF419A");
        });

        modelBuilder.Entity<ProductViewHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Product___096AA2E942A51BFF");

            entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductViewHistories).HasConstraintName("FK__Product_V__produ__6D0D32F4");

            entity.HasOne(d => d.User).WithMany(p => p.ProductViewHistories).HasConstraintName("FK__Product_V__user___6C190EBB");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ProductId }).HasName("PK__Rating__FDCE10D0498D2AD9");

            entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__product___68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__user_id__6754599E");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("PK__Return__35C23473531189E9");

            entity.Property(e => e.TimeStamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Returns).HasConstraintName("FK__Return__order_id__5DCAEF64");
        });

        modelBuilder.Entity<ReturnItem>(entity =>
        {
            entity.HasKey(e => e.ReturnItemId).HasName("PK__Return_I__3CFDE9F2758272B1");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.ReturnItems).HasConstraintName("FK__Return_It__order__619B8048");

            entity.HasOne(d => d.Return).WithMany(p => p.ReturnItems).HasConstraintName("FK__Return_It__retur__60A75C0F");
        });

        modelBuilder.Entity<ReturnReason>(entity =>
        {
            entity.HasKey(e => e.ReasonId).HasName("PK__Return_R__846BB55471535E7D");
        });

        modelBuilder.Entity<VoucherDBContext>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Voucher__80B6FFA89952A5CC");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsageCount).HasDefaultValue(0);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
