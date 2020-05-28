﻿// <auto-generated />
using System;
using HealthyCountry.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthyCountry.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200503110938_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MIS.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasColumnName("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Gender")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .HasColumnName("Password")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .HasColumnName("Phone")
                        .HasColumnType("varchar(12)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("Role")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("TaxId")
                        .HasColumnName("TaxId")
                        .HasColumnType("varchar(10)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
