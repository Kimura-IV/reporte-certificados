using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TEVENTOEXPOSITOR", Schema = "auditoria")]
    public class TeventoExpositorAuditoria
    {
        [Key]
        [Column("IDEVENTOEXPOSITOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEventoExpositor { get; set; }

        [Column("IDEVENTO")]
        [Required]
        public int IdEvento { get; set; }

        [Column("IDEXPOSITOR")]
        [Required]
        public int IdExpositor { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsusarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
    }
}
