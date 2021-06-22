using System;
using HealthyCountry.Models;
using Microsoft.EntityFrameworkCore;
using MIS.Models;

namespace HealthyCountry.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        //public virtual DbSet<Declarations> Declarations { get; set; }
        // public virtual DbSet<Employees> Employees { get; set; }
        //public virtual DbSet<Msps> Msps { get; set; }

        //public virtual DbSet<Appointments> Appointments { get; set; }
        //public DbSet<IdentityRole> IdentityRole { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Gender)
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.LastName)
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.MiddleName)
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.TaxId)
                    .HasColumnName("TaxId")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Phone)
                    .HasColumnName("Phone")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.Password)
                    .HasColumnName("Password")
                    .HasColumnType("varchar(255)");
                entity.Property(e => e.Role)
                    .HasColumnName("Role")
                    .HasColumnType("varchar(30)");
            });
            modelBuilder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Employees);

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.NewGuid().ToString(),
                BirthDate = new DateTime(1000, 10, 10),
                Email = "admin@gmail.com",
                FirstName = "Admin",
                Gender = "MALE",
                LastName = "Adminovski",
                MiddleName = "Adminovich",
                Password = BCrypt.Net.BCrypt.HashPassword("Sasha280920"),
                Role = UserRoles.ADMIN,
                Phone = "380505680632",
                TaxId = "11111111",
                OrganizationId = "org_1"
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.OrganizationId)
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.Edrpou)
                    .HasColumnType("varchar(10)");
                entity.Property(e => e.Name)
                    .HasColumnType("varchar(255)");
                entity.Property(e => e.Address)
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Organization>().HasData(new Organization
            {
                OrganizationId = "org_1",
                Name = "Default Organization",
                Edrpou = "11111111",
                Address = "London 221B Baker Street",
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId)
                    .HasName("PRIMARY");

                entity.ToTable("appointments");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("EmployeeId");

                entity.HasIndex(e => e.PatientId)
                    .HasName("UserId");

                entity.Property(e => e.AppointmentId).HasColumnType("varchar(250)");

                entity.Property(e => e.DateTime).HasColumnType("datetime(6)");

                entity.Property(e => e.EmployeeId).HasColumnType("varchar(250)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("varchar(24)");

                entity.Property(e => e.PatientId).HasColumnType("varchar(250)");

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
            
            modelBuilder.Entity<ICPC2Entity>()
                .HasAlternateKey(d => d.TableKey);
            modelBuilder.Entity<ICPC2Entity>()
                .HasIndex(c => new {c.Code, c.Name, c.IsActual, c.InsertDate}).IsUnique();
            modelBuilder.Entity<ICPC2Entity>()
                .HasIndex(c => c.Name).ForMySqlIsFullText();
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