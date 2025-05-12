namespace PatientHistoryAPI.DTOs
{
    public class DiagnosisCreateDto
    {
        public int VisitId { get; set; }  // ID of the related Visit
        public string Diagnosis1 { get; set; }  // Diagnosis description
        public string Description { get; set; }  // Additional details about the diagnosis
    }
}
