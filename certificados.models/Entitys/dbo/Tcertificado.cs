using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo

{
    [Table("TCERFIFICADO", Schema = "dbo")]
    public class Tcertificado
    {
        [Key]
        [Column("IDCERTIFICADO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCertificado { get; set; }

        [Column("TITULO")]
        [StringLength(200)]
        [Required]
        public required string Titulo { get; set; }

        [Column("IMAGEN")]
        public byte[]? Imagen { get; set; }

        [Column("IDFORMATO")]
        [Required]
        [ForeignKey("TformatoCertificado")]
        public int IdFormato { get; set; }

        [Column("TIPO")]
        [StringLength(100)]
        public string? Tipo { get; set; }

        [Column("ESTADO")]
        [Required]
        public bool Estado { get; set; }

        [Column("FCREACION")]
        [Required]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]

        public required virtual TformatoCertificado TformatoCertificado { get; set; }

    }
}
