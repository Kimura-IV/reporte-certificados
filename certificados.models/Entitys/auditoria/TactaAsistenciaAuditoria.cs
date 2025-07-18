using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.models.Entitys.auditoria
{
    [Table("TACTAASISTENCIA", Schema = "auditoria")]
    public class TactaAsistenciaAuditoria
    {
        [Key]
        [Column("IDASISTENCIA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAsistencia { get; set; }

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
