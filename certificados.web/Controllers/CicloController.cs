using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/ciclo/")]
    public class CicloController : Controller
    {
        private readonly CicloService cicloService;
        public CicloController(CicloService cicloService)
        {

            this.cicloService = cicloService;
        }

        [HttpGet("all")]
        public ActionResult<ResponseApp> listarCiclo()
        {
            return Ok(cicloService.ListarCiclos());
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearCiclo([FromBody] Dictionary<string, object> requestBody)
        {

            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var nombre = (JsonElement)requestBody["Nombre"];
            var descripcion = (JsonElement)requestBody["Descripcion"];
            var usuarioIngreso = (JsonElement)requestBody["UsuarioIngreso"];

            var ciclo = new Tciclo
            {
                Nombre = nombre.GetString(),
                Descripcion = descripcion.GetString(),
                UsuarioIngreso = usuarioIngreso.ValueKind == JsonValueKind.Number ? usuarioIngreso.GetInt32().ToString() : usuarioIngreso.GetString(),
            };

            return Ok(cicloService.InsertarCiclo(ciclo));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarCiclo([FromBody] Dictionary<string, object> requestBody)
        {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var idCiclo = (JsonElement)requestBody["idCiclo"];
            var nombre = (JsonElement)requestBody["Nombre"];
            var descripcion = (JsonElement)requestBody["Descripcion"];
            var userModificacion = (JsonElement)requestBody["UserModificacion"];

            var ciclo = new Tciclo
            {
                IdCiclo = int.Parse(idCiclo.ToString()),
                Nombre = nombre.GetString(),
                Descripcion = descripcion.GetString(),
                UserModificacion = userModificacion.ValueKind == JsonValueKind.Number ? userModificacion.GetInt32().ToString() : userModificacion.GetString(),
            };

            return Ok(cicloService.ModificarCiclo(ciclo));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> listarById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idCiclo", out var idCicloObj) || idCicloObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idCicloObj.ToString(), out int idCiclo))
            {

                return BadRequest(Utils.BadResponse("ID MODALIDAD NO VÁLIDO"));
            }
            return Ok(cicloService.ConsultarCiclo(idCiclo));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idCiclo", out var idCicloObj) || idCicloObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idCicloObj.ToString(), out int idCiclo))
            {

                return BadRequest(Utils.BadResponse("ID MODALIDAD NO VÁLIDO"));
            }
            return Ok(cicloService.EliminarCiclo(idCiclo));
        }
    }
}
