using System;
using HealthyCountry.Models;

namespace HealthyCountry.DataModels
{
    public class ChangeDiagnosisRequest
    {
        public Severity? Severity { get; set; }
        public ClinicalStatus? ClinicalStatus { get; set; }
        public DateTime Date { get; set; }
        public string CodeId { get; set; }
    }
}