using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificados.models.Entitys.dbo
{
    [Table("TFORMATOCERTIFICADO", Schema = "dbo")]
    public class TformatoCertificado
    {
        [Key]
        [Column("IDFORMATO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idFormato { get; set; }

        [Column("NOMBREPLANTILLA")]
        [StringLength(500)]
        public string? NombrePlantilla { get; set; }

        [Column("LINEAGRAFICA")]
        public byte[]? LineaGrafica { get; set; }

        [Column("LOGOUG")]
        public byte[]? LogoUG { get; set; }

        [Column("ORIGEN")]
        [StringLength(500)]
        public string? Origen { get; set; }

        [Column("TIPO")]
        [StringLength(100)]
        public string? Tipo { get; set; }

        [Column("LEYENDA")]
        [StringLength(1000)]
        public string? Leyenda { get; set; }

        [Column("QR")]
        public byte[]? Qr { get; set; }

        [Column("CARGOFIRMANTEUNO")]
        [StringLength(500)]
        public string? CargoFirmanteUno { get; set; }

        [Column("NOMBREFIRMANTEUNO")]
        [StringLength(500)]
        public string? NombreFirmanteUno { get; set; }

        [Column("CARGOFIRMANTEDOS")]
        [StringLength(500)]
        public string? CargoFirmanteDos { get; set; }

        [Column("NOMBREFIRMANTEDOS")]
        [StringLength(500)]
        public string? NombreFirmanteDos { get; set; }

        [Column("CARGOFIRMANTETRES")]
        [StringLength(500)]
        public string? CargoFirmanteTres { get; set; }

        [Column("NOMBREFIRMANTETRES")]
        [StringLength(500)]
        public string? NombreFirmanteTres { get; set; }

        [Column("FCREACION")]
        public DateTime FCreacion { get; set; }

        [Column("FMODIFICACION")]
        public DateTime FModificacion { get; set; }

        [Column("USUARIOINGRESO")]
        [StringLength(5)]
        public string? UsuarioIngreso { get; set; }

        [Column("USERMODIFICACION")]
        [StringLength(5)]
        public string? UsuarioActualizacion { get; set; }

    }
}
