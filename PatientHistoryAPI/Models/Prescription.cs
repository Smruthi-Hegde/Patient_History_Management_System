using System;
using System.Collections.Generic;

namespace PatientHistoryAPI.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int? VisitId { get; set; }

    public string? Medication { get; set; }

    public string? Dosage { get; set; }

    public string? Duration { get; set; }

    public virtual Visit? Visit { get; set; }
}
