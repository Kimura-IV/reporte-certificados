using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [Route("api/calificacion")]
    public class ActaCalificacionController : Controller
    {
        private readonly ActaCalificacionService calificacionService;
        private readonly EventoService eventoService;


        public ActaCalificacionController(ActaCalificacionService calificacionService, EventoService eventoService)
        {
            this.calificacionService = calificacionService;
            this.eventoService = eventoService;
        }


        [HttpGet("all")]
        public ActionResult<ResponseApp> Listar() {

            return calificacionService.Listar();
        }
        [HttpPost("crear")]
        public ActionResult<ResponseApp> Crear([FromBody] ActaCalificacionDTO dto) {

            if (dto == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }

            var eventoResponse = eventoService.ListarPorId(dto.IdEvento);

            if (!eventoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse($"EL EVENTO NO EXISTE {dto.IdEvento}"));
            }

            Tevento evento = EventoMapper.convertEntity(eventoResponse.Data);

            return Ok(calificacionService.Insertar(CalificacionMapper.toEntity(dto, evento)));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> Buscar([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idActa", out var idActaObj) || idActaObj == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!int.TryParse(idActaObj.ToString(), out int idActa))
            {
                return BadRequest(Utils.BadResponse("ID ACTA NO VÁLIDO"));
            }

            return Ok(calificacionService.BuscarId(idActa));    
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> Eliminar([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idActa", out var idActaObj) || idActaObj == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!int.TryParse(idActaObj.ToString(), out int idActa))
            {
                return BadRequest(Utils.BadResponse("ID ACTA NO VÁLIDO"));
            }

            return Ok(calificacionService.Eliminar(idActa));
        }

    }
}
