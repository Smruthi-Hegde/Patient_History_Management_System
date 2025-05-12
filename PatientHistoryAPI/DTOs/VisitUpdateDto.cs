namespace PatientHistoryAPI.Dtos
{
    public class VisitUpdateDto
    {
        public int VisitId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? VisitDate { get; set; }
        public int? DoctorId { get; set; }
        public string? Notes { get; set; }
      public List<string> Diagnoses { get; set; } = new List<string>();
        public List<string> Medications { get; set; } = new List<string>();
    }
}
