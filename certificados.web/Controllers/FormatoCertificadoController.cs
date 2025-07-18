using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/formato")]
    public class FormatoCertificadoController : Controller
    {
        private readonly FormatoCertificadoService formatoService;

        public FormatoCertificadoController(FormatoCertificadoService formatoService) { 
        
            this.formatoService = formatoService;
        }
        /*
         * Endpoint para crear un FORMATO
         */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearFormato([FromBody] Dictionary<string, string> formatoData) {

            if (formatoData == null || formatoData.Count == 0)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }

            return Ok(formatoService.CrearFormato(FormatoCertificadoMapper.toEntityCreate(formatoData)));
        }

        /*
        * Endpoint para eliminar un FORMATO
        */
        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarFormato([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idFormato", out var idFormatoObj) || idFormatoObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idFormatoObj.ToString(), out int idFormato))
            {
                return BadRequest(Utils.BadResponse("ID FORMATO NO VÁLIDO"));
            }

            return Ok(formatoService.EliminarFormato(idFormato));
        }

        /*
        * Endpoint para buscar un FORMATO
        */
        [HttpPost("id")]
        public ActionResult<ResponseApp> buscarFormato([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idFormato", out var idFormatoObj) || idFormatoObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idFormatoObj.ToString(), out int idFormato))
            {
                return BadRequest(Utils.BadResponse("ID FORMATO NO VÁLIDO"));
            }

            return Ok(formatoService.ListarFormatoByID(idFormato));
        }
        /*
        * Endpoint para listar un FORMATO
        */
        [HttpGet("all")]
        public ActionResult<ResponseApp> listarFormatos() { 
            return Ok(formatoService.ListarFormatos());
        }
    }
}
