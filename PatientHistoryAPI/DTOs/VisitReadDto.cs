namespace PatientHistoryAPI.Dtos
{
    public class VisitReadDto
    {
        public int VisitId { get; set; }
        public DateTime? VisitDate { get; set; }
        public string? Notes { get; set; }

        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }

        public List<string> Diagnoses { get; set; } = new();
        public List<string> Medications { get; set; } = new();
    }
}
