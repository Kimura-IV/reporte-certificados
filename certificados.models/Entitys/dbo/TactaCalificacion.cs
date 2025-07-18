using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TACTACALIFICACION", Schema = "dbo")]
    public class TactaCalificacion
    {
        [Key]
        [Column("IDCALIFICACION")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCalificacion { get; set; }

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
