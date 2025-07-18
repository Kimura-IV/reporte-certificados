using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    [Route("api/rol")]
    public class RolController : Controller
    {
        private readonly RolService rolService;
            
        public RolController(RolService rolService)
        {
            this.rolService = rolService;
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearRol([FromBody] Dictionary<string, object> requestBody) {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            var dto = new Trol
            {
                Nombre = requestBody["Nombre"]?.ToString(),
                Estado = true,
                UsuarioIngreso = requestBody["UsuarioIngreso"]?.ToString(),
                Observacion = requestBody["Observacion"]?.ToString(),
                FCreacion = Utils.timeParsed(DateTime.Now)
            };

            return Ok(rolService.CrearRol(dto));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarRol([FromBody] Dictionary<string, JsonElement> requestBody) {

            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            var dto = new Trol
            {
                IdRol = int.Parse(requestBody["idRol"].ToString()),
                Nombre = requestBody["Nombre"].GetString(),
                Estado = requestBody["Estado"].ToString().ToLower() == "true",
                UsuarioModificacion = requestBody["UsuarioActualizacion"].ToString(),
                Observacion = requestBody["Observacion"].ToString(), 
            };
            return Ok(rolService.ModificarRol(dto));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarRol([FromBody] Dictionary<string, object> requestBody)
        {
            if (!requestBody.TryGetValue("idRol", out var idRolObj) ||
             idRolObj == null ||
             !int.TryParse(idRolObj.ToString(), out int idRol))
                {
                    return BadRequest(Utils.BadResponse("FALTA PARAMETROS"));
                }

            return Ok(rolService.EliminarRol(idRol));
        }

        [HttpGet("all")]
        public ResponseApp obtenerRoles()
        {
            return rolService.ListarRoles();
        }

        [HttpPost("id")]
        public ActionResult ObtenerRolById([FromBody] Dictionary<string, object> requestBody)
        {
            if (!requestBody.TryGetValue("idRol", out var idRolObj) ||
                idRolObj == null ||
                !int.TryParse(idRolObj.ToString(), out int idRol))
            {
                return BadRequest(Utils.BadResponse("FALTA PARAMETROS"));
            }
            return Ok(rolService.BuscarRol(idRol));
        }

    }
}
