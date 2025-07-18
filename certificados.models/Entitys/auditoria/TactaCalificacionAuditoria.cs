using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TACTACALIFICACION", Schema = "auditoria")]
    public class TactaCalificacionAuditoria
    {
        [Key]
        [Column("IDCALIFICACION")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCalificacion { get; set; }

        [Column("IDEVENTO")]
        [Required]
        public int IdEvento { get; set; }

        [Column("ACTADOCUMENTO")]
        public byte[]? ActaDocumento { get; set; }

        [Column("FCREACION")]
        [Required]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
    }
}
