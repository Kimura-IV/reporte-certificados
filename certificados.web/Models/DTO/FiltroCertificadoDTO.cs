using ZXing;

namespace certificados.web.Models.DTO
{
    public class FiltroCertificadoDTO
    {
        public DateTime? Emision { get; set; }
        public int Plantilla {get;set;}
        public string? Firmante {get;set;}
        public string? Tipo {get;set;}
        public string? Creador {get;set;}
        public bool? Estado {get;set;}
    }
}
