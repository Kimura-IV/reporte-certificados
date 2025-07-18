using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TLOG", Schema = "dbo")]
    public class Tlog
    {
        [Key]
        [Required]
        [Column("IDLOG")]
        public int IdLog { get; set; }

        [Column("MESSAGE")]
        public string? Message { get; set; }

        [Column("STACKTRACE")]
        public string? StackTrace { get; set; }

        [Column("ERRORTYPE")]
        public string? ErrorType { get; set; }

        [Column("TIMESTAMP")]
        public string? Timestamp { get; set; }  
    }
}
