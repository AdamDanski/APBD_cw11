using Microsoft.EntityFrameworkCore;
using zad11.Models;

namespace zad11.Data;

public class DatabaseContext : DbContext
{
   public DbSet<Patient> Patients { get; set; }
   public DbSet<Doctor> Doctors { get; set; }
   public DbSet<Medicament> Medicaments { get; set; }
   public DbSet<Prescription> Prescriptions { get; set; }
   public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.IdPatient);
            e.Property(p => p.FirstName).HasMaxLength(100);
            e.Property(p => p.LastName).HasMaxLength(100);
        });
        modelBuilder.Entity<Doctor>(e =>
        {
            e.HasKey(d => d.IdDoctor);
            e.Property(d => d.FirstName).HasMaxLength(100);
            e.Property(d => d.LastName).HasMaxLength(100);  
            e.Property(d => d.Email).HasMaxLength(100);
        });
        modelBuilder.Entity<Medicament>(e =>
        {
            e.HasKey(m => m.IdMedicament);
            e.Property(m => m.Name).HasMaxLength(100);
            e.Property(m => m.Description).HasMaxLength(500);
            e.Property(m=> m.Type).HasMaxLength(100);
        });
        modelBuilder.Entity<Prescription>(e =>
        {
            e.HasKey(p => p.IdPrescription);
            e.Property(p => p.Date).IsRequired();
            e.Property(p=> p.DueDate).IsRequired();
            
            e.HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p=>p.PatientId);
            
            e.HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorId);
        });

        modelBuilder.Entity<PrescriptionMedicament>(e =>
        {
            e.HasKey(pm => new {pm.IdMedicament, pm.IdPrescription});
            
            e.Property(pm => pm.Description).HasMaxLength(100);
            
            e.HasOne(pm => pm.Prescription)
                .WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdPrescription);
            
            e.HasOne(pm => pm.Medicament)
                .WithMany(m => m.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdMedicament);
        });
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna.nowak@example.com"
            },
            new Doctor
            {
                IdDoctor = 2,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@example.com"
            }
        );
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Name = "Apap",
                Description = "Ból głowy",
                Type = "Tabletka"
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Ibuprom",
                Description = "Ból mięśni",
                Type = "Kapsułka"
            }
        );
        modelBuilder.Entity<Patient>().HasData(
            new Patient
            {
                IdPatient = 1,
                FirstName = "Adam",
                LastName = "Dański",
                DateOfBirth = new DateTime(1990, 5, 20)
            },
            new Patient
            {
                IdPatient = 2,
                FirstName = "Juliusz",
                LastName = "Raczkowski",
                DateOfBirth = new DateTime(1985, 8, 15)
            }
        );
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription
            {
                IdPrescription = 1,
                Date = new DateTime(2024, 6, 1),
                DueDate = new DateTime(2024, 6, 10),
                PatientId = 1,
                DoctorId = 1
            }
        );
        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament
            {
                IdPrescription = 1,
                IdMedicament = 1,
                Dose = 2,
                Description = "Stosować rano i wieczorem"
            },
            new PrescriptionMedicament
            {
                IdPrescription = 1,
                IdMedicament = 2,
                Dose = 1,
                Description = "Po posiłku"
            }
        );


    }
}