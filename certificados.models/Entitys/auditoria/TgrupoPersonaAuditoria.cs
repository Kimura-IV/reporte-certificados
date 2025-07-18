using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TGRUPOPERSONA", Schema = "auditoria")]
    public class TgrupoPersonaAuditoria
    {
        [Key]
        [Column("IDGRUPOPERSONA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrupoPersona { get; set; }

        [Column("IDGRUPO")]
        [Required]
        public int IdGrupo { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        public string? Cedula { get; set; }

        [Column("ESTADO")]
        [StringLength(10)]
        public string? Estado { get; set; }

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
