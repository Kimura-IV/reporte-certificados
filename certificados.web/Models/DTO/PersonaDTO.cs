using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace certificados.web.Models.DTO
{
    public class PersonaDTO
    {
        [JsonProperty("cedula")]
        //Crear persona
     
        [StringLength(10, ErrorMessage = "La cédula no puede tener más de 10 caracteres.")]
        public   string Cedula { get; set; }
        [JsonProperty("nombres")]
     
        [StringLength(35, ErrorMessage = "Los nombres no pueden tener más de 30 caracteres.")]
        public   string Nombres { get; set; }

        [JsonProperty("apellidos")]
     
        [StringLength(30, ErrorMessage = "Los apellidos no pueden tener más de 30 caracteres.")]
        public   string Apellidos { get; set; }

        [JsonProperty("edad")]  
     
        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años.")]
        public int Edad { get; set; }

        [JsonProperty("genero")]
     
        [StringLength(1, ErrorMessage = "El género debe ser un carácter.")]
        public   string Genero { get; set; }

        [JsonProperty("usuarioIngreso")]
     
        [StringLength(100, ErrorMessage = "El usuario que realiza el ingreso no puede exceder los 100 caracteres.")]
        public   string UsuarioIngreso { get; set; }

        [JsonProperty("usuarioActualizacion")]
        [StringLength(100, ErrorMessage = "El usuario que realiza el ingreso no puede exceder los 100 caracteres.")]
        public   string UsuarioActualizacion { get; set; }

        [JsonProperty("email")]
     
        [StringLength(100, ErrorMessage = "El usuario que realiza el ingreso no puede exceder los 100 caracteres.")]
        public   string email { get; set; }

        [JsonProperty("clave")]
     
        [StringLength(100, ErrorMessage = "El usuario que realiza el ingreso no puede exceder los 100 caracteres.")]
        public   string clave { get; set; }

        [JsonProperty("idRol")]
     
        public int idRol { get; set; }

        [JsonProperty("estado")]
        [StringLength(3, ErrorMessage = "El estado que realiza el ingreso no puede exceder los 3 caracteres.")]
        public string? estado { get; set; }
    }
}
