using System;
using System.Collections.Generic;

namespace PatientHistoryAPI.Models;

public partial class Diagnosis
{
    public int DiagnosisId { get; set; }

    public int? VisitId { get; set; }

    public string? Diagnosis1 { get; set; }

    public string? Description { get; set; }

    public virtual Visit? Visit { get; set; }

}
