﻿// <auto-generated />
using System;
using HealthyCountry.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace HealthyCountry.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HealthyCountry.Models.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DiagnosisId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PatientId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DiagnosisId")
                        .IsUnique();

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HealthyCountry.Models.AppointmentToActionLink", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CodingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasKey("AppointmentId", "CodingId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("CodingId");

                    b.ToTable("AppointmentsToActionLinks");
                });

            modelBuilder.Entity("HealthyCountry.Models.AppointmentToReasonLink", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CodingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasKey("AppointmentId", "CodingId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("CodingId");

                    b.ToTable("AppointmentsToReasonLinks");
                });

            modelBuilder.Entity("HealthyCountry.Models.DiagnosisEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("ClinicalStatus")
                        .HasColumnType("integer");

                    b.Property<Guid?>("CodeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Severity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CodeId");

                    b.ToTable("Diagnoses");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2Entity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<string>("Considerations")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("Criteria")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("Exclusions")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("Inclusions")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Notes")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("NumberOnlyCode")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.Property<string>("ShortName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("TableKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TableKey"));

                    b.HasKey("Id");

                    b.HasAlternateKey("TableKey");

                    b.HasIndex("Name");

                    b.HasIndex("NumberOnlyCode");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.HasIndex("Code", "Name", "IsActual", "InsertDate")
                        .IsUnique();

                    b.ToTable("ICPC2Codes");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2GroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte>("GroupId")
                        .HasColumnType("smallint");

                    b.Property<Guid>("ICPC2Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TableKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TableKey"));

                    b.HasKey("Id");

                    b.HasAlternateKey("TableKey");

                    b.HasIndex("ICPC2Id");

                    b.HasIndex("GroupId", "ICPC2Id", "IsActual", "InsertDate")
                        .IsUnique();

                    b.ToTable("ICPC2GroupEntity");
                });

            modelBuilder.Entity("HealthyCountry.Models.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Edrpou")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("650d70c4-5136-4c13-9a60-aa3aebae8ea5"),
                            Address = "London 221B Baker Street",
                            Edrpou = "11111111",
                            IsActive = false,
                            Name = "Default Organization"
                        });
                });

            modelBuilder.Entity("HealthyCountry.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("Specialization")
                        .HasColumnType("integer");

                    b.Property<string>("TaxId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("45497320-bf4f-4850-bb44-fa55b68d1618"),
                            BirthDate = new DateTime(2000, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@gmail.com",
                            FirstName = "Admin",
                            Gender = "MALE",
                            IsActive = false,
                            LastName = "Adminovski",
                            MiddleName = "Adminovich",
                            OrganizationId = new Guid("650d70c4-5136-4c13-9a60-aa3aebae8ea5"),
                            Password = "$2b$10$hTl7cXE2GTVkL3.ppn9hSOgivwKCueNoLz93pFEzqLm9yheC4OfgS",
                            Phone = "380505680632",
                            Role = 3,
                            Specialization = 0,
                            TaxId = "11111111"
                        });
                });

            modelBuilder.Entity("HealthyCountry.Models.Appointment", b =>
                {
                    b.HasOne("HealthyCountry.Models.DiagnosisEntity", "Diagnosis")
                        .WithOne("Appointment")
                        .HasForeignKey("HealthyCountry.Models.Appointment", "DiagnosisId");

                    b.HasOne("HealthyCountry.Models.User", "Employee")
                        .WithMany("AppointmentsAsDoctor")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HealthyCountry.Models.User", "Patient")
                        .WithMany("AppointmentsAsPatient")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Diagnosis");

                    b.Navigation("Employee");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HealthyCountry.Models.AppointmentToActionLink", b =>
                {
                    b.HasOne("HealthyCountry.Models.Appointment", "Appointment")
                        .WithMany("Actions")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Coding")
                        .WithMany("AppointmentActions")
                        .HasForeignKey("CodingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Coding");
                });

            modelBuilder.Entity("HealthyCountry.Models.AppointmentToReasonLink", b =>
                {
                    b.HasOne("HealthyCountry.Models.Appointment", "Appointment")
                        .WithMany("Reasons")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Coding")
                        .WithMany("AppointmentReasons")
                        .HasForeignKey("CodingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Coding");
                });

            modelBuilder.Entity("HealthyCountry.Models.DiagnosisEntity", b =>
                {
                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "Code")
                        .WithMany("AppointmentDiagnosis")
                        .HasForeignKey("CodeId");

                    b.Navigation("Code");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2GroupEntity", b =>
                {
                    b.HasOne("HealthyCountry.Models.ICPC2Entity", "ICPC2")
                        .WithMany("Groups")
                        .HasForeignKey("ICPC2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ICPC2");
                });

            modelBuilder.Entity("HealthyCountry.Models.User", b =>
                {
                    b.HasOne("HealthyCountry.Models.Organization", "Organization")
                        .WithMany("Employees")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("HealthyCountry.Models.Appointment", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("Reasons");
                });

            modelBuilder.Entity("HealthyCountry.Models.DiagnosisEntity", b =>
                {
                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("HealthyCountry.Models.ICPC2Entity", b =>
                {
                    b.Navigation("AppointmentActions");

                    b.Navigation("AppointmentDiagnosis");

                    b.Navigation("AppointmentReasons");

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("HealthyCountry.Models.Organization", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("HealthyCountry.Models.User", b =>
                {
                    b.Navigation("AppointmentsAsDoctor");

                    b.Navigation("AppointmentsAsPatient");
                });
#pragma warning restore 612, 618
        }
    }
}
