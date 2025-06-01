namespace zad11.DTOs;

public class PrescriptionCreateDTO
{
    public PatientDTO Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentDTO> Medicaments { get; set; }
}