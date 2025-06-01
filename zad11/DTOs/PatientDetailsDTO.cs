
namespace zad11.DTOs;

public class PatientDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<PrescriptionDetailsDTO> Prescriptions { get; set; }
}