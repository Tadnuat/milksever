using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MilkStore.Repo.Entities
{
    public partial class MilkContext : DbContext
    {
        public MilkContext()
        {
        }

        public MilkContext(DbContextOptions<MilkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AgeRange> AgeRanges { get; set; }
        public virtual DbSet<BrandMilk> BrandMilks { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DeliveryMan> DeliveryMen { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<ProductItem> ProductItems { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=Milk;User Id=sa;Password=12345;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminID).HasName("PK__Admin__719FE4E89570F68B");

                entity.ToTable("Admin");

                entity.Property(e => e.AdminID)
                    .ValueGeneratedNever()
                    .HasColumnName("AdminID");

                entity.Property(e => e.Username)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.Property(e => e.Password)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<AgeRange>(entity =>
            {
                entity.HasKey(e => e.AgeRangeID).HasName("PK__AgeRange__8CC41DB1F51D4D6D");

                entity.ToTable("AgeRange");

                entity.Property(e => e.AgeRangeID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("AgeRangeID");
                entity.Property(e => e.Baby).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Mama).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ProductItemID).HasColumnName("ProductItemID");

                entity.HasOne(d => d.ProductItem).WithMany(p => p.AgeRanges)
                    .HasForeignKey(d => d.ProductItemID)
                    .HasConstraintName("FK__AgeRange__Produc__4AB81AF0");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<BrandMilk>(entity =>
            {
                entity.HasKey(e => e.BrandMilkID).HasName("PK__BrandMil__03987540F41D4097");

                entity.ToTable("BrandMilk");

                entity.Property(e => e.BrandMilkID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BrandMilkID");
                entity.Property(e => e.BrandName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

                entity.HasOne(d => d.Company).WithMany(p => p.BrandMilks)
                    .HasForeignKey(d => d.CompanyID)
                    .HasConstraintName("FK__BrandMilk__Compa__3C69FB99");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyID).HasName("PK__Company__2D971C4CC3C9BC4A");

                entity.ToTable("Company");

                entity.Property(e => e.CompanyID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CompanyID");
                entity.Property(e => e.CompanyName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CountryID).HasColumnName("CountryID");

                entity.HasOne(d => d.Country).WithMany(p => p.Companies)
                    .HasForeignKey(d => d.CountryID)
                    .HasConstraintName("FK__Company__Country__398D8EEE");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryID).HasName("PK__Country__10D16030");

                entity.ToTable("Country");

                entity.Property(e => e.CountryID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CountryID");
                entity.Property(e => e.CountryName).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID).HasName("PK__Customer__CD65CB85");

                entity.ToTable("Customer");

                entity.Property(e => e.CustomerID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CustomerID");
                entity.Property(e => e.CustomerName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Password).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Phone).HasColumnType("nvarchar(15)").IsRequired();

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<DeliveryMan>(entity =>
            {
                entity.HasKey(e => e.DeliveryManID).HasName("PK__Delivery__498A5B50");

                entity.ToTable("DeliveryMan");

                entity.Property(e => e.DeliveryManID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DeliveryManID");
                entity.Property(e => e.DeliveryName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.DeliveryStatus).HasColumnType("nvarchar(max)");
                entity.Property(e => e.PhoneNumber).HasColumnType("nvarchar(15)").IsRequired();
                entity.Property(e => e.StorageID).HasColumnName("StorageID");
                entity.Property(e => e.StorageName).HasColumnType("nvarchar(max)").IsRequired();

                entity.HasOne(d => d.Storage).WithMany(p => p.DeliveryMen)
                    .HasForeignKey(d => d.StorageID)
                    .HasConstraintName("FK__DeliveryM__Stora__3E52440B");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID).HasName("PK__Order__C3905BCF");

                entity.ToTable("Order");

                entity.Property(e => e.OrderID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OrderID");
                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
                entity.Property(e => e.DeliveryManID).HasColumnName("DeliveryManID");
                entity.Property(e => e.DeliveryName).HasColumnType("nvarchar(max)");
                entity.Property(e => e.DeliveryPhone).HasColumnType("nvarchar(15)");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.ShippingAddress).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PaymentMethod).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.StorageID).HasColumnName("StorageID");
                entity.Property(e => e.Status).HasColumnType("nvarchar(max)").IsRequired();

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK__Order__CustomerI__45F365D3");

                entity.HasOne(d => d.DeliveryMan).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryManID)
                    .HasConstraintName("FK__Order__DeliveryM__46E78A0C");

                entity.HasOne(d => d.Storage).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StorageID)
                    .HasConstraintName("FK__Order__StorageID__47DBAE45");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentID).HasName("PK__Payment__C531D7FC");

                entity.ToTable("Payment");

                entity.Property(e => e.PaymentID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PaymentID");
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.PaymentMethod).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.OrderID).HasColumnName("OrderID");

                entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderID)
                    .HasConstraintName("FK__Payment__OrderID__4CA06362");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailID).HasName("PK__OrderDet__4E64D4B1");

                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OrderDetailID");
                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
                entity.Property(e => e.OrderID).HasColumnName("OrderID");
                entity.Property(e => e.ProductItemID).HasColumnName("ProductItemID");
                entity.Property(e => e.ItemName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Image).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Quantity).HasColumnType("int");
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.OrderStatus).HasColumnType("int");
                entity.Property(e => e.StockQuantity).HasColumnType("int");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderID)
                    .HasConstraintName("FK__OrderDeta__Order__4E88ABD4");

                entity.HasOne(d => d.Customer).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK__OrderDeta__Custo__4D94879B");

                entity.HasOne(d => d.ProductItem).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductItemID)
                    .HasConstraintName("FK__OrderDeta__Produ__4F7CD00D");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<ProductItem>(entity =>
            {
                entity.HasKey(e => e.ProductItemID).HasName("PK__ProductI__2B9B72C9");

                entity.ToTable("ProductItem");

                entity.Property(e => e.ProductItemID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProductItemID");
                entity.Property(e => e.Benefit).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Image1).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Image2).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Image3).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ItemName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Weight).HasColumnType("float");
                entity.Property(e => e.BrandMilkID).HasColumnName("BrandMilkID");
                entity.Property(e => e.Baby).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Mama).HasColumnType("nvarchar(max)");
                entity.Property(e => e.BrandName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CountryName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.CompanyName).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.SoldQuantity).HasColumnType("int");
                entity.Property(e => e.StockQuantity).HasColumnType("int");

                entity.HasOne(d => d.BrandMilk).WithMany(p => p.ProductItems)
                    .HasForeignKey(d => d.BrandMilkID)
                    .HasConstraintName("FK__ProductIt__Brand__3F466844");

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(e => e.StorageID).HasName("PK__Storage__0B419C4C");

                entity.ToTable("Storage");

                entity.Property(e => e.StorageID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("StorageID");
                entity.Property(e => e.StorageName).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.Delete)
                    .IsRequired()
                    .HasDefaultValue(1)
                    .HasColumnName("Delete");
            });
        }
    }
}
