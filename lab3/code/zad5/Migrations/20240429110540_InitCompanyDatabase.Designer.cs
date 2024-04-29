﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using zad5;

#nullable disable

namespace zad5.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20240429110540_InitCompanyDatabase")]
    partial class InitCompanyDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("zad5.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CompanyID");

                    b.ToTable("Companies");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Company");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("zad5.Customer", b =>
                {
                    b.HasBaseType("zad5.Company");

                    b.Property<int>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Discount")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("zad5.Supplier", b =>
                {
                    b.HasBaseType("zad5.Company");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Supplier");
                });
#pragma warning restore 612, 618
        }
    }
}
