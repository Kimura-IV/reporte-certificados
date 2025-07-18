using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.models.Entitys.auditoria
{

    [Table("TROL", Schema = "auditoria")]
    public class TrolAuditoria
    {
        [Key]
        [Column("IDROL")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }

        [Column("FHISTORIA")]
        public DateTime FHistoria { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public string? Nombre { get; set; }

        [Column("OBSERVACION")]
        [StringLength(200)]
        [Required]
        public string? Observacion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }

        [Column("FCREACION")]
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
