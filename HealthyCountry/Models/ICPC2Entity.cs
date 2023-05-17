using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace HealthyCountry.Models
{
    public class ICPC2Entity
    {
        public Guid Id { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableKey { get; set; }
        [Required, StringLength(25)]
        public string Code { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string ShortName { get; set; }
        [StringLength(2000)]
        public string Inclusions { get; set; }
        [StringLength(2000)]
        public string Exclusions { get; set; }
        [StringLength(2000)]
        public string Criteria { get; set; }
        [StringLength(2000)]
        public string Considerations { get; set; }
        [StringLength(2000)]
        public string Notes { get; set; }
        [Required]
        public bool IsActual { get; set; } = true;
        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdateDate { get; set; }
        [Required, StringLength(25)]    
        public string NumberOnlyCode { get; set; }
        [JsonIgnore]
        public NpgsqlTsVector SearchVector { get; set; }
        public List<ICPC2GroupEntity> Groups { get; set; } = new();
        public List<AppointmentToActionLink> AppointmentActions { get; set; } = new();
        public List<AppointmentToReasonLink> AppointmentReasons { get; set; } = new();
        public List<DiagnosisEntity> AppointmentDiagnosis { get; set; } = new();
    }
}