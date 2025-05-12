using System;
using System.Collections.Generic;

namespace PatientHistoryAPI.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int? PatientId { get; set; }

    public DateTime? VisitDate { get; set; }

    public int? DoctorId { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
