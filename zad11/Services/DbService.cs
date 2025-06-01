using Microsoft.EntityFrameworkCore;
using zad11.Data;
using zad11.DTOs;
using zad11.Models;

namespace zad11.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(PrescriptionCreateDTO dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new Exception("Too many medicaments");
        if (dto.DueDate < dto.Date)
            throw new Exception("Date must be after duedate");
        
        var medicamentIds = dto.Medicaments.Select(m=>m.IdMedicament).ToList();
        var existingMedicament = await _context.Medicaments
            .Where(m=>medicamentIds.Contains(m.IdMedicament))
            .Select(m=>m.IdMedicament)
            .ToListAsync();
        
        if (existingMedicament.Count != medicamentIds.Count)
            throw new Exception("Number of medicaments does not match");

        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            p.FirstName == dto.Patient.FirstName &&
            p.LastName == dto.Patient.LastName &&
            p.DateOfBirth == dto.Patient.DateOfBirth);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                DateOfBirth = dto.Patient.DateOfBirth
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            DoctorId = dto.IdDoctor,
            PatientId = patient.IdPatient,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };

        foreach (var med in dto.Medicaments)
        {
            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Description = med.Description
            });
        }
        
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientDetailsDTO> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p=>p.Prescriptions)
            .ThenInclude(pr=>pr.Doctor)
            .Include(p=>p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm=>pm.Medicament)
            .FirstOrDefaultAsync(p=>p.IdPatient==idPatient);
        
        if (patient == null)
            throw new Exception("Patient not found");

        return new PatientDetailsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDTO
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}