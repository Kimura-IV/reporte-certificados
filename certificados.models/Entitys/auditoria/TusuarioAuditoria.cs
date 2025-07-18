using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TUSUARIO", Schema = "auditoria")]
    public class TusuarioAuditoria
    {
        [Key]
        [Column("IDUSUARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Column("EMAIL")]
        [StringLength(80)]
        [Required]
        public string? Email { get; set; }

        [Column("CLAVE")]
        [StringLength(100)]
        [Required]
        public string? Clave { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        public string? Cedula { get; set; }

        [Column("ESTADO")]
        [StringLength(3)]
        [Required]
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

        [Column("IDROL")]
        [Required]
        public int IdRol { get; set; }

    }
}
