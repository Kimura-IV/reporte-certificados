using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TPERSONA", Schema = "dbo")]
    public class Tpersona
    {
        [Key]
        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        public string? Cedula { get; set; }

        [Column("NOMBRES")]
        [StringLength(30)]
        [Required]
        public string? Nombres { get; set; }

        [Column("APELLIDOS")]
        [StringLength(30)]
        [Required]
        public string? Apellidos { get; set; }

        [Column("EDAD")]
        public int Edad { get; set; }

        [Column("GENERO")]
        [StringLength(1)]
        public string? Genero { get; set; }

        [Column("FCREACION")]
        public DateTime FechaCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FechaModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(100)]
        public string? UsuarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }
    }
}