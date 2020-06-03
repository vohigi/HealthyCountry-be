using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MIS.Models;

namespace HealthyCountry.Models
{
    public class ICPC2Entity
    {
        [Key, Column(TypeName = "char(36)")]
            public string Id { get; set; }
            /// <summary>
            /// Incremental key.
            /// </summary>
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
            [Required, Column(TypeName = "datetime(0)")]
            public DateTime InsertDate { get; set; } = DateTime.Now;
            [Column(TypeName = "datetime(0)")]
            public DateTime? LastUpdateDate { get; set; }
            [Required, StringLength(25)]    
            public string NumberOnlyCode { get; set; }
            public List<ICPC2GroupEntity> Groups { get; set; } = new List<ICPC2GroupEntity>();
            public List<Appointment> AppointmentReasons { get; set; } = new List<Appointment>();
            public List<Appointment> AppointmentDiagnosis { get; set; } = new List<Appointment>();
            public List<Appointment> AppointmentActions { get; set; } = new List<Appointment>();
    }
}