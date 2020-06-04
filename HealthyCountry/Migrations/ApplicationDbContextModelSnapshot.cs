﻿// <auto-generated />
using System;
using HealthyCountry.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthyCountry.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HealthyCountry.Models.ICPC2Entity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Considerations")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<string>("Criteria")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<string>("Exclusions")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<string>("Inclusions")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("datetime(0)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("Notes")
                        .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                        .HasMaxLength(2000);

                    b.Property<string>("NumberOnlyCode")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("ShortName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<int>("TableKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("TableKey");

                    b.HasIndex("Name")
                        .HasAnnotation("MySql:FullTextIndex", true);

                    b.HasIndex("NumberOnlyCode");

                    b.HasIndex("Code", "Name", "IsActual", "InsertDate")
                        .IsUnique();

                    b.ToTable("ICPC2Codes");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2GroupEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<byte>("GroupId")
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("ICPC2Id")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("datetime(0)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(0)");

                    b.Property<int>("TableKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("TableKey");

                    b.HasIndex("ICPC2Id");

                    b.HasIndex("GroupId", "ICPC2Id", "IsActual", "InsertDate")
                        .IsUnique();

                    b.ToTable("ICPC2GroupEntity");
                });

            modelBuilder.Entity("HealthyCountry.Models.Organization", b =>
                {
                    b.Property<string>("OrganizationId")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Edrpou")
                        .HasColumnType("varchar(10)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");

                    b.HasData(
                        new
                        {
                            OrganizationId = "org_1",
                            Address = "London 221B Baker Street",
                            Edrpou = "11111111",
                            IsActive = false,
                            Name = "Default Organization"
                        });
                });

            modelBuilder.Entity("HealthyCountry.Models.User", b =>
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

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("OrganizationId")
                        .HasColumnType("varchar(50)");

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

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = "bb9e56b6-2fe4-4e1d-b27e-229cc1a6ce77",
                            BirthDate = new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@gmail.com",
                            FirstName = "Admin",
                            Gender = "MALE",
                            IsActive = false,
                            LastName = "Adminovski",
                            MiddleName = "Adminovich",
                            OrganizationId = "org_1",
                            Password = "$2b$10$AzUz5k0q5TUhAPh.tymyAO7iaiXD8y77EM0m9Q1UNpyH1B9wNYFqS",
                            Phone = "380505680632",
                            Role = "ADMIN",
                            TaxId = "11111111"
                        });
                });

            modelBuilder.Entity("MIS.Models.Appointment", b =>
                {
                    b.Property<string>("AppointmentId")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ActionId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DiagnosisId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("PatientId")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ReasonId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(24)");

                    b.HasKey("AppointmentId")
                        .HasName("PRIMARY");

                    b.HasIndex("ActionId");

                    b.HasIndex("DiagnosisId");

                    b.HasIndex("EmployeeId")
                        .HasName("EmployeeId");

                    b.HasIndex("PatientId")
                        .HasName("UserId");

                    b.HasIndex("ReasonId");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2GroupEntity", b =>
                {
                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "ICPC2")
                        .WithMany("Groups")
                        .HasForeignKey("ICPC2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthyCountry.Models.User", b =>
                {
                    b.HasOne("HealthyCountry.Models.Organization", "Organization")
                        .WithMany("Employees")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("MIS.Models.Appointment", b =>
                {
                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Action")
                        .WithMany("AppointmentActions")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Diagnosis")
                        .WithMany("AppointmentDiagnosis")
                        .HasForeignKey("DiagnosisId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HealthyCountry.Models.User", "Employee")
                        .WithMany("AppointmentsAsDoctor")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HealthyCountry.Models.User", "Patient")
                        .WithMany("AppointmentsAsPatient")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Reason")
                        .WithMany("AppointmentReasons")
                        .HasForeignKey("ReasonId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
