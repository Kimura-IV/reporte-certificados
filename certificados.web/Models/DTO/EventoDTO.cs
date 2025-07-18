using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.web.Models.DTO
{
    public class EventoDTO
    {
        public int Idevento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Horas { get; set; }
        public string? Lugar { get; set; }
        public int ConCertificado { get; set; }
        public string? Periodo { get; set; }
        public string? Tematica { get; set; }
        public string? Dominio { get; set; }
        public string? Facilitador { get; set; }
        public string? Estado { get; set; }
        public int IdGrupo { get; set; }
        public int IdModalidad { get; set; }
        public int IdTipoEvento { get; set; }
        public int IdDecanato { get; set; }
        public string? UsuarioIngreso { get; set; }
        public string? UsuarioActualizacion { get; set; }
    }
}
