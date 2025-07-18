using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TGRUPOPERSONA", Schema = "dbo")]
    public class TgrupoPersona
    {
        [Key]
        [Column("IDGRUPOPERSONA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrupoPersona { get; set; }

        [Column("IDGRUPO")]
        [Required]
        [ForeignKey("Tgrupo")]
        public int IdGrupo { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        [ForeignKey("Tpersona")]
        public string? Cedula { get; set; }

        [Column("ESTADO")]
        [StringLength(10)]
        public string? Estado { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsusarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tpersona Tpersona { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tgrupo Tgrupo { get; set; }



    }
}
