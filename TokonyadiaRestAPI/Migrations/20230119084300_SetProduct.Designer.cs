// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TokonyadiaRestAPII.Repositories;

#nullable disable

namespace TokonyadiaRestAPII.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20230119084300_SetProduct")]
    partial class SetProduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("address");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("customer_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("NVarchar(14)")
                        .HasColumnName("phone_number");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("customer", "dbo");
                });

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("Nvarchar(100)")
                        .HasColumnName("description");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("Nvarchar(50)")
                        .HasColumnName("product_name");

                    b.HasKey("Id");

                    b.ToTable("product");
                });

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.ProductPrice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<long>("Price")
                        .HasColumnType("bigint")
                        .HasColumnName("price");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("product_id");

                    b.Property<int>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("store_id");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("product_price");
                });

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("address");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("NVarchar(14)")
                        .HasColumnName("phone_number");

                    b.Property<string>("SiupNumber")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("siup_number");

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnType("NVarchar(100)")
                        .HasColumnName("store_name");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("SiupNumber")
                        .IsUnique();

                    b.ToTable("store", "dbo");
                });

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.ProductPrice", b =>
                {
                    b.HasOne("TokonyadiaRestAPII.Entities.Product", "Product")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TokonyadiaRestAPII.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("TokonyadiaRestAPII.Entities.Product", b =>
                {
                    b.Navigation("ProductPrices");
                });
#pragma warning restore 612, 618
        }
    }
}
