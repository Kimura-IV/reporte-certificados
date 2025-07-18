using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TCICLO", Schema = "auditoria")]
    public class TcicloAuditoria

    {
        [Key]
        [Column("IDCICLO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCiclo { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        [Required]
        public string? Nombre { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(255)]
        [Required]
        public string? Descripcion { get; set; }

        [Column("FCREACION")]
        public DateTime Fcreacion { get; set; }

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
