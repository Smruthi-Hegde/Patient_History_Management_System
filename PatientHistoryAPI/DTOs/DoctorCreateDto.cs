namespace PatientHistoryAPI.DTOs
{
    public class DoctorCreateDto
    {
        public int DoctorId { get; set; }  // This is optional during creation, but necessary for update
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
    }
}
