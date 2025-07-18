using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/grupo/")]
    public class GrupoController : Controller
    {
        private readonly GrupoService grupoPersonaService;

        public GrupoController(GrupoService grupo) { 
        
            this.grupoPersonaService = grupo;
        }
        [HttpGet("all")]
        public ActionResult<ResponseApp> listarEstados()
        {

            return Ok(grupoPersonaService.ListarGrupos());
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> CrearGrupoPersona([FromBody] Dictionary<string, object> requestBody)
        {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var jsonNombre = (JsonElement)requestBody["Nombre"];
            var jsonCantidad = (JsonElement)requestBody["Cantidad"];
            var jsonUsuarioIngreso = (JsonElement)requestBody["UsuarioIngreso"];

            var dto = new Tgrupo
            {
                Nombre = jsonNombre.GetString(),
                Cantidad = jsonCantidad.GetInt32(),
                UsuarioIngreso = jsonUsuarioIngreso.ValueKind == JsonValueKind.Number ? jsonUsuarioIngreso.GetInt32().ToString() : jsonUsuarioIngreso.GetString(),
            };

            return Ok(grupoPersonaService.InsertarGrupo(dto));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> ModificarGrupo([FromBody] Dictionary<string, JsonElement> requestBody)
        {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            var jsonIdGrupo = (JsonElement)requestBody["idGrupo"];
            var jsonNombre = (JsonElement)requestBody["Nombre"];
            var jsonCantidad = (JsonElement)requestBody["Cantidad"];
            var jsonUsuarioActualizacion = (JsonElement)requestBody["UsuarioActualizacion"];

            var dto = new Tgrupo
            {
                IdGrupo = int.Parse(jsonIdGrupo.ToString()),
                Nombre = jsonNombre.GetString(),
                Cantidad = jsonCantidad.GetInt32(),
                UsuarioActualizacion = jsonUsuarioActualizacion.ValueKind == JsonValueKind.Number ? jsonUsuarioActualizacion.GetInt32().ToString() : jsonUsuarioActualizacion.GetString(),
            };

            return Ok(grupoPersonaService.ModificarGrupo(dto));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> listarById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idGrupo", out var idGrupoObj) || idGrupoObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idGrupoObj.ToString(), out int idGrupo))
            {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(grupoPersonaService.BuscarGrupo(idGrupo));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> EliminarById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idGrupo", out var idGrupoObj) || idGrupoObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idGrupoObj.ToString(), out int idGrupo))
            {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(grupoPersonaService.EliminarGrupo(idGrupo));
        }
    }
}
