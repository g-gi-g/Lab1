using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceWebApplication.Data;

public partial class DbmarketplaceContext : DbContext
{
    public DbmarketplaceContext()
    {
        Database.EnsureCreated();
    }

    public DbmarketplaceContext(DbContextOptions<DbmarketplaceContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferCategory> OfferCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<SavedOffer> SavedOffers { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.TimeCreated).HasColumnType("datetime");

            entity.HasOne(d => d.Offer).WithMany(p => p.Chats)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Chats_Offers");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.Text).HasMaxLength(200);
            entity.Property(e => e.TimeAdded).HasColumnType("datetime");

            entity.HasOne(d => d.Offer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Feedbacks_Offers");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Feedbacks_Users");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Text).HasMaxLength(200);
            entity.Property(e => e.TimeAdded).HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Messages_Chats");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.Text).HasMaxLength(300);
            entity.Property(e => e.TimeAdded).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Class).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Notifications_NotificationClasses");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_NotificationClasses");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.TimeAdded).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Offers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Offers_Categories");

            entity.HasOne(d => d.Seller).WithMany(p => p.Offers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Offers_Users");
        });

        modelBuilder.Entity<OfferCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Categories");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.DateOfOrder).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Users");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_PaymentMethods");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_Statuses");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderItem");

            entity.HasOne(d => d.Offer).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderItem_Offers");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderItem_Orders");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Statuses");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SavedOffer>(entity =>
        {
            entity.Property(e => e.TimeAdded).HasColumnType("datetime");

            entity.HasOne(d => d.Offer).WithMany(p => p.SavedOffers)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SavedOffers_Offers");

            entity.HasOne(d => d.User).WithMany(p => p.SavedOffers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SavedOffers_Users");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.Property(e => e.ArrivalBuildingNumber).HasMaxLength(10);
            entity.Property(e => e.ArrivalCity).HasMaxLength(50);
            entity.Property(e => e.ArrivalCountry).HasMaxLength(50);
            entity.Property(e => e.ArrivalStreet).HasMaxLength(50);
            entity.Property(e => e.ArrivalZipCode).HasMaxLength(10);
            entity.Property(e => e.DateStarted).HasColumnType("datetime");
            entity.Property(e => e.DepartmentBuildingNumber).HasMaxLength(10);
            entity.Property(e => e.DepartmentCity).HasMaxLength(50);
            entity.Property(e => e.DepartmentCountry).HasMaxLength(50);
            entity.Property(e => e.DepartmentStreet).HasMaxLength(50);
            entity.Property(e => e.DepartmentZipCode).HasMaxLength(10);

            entity.HasOne(d => d.Order).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Shippings_Orders");

            entity.HasOne(d => d.ShippingCompany).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.ShippingCompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Shippings_ShippingCompanies");
        });

        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.DateOfRegistration).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
