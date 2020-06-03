using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyCountry.Models
{
    public class ICPC2GroupEntity
    {
        [Key, Column(TypeName = "char(36)")]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Incremental key.
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableKey { get; set; }
        [Required, Column(TypeName = "char(36)")]
        public Guid ICPC2Id { get; set; }
        [ForeignKey("ICPC2Id")]
        public ICPC2Entity ICPC2 { get; set; }
        [Required]
        public ICPC2Groups GroupId { get; set; }
        [Required]
        public bool IsActual { get; set; } = true;
        [Required, Column(TypeName = "datetime(0)")]
        public DateTime InsertDate { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime(0)")]
        public DateTime? LastUpdateDate { get; set; }
    }
    public enum ICPC2Groups : byte
    {
        Reason = 1,
        Result,
        Action
    }
}