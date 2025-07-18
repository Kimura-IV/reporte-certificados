using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.web.Models.DTO
{
    public class DocenteDTO
    {
        public required string CodigoDocente { get; set; }

        public string? Cedula { get; set; }

        public string? Titulo { get; set; }

        public string? Facultad { get; set; }

        public string? Carrera { get; set; }

        public int IdEstado { get; set; }

        public string? UsuarioIngreso { get; set; }

        public string? UsuarioModifacion { get; set; }
    }
}
