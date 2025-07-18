using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TGRUPO", Schema = "dbo")]
    public class Tgrupo
    {
        [Key]
        [Column("IDGRUPO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrupo { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public required string Nombre { get; set; }

        [Column("CANTIDAD")]
        public int Cantidad { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
    }
}
