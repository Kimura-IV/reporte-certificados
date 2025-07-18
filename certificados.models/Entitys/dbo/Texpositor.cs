using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TEXPOSITOR", Schema = "dbo")]
    public class Texpositor
    {
        [Key]
        [Column("IDEXPOSITOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdExpositor { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        [ForeignKey("Tpersona")]
        public required string Cedula { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsusarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tpersona Tpersona { get; set; }

    }
}
