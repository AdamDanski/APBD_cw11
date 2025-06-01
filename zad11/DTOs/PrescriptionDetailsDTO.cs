namespace zad11.DTOs;

public class PrescriptionDetailsDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date {get; set;}
    public DateTime DueDate {get; set;}
    public DoctorDTO Doctor {get; set;}
    public List<MedicamentDetailsDTO> Medicaments {get; set;}
}