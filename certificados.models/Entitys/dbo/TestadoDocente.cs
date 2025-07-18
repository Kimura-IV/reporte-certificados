using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.models.Entitys.dbo
{
    [Table("TESTADODOCENTE", Schema = "dbo")]
    public class TestadoDocente
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
