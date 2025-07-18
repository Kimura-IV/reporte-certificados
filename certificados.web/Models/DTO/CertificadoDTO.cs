using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.web.Models.DTO
{
    public class CertificadoDTO
    {
        public int IdCertificado { get; set; }

        public required string Titulo { get; set; }

        public byte[]? Imagen { get; set; }

        public int IdEvento { get; set; }

        public int IdFormato { get; set; }

        public string? Tipo { get; set; }

        public bool Estado { get; set; }

        public string? UsuarioIngreso { get; set; }
        public string? UsuarioActualizacion { get; set; }
    }
}
