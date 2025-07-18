using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace certificados.models.Entitys.dbo
{
    [Table("TDOCENTE", Schema = "dbo")]
    public class Tdocente
    {
        [Key]
        [Column("CODIGODOCENTE")]
        [StringLength(10)]
        [Required]
        public required string CodigoDocente { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        [ForeignKey("Tpersona")]
        public string? Cedula { get; set; }

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
        [Required]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("IDESTADO")]
        [ForeignKey("TestadoDocente")]
        [Range(0, int.MaxValue)]
        public int IdEstado { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(5)]
        public string? UserModificacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual TestadoDocente TestadoDocente { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tpersona Tpersona { get; set; }

    }
}
