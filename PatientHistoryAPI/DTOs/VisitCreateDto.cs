namespace PatientHistoryAPI.Dtos
{
    public class VisitCreateDto
    {
        public int? PatientId { get; set; }
        public DateTime? VisitDate { get; set; }
        public int? DoctorId { get; set; }
        public string? Notes { get; set; }

          // Add Diagnoses and Medications to the DTO if not already present
        public List<string> Diagnoses { get; set; } = new List<string>();
        public List<string> Medications { get; set; } = new List<string>();
    }
}
