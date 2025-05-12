namespace PatientHistoryAPI.DTOs
{
    public class PatientCreateDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string ContactNumber { get; set; }
        public required string Address { get; set; }
    }
}
