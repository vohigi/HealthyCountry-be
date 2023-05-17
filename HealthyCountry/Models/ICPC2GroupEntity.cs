using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyCountry.Models
{
    public class ICPC2GroupEntity
    {
        public Guid Id { get; set; } 
        /// <summary>
        /// Incremental key.
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableKey { get; set; }
        public Guid ICPC2Id { get; set; }
        public ICPC2Entity ICPC2 { get; set; }
        [Required]
        public ICPC2Groups GroupId { get; set; }
        [Required]
        public bool IsActual { get; set; } = true;
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdateDate { get; set; }
    }
    public enum ICPC2Groups : byte
    {
        Reason = 1,
        Result,
        Action
    }
}