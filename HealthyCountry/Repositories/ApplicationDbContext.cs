using System;
using HealthyCountry.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyCountry.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DiagnosisEntity> Diagnoses { get; set; }
        public DbSet<AppointmentToActionLink> AppointmentsToActionLinks { get; set; }
        public DbSet<AppointmentToReasonLink> AppointmentsToReasonLinks { get; set; }
        public DbSet<ICPC2Entity> ICPC2Codes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Employees);
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("45497320-bf4f-4850-bb44-fa55b68d1618"),
                BirthDate = new DateTime(2000, 09, 28, 0,0,0,DateTimeKind.Utc),
                Email = "admin@gmail.com",
                FirstName = "Admin",
                Gender = "MALE",
                LastName = "Adminovski",
                MiddleName = "Adminovich",
                Password = BCrypt.Net.BCrypt.HashPassword("Sasha280920"),
                Role = UserRoles.ADMIN,
                Phone = "380505680632",
                TaxId = "11111111",
                OrganizationId = Guid.Parse("650d70c4-5136-4c13-9a60-aa3aebae8ea5")
            });
            modelBuilder.Entity<Organization>().HasData(new Organization
            {
                Id = Guid.Parse("650d70c4-5136-4c13-9a60-aa3aebae8ea5"),
                Name = "Default Organization",
                Edrpou = "11111111",
                Address = "London 221B Baker Street",
            });
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EmployeeId);
                entity.HasIndex(e => e.PatientId);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.AppointmentsAsDoctor)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.AppointmentsAsPatient)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(d => d.Diagnosis)
                    .WithOne(p => p.Appointment)
                    .HasForeignKey<Appointment>(x=>x.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppointmentToActionLink>(entity =>
            {
                entity.HasKey(x => new {x.AppointmentId, x.CodingId});
                entity.HasAlternateKey(x => x.Id);
                entity.HasOne(x => x.Appointment)
                    .WithMany(x => x.Actions)
                    .HasForeignKey(x => x.AppointmentId);
                entity.HasOne(x => x.Coding)
                    .WithMany(x => x.AppointmentActions)
                    .HasForeignKey(x => x.CodingId);
            });
            modelBuilder.Entity<AppointmentToReasonLink>(entity =>
            {
                entity.HasKey(x => new {x.AppointmentId, x.CodingId});
                entity.HasAlternateKey(x => x.Id);
                entity.HasOne(x => x.Appointment)
                    .WithMany(x => x.Reasons)
                    .HasForeignKey(x => x.AppointmentId);
                entity.HasOne(x => x.Coding)
                    .WithMany(x => x.AppointmentReasons)
                    .HasForeignKey(x => x.CodingId);
            });
            modelBuilder.Entity<ICPC2Entity>().HasIndex(f => f.SearchVector)
                .HasMethod("GIN");
            modelBuilder.Entity<ICPC2Entity>()
                .HasAlternateKey(d => d.TableKey);
            modelBuilder.Entity<ICPC2Entity>()
                .HasIndex(c => new {c.Code, c.Name, c.IsActual, c.InsertDate}).IsUnique();
            modelBuilder.Entity<ICPC2Entity>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<ICPC2Entity>()
                .HasIndex(c => c.NumberOnlyCode);
            modelBuilder.Entity<ICPC2GroupEntity>()
                .HasAlternateKey(d => d.TableKey);
            modelBuilder.Entity<ICPC2GroupEntity>()
                .HasIndex(c => new {c.GroupId, c.ICPC2Id, c.IsActual, c.InsertDate}).IsUnique();
            modelBuilder.Entity<ICPC2GroupEntity>()
                .HasOne(sc => sc.ICPC2)
                .WithMany(c => c.Groups)
                .HasForeignKey(sc => sc.ICPC2Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}