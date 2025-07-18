using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TUSUARIO", Schema = "dbo")]
    public class Tusuario
    {
        [Key]
        [Column("IDUSUARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }

        [Column("EMAIL")]
        [StringLength(20)]
        [Required]
        public string? Email { get; set; }

        [Column("CLAVE")]
        [StringLength(100)]
        [Required]
        public string? Clave { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        [ForeignKey("Tpersona")]
        public string? Cedula { get; set; }

        [Column("ESTADO")]
        [StringLength(3)]
        [Required]
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
        public string? UsuarioModificacion { get; set; }

        [Column("IDROL")]
        [Required]
        [ForeignKey("Trol")]
        public int IdRol { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tpersona Tpersona { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Trol Trol { get; set; }


    }
}
