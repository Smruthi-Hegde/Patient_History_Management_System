using System;
using System.Collections.Generic;

namespace PatientHistoryAPI.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
