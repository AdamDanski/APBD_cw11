using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zad11.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime DueDate { get; set; }
    
    [ForeignKey(nameof(Patient))]
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    [ForeignKey(nameof(Doctor))]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
}