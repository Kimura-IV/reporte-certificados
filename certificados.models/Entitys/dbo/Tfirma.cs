using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TFIRMA", Schema = "dbo")]
    public class Tfirma
    {
        [Key]
        [Column("IDFIRMA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFirma { get; set; }

        [Column("CEDULA")]
        [StringLength(10)]
        [Required]
        [ForeignKey("Tpersona")]
        public required string Cedula { get; set; }

        [Column("FCADUCIDAD")]
        public DateTime FCaducidad { get; set; }

        [Column("PASSWORD")]
        [StringLength(100)]
        [Required]
        public string? Password { get; set; }

        [Column("FIRMA")]
        public string? Firma { get; set; }

        [Column("CARGO")]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Column("FCREACION")]
        public DateTime Fcreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual Tpersona Tpersona { get; set; }

    }
}
