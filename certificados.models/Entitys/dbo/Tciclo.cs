using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TCICLO", Schema = "dbo")]
    public class Tciclo
    {
        [Key]
        [Column("IDCICLO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCiclo { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public required string Nombre { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(255)]
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
        public string? UserModificacion { get; set; }
    }
}
