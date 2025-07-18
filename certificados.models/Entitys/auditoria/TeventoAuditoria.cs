using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TEVENTO", Schema = "auditoria")]
    public class TeventoAuditoria
    {
        [Key]
        [Column("IDEVENTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idevento { get; set; }

        [Column("FECHAINICIO")]
        [Required]
        public DateTime FechaInicio { get; set; }

        [Column("FECHAFIN")]
        [Required]
        public DateTime FechaFin { get; set; }

        [Column("HORAS")]
        public int Horas { get; set; }

        [Column("LUGAR")]
        [StringLength(255)]
        public string? Lugar { get; set; }

        [Column("CONCERTIFICADO")]
        public int ConCertificado { get; set; }

        [Column("PERIODO")]
        [StringLength(255)]
        public string? Periodo { get; set; }

        [Column("TEMATICA")]
        [StringLength(255)]
        public string? Tematica { get; set; }

        [Column("DOMINIO")]
        [StringLength(255)]
        public string? Dominio { get; set; }

        [Column("ESTADO")]
        [StringLength(3)]
        public string? Estado { get; set; }

        [Column("FACILITADOR")]
        [StringLength(15)]
        public string? Facilitador { get; set; }

        [Column("IDGRUPO")]
        [Required]
        public int IdGrupo { get; set; }

        [Column("IDMODALIDAD")]
        [Required]
        public int IdModalidad { get; set; }

        [Column("IDTIPOEVENTO")]
        [Required]
        public int IdTipoEvento { get; set; }

        [Column("IDDECANATO")]
        [Required]
        public int IdDecanato { get; set; }

        [Column("FCREACION")]
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
