using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace certificados.web.Models.DTO
{
    public class ExpositorDTO
    {
        public int? IdExpositor { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "La cédula debe tener 10 caracteres.")]
        public string Cedula { get; set; }

        [StringLength(5, ErrorMessage = "El usuario de ingreso debe tener máximo 5 caracteres.")]
        public string? UsuarioIngreso { get; set; }

        [StringLength(5, ErrorMessage = "El usuario de actualización debe tener máximo 5 caracteres.")]
        public string? UsuarioActualizacion { get; set; }

    }
}
