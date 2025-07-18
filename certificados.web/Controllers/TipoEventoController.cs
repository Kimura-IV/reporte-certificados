using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/tipoEvento")]
    public class TipoEventoController :Controller
    {
        private readonly TipoEventoService tipoEventoService;
        public TipoEventoController(TipoEventoService tipoEventoService) { 
            this.tipoEventoService = tipoEventoService;
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> creartipoEvento([FromBody] TtipoEvento ttipoEvento) {
            if (ttipoEvento == null) {
                return Utils.BadResponse("FALTAN PARAMETROS");
            }

            return Ok(tipoEventoService.InsertarTipoEvento(ttipoEvento));
        }
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> ActualizartipoEvento([FromBody] TtipoEvento ttipoEvento)
        {
            if (ttipoEvento == null)
            {
                return Utils.BadResponse("FALTAN PARAMETROS");
            }

            return Ok(tipoEventoService.ActualizarTipoEvento(ttipoEvento));
        }
        [HttpGet("all")]
        public ActionResult<ResponseApp> ListartipoEvento()
        {

            return Ok(tipoEventoService.listarTiposEvento());
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> listarById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idtipoevento", out var idtipoeventoObj) || idtipoeventoObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idtipoeventoObj.ToString(), out int idtipoevento))
            {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(tipoEventoService.ConsultarTipoEvento(idtipoevento));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idtipoevento", out var idtipoeventoObj) || idtipoeventoObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idtipoeventoObj.ToString(), out int idtipoevento))
            {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(tipoEventoService.EliminarTipoEvento(idtipoevento));
        }

    }
}
