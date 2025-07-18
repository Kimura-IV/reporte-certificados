using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TDOCENTE", Schema = "auditoria")]
    public class TdocenteAuditoria
    {
        [Key]
        [Column("CODIGODOCENTE")]
        [StringLength(10)]
        [Required]
        public  string CodigoDocente { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        public  string Cedula { get; set; }

        [Column("TITULO")]
        [StringLength(200)]
        public string? Titulo { get; set; }

        [Column("FACULTAD")]
        [StringLength(200)]
        public string? Facultad { get; set; }

        [Column("CARRERA")]
        [StringLength(200)]
        public string? Carrera { get; set; }

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

        [Column("IDESTADODOCENTE")]
        public int IdEstadoDocente { get; set; }

    }
}
