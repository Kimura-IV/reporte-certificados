using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TTIPOEVENTO", Schema = "dbo")]
    public class TtipoEvento
    {
        [Key]
        [Column("IDTIPOEVENTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idtipoevento { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public string? Nombre { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(100)]
        [Required]
        public string? Descripcion { get; set; }

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
