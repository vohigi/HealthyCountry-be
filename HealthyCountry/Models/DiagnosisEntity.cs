using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HealthyCountry.Models
{
    public class DiagnosisEntity
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Severity? Severity { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ClinicalStatus? ClinicalStatus { get; set; }
        public DateTime Date { get; set; }
        public ICPC2Entity Code { get; set; }
        public Appointment Appointment { get; set; }
        
    }
    public enum Severity
    {
        Light,
        Middle,
        Heavy
    }
    
    public enum ClinicalStatus
    {
        Active,
        Relapse,
        Remission,
        Cured
    }
}