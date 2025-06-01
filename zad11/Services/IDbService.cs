using zad11.DTOs;

namespace zad11.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(PrescriptionCreateDTO dto);
    Task<PatientDetailsDTO> GetPatientDetailsAsync(int idPatient);
}