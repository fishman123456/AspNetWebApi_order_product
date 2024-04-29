﻿// <auto-generated />
using System;
using AspNetWebApi_order_product.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspNetWebApi_order_product.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240429173050_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdOrder_f");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Client")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ClientOrder_f");

                    b.Property<DateTime>("CreatationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatationDate");

                    b.Property<string>("СodeOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("СodeOrder_f");

                    b.HasKey("Id");

                    b.HasIndex("СodeOrder")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.OrderProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdOrderProduct_f");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("QuantityOrderProduct_f");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdProduct_f");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("PriceProduct_f");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("QuantityProduct_f");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TitleProduct_f");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.OrderProduct", b =>
                {
                    b.HasOne("AspNetWebApi_order_product.Entity.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId");

                    b.HasOne("AspNetWebApi_order_product.Entity.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("AspNetWebApi_order_product.Entity.Product", b =>
                {
                    b.Navigation("OrderProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
