namespace PatientHistoryAPI.DTOs
{
    public class PrescriptionCreateDto
    {
        public int? VisitId { get; set; }
        public string Medication { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int PrescriptionId { get; set; } // Adding PrescriptionId for the Update scenario
    }
}
