using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/estadoDocente")]
    public class EstadoDocenteController : Controller
    {
        private readonly EstadoDocenteService estadoDocenteService;

        public EstadoDocenteController(EstadoDocenteService estadoDocenteService)
        {

            this.estadoDocenteService = estadoDocenteService;
        }
        /*
         * Endpoint para crear un Docente
         */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearDocente([FromBody] TestadoDocente testadoDocente)
        {

            if (testadoDocente == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(estadoDocenteService.CrearEstadoDocente(EstadoDocenteMapper.toEntityCreate(testadoDocente)));
        }

        /*
         * Endpoint para crear un Docente
         */
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarDocente([FromBody] TestadoDocente testadoDocente)
        {

            if (testadoDocente == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(estadoDocenteService.ActualizarEstadoDocente(EstadoDocenteMapper.toEntityUpdate(testadoDocente)));
        }

        /*
         * Endpoint para crear un Docente
         */
        [HttpPost("id")]
        public ActionResult<ResponseApp> BuscarDocente([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idEstadoDocente", out var idEstadoDocenteObj) || idEstadoDocenteObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idEstadoDocenteObj.ToString(), out int idEstadoDocente))
            {
                return BadRequest(Utils.BadResponse("ID ESTADO DOCENTE NO VÁLIDO"));
            }

            return Ok(estadoDocenteService.ListarById(idEstadoDocente));
        }

        [HttpGet("all")]
        public ActionResult<ResponseApp> listarEstados()
        {

            return Ok(estadoDocenteService.ListarEstados());
        }
    }
}
