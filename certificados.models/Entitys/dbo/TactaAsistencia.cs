using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace certificados.models.Entitys.dbo
{
    [Table("TACTAASISTENCIA", Schema = "dbo")]
    public class TactaAsistencia
    {
        [Key]
        [Column("IDASISTENCIA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAsistencia { get; set; }

        [Column("IDEVENTO")]
        [Required]
        [ForeignKey("Tevento")]
        public int IdEvento { get; set; }

        [Column("ACTADOCUMENTO")]
        public byte[]? ActaDocumento { get; set; }

        [Column("FCREACION")]
        [Required]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tevento Tevento { get; set; }

    }
}
