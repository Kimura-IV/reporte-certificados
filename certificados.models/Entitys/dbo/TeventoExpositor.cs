using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TEVENTOEXPOSITOR", Schema = "dbo")]
    public class TeventoExpositor
    {
        [Key]
        [Column("IDEVENTOEXPOSITOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEventoExpositor { get; set; }

        [Column("IDEVENTO")]
        [Required]
        [ForeignKey("Tevento")]
        public int IdEvento { get; set; }

        [Column("IDEXPOSITOR")]
        [Required]
        [ForeignKey("Texpositor")]
        public int IdExpositor { get; set; }

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

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tevento Tevento { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Texpositor Texpositor { get; set; }


    }
}
