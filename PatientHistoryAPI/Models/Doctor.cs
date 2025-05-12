using System;
using System.Collections.Generic;

namespace PatientHistoryAPI.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Specialization { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
