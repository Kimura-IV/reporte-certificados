using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/modalidad/")]
    public class ModalidadController:Controller
    {
        private readonly ModalidadService modalidadService;

        public ModalidadController(ModalidadService modalidadService) { 
        
              this.modalidadService = modalidadService;
        }

        [HttpGet("all")]
        public ActionResult<ResponseApp> listarModalidad()
        {
            return Ok(modalidadService.ListarModalidades());
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearModalidad([FromBody] Dictionary<string, object> requestBody) {

            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var nombre = (JsonElement)requestBody["Nombre"];
            var descripcion = (JsonElement)requestBody["Descripcion"];
            var usuarioIngreso = (JsonElement)requestBody["UsuarioIngreso"];

            var modalidad = new Tmodalidad
            {

                Nombre = nombre.GetString(),
                Descripcion = descripcion.GetString(),
                UsusarioIngreso = usuarioIngreso.ValueKind == JsonValueKind.Number ? usuarioIngreso.GetInt32().ToString() : usuarioIngreso.GetString(),
            };

            return Ok(modalidadService.InsertarModalidad(modalidad));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarModalidad([FromBody] Dictionary<string, object> requestBody)
        {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var idModalidad = (JsonElement)requestBody["idModalidad"];
            var nombre = (JsonElement)requestBody["Nombre"];
            var descripcion = (JsonElement)requestBody["Descripcion"];
            var usuarioActualizacion = (JsonElement)requestBody["UsuarioActualizacion"];

            var modalidad = new Tmodalidad
            {
                IdModalidad = int.Parse(idModalidad.ToString()),
                Nombre = nombre.GetString(),
                Descripcion = descripcion.GetString(),
                UsuarioActualizacion = usuarioActualizacion.ValueKind == JsonValueKind.Number ? usuarioActualizacion.GetInt32().ToString() : usuarioActualizacion.GetString(),
            };

            return Ok(modalidadService.ModificarModalidad(modalidad));
        }
        [HttpPost("id")]
        public ActionResult<ResponseApp> listarById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idModalidad", out var idModalidadObj) || idModalidadObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idModalidadObj.ToString(), out int idModalidad))
            {

                return BadRequest(Utils.BadResponse("ID MODALIDAD NO VÁLIDO"));
            }
            return Ok(modalidadService.ConsultarModalidad(idModalidad));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idModalidad", out var idModalidadObj) || idModalidadObj == null)
            { 
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idModalidadObj.ToString(), out int idModalidad))
            {

                return BadRequest(Utils.BadResponse("ID MODALIDAD NO VÁLIDO"));
            }
            return Ok(modalidadService.EliminarModalidad(idModalidad));
        }
    }
}
