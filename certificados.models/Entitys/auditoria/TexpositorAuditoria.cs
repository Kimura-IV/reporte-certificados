using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TEXPOSITOR", Schema = "auditoria")]
    public class TexpositorAuditoria
    {
        [Key]
        [Column("IDEXPOSITOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdExpositor { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        public  string Cedula { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsusarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }

    }
}
