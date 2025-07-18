using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TPERSONA", Schema = "auditoria")]
    public class TpersonaAuditoria
    {
        [Key]
        [Column("CEDULA")]
        [StringLength(10)]
        public string? Cedula { get; set; }

        [Column("NOMBRES")]
        [StringLength(80)]
        [Required]
        public string? Nombres { get; set; }

        [Column("APELLIDOS")]
        [StringLength(80)]
        [Required]
        public string? Apellidos { get; set; }

        [Column("EDAD")]
        public int Edad { get; set; }

        [Column("GENERO")]
        [StringLength(1)]
        public string? Genero { get; set; }

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
