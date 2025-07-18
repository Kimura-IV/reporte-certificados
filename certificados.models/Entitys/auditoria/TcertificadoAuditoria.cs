using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria

{
    [Table("TCERTIFICADO", Schema = "auditoria")]
    public class TcertificadoAuditoria
    {
        [Key]
        [Column("IDCERTIFICADO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCertificado { get; set; }

        [Column("TITULO")]
        [StringLength(200)]
        [Required]
        public   string Titulo { get; set; }

        [Column("IMAGEN")]
        public byte[]? Imagen { get; set; }

        [Column("IDEVENTO")]
        [Required]
        public int IdEvento { get; set; }

        [Column("IDFORMATO")]
        [Required]
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

        [Column("USUARIOACTUALIZACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }
    }
}
