using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.auditoria
{
    [Table("TESTADODOCENTE", Schema = "auditoria")]
    public class TestadoDocenteAuditoria
    {
        [Key]
        [Column("IDESTADO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstado { get; set; }

        [Column("NOMBRE")]
        [StringLength(100)]
        public required string Nombre { get; set; }
    }
}
