using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TDECANATO", Schema = "auditoria")]
    public class TdecanatoAuditoria
    {
        [Key]
        [Column("IDDECANATO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDecanato { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public string? Nombre { get; set; }

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
