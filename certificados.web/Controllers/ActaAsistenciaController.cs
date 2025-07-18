using Azure.Core;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [Route("api/asistencia")]
    public class ActaAsistenciaController : Controller
    {
        private readonly ActaAsistenciaService actaAsistenciaService;
        private readonly EventoService eventoService;

        public ActaAsistenciaController(ActaAsistenciaService actaAsistenciaService, EventoService eventoService) { 
        
            this.actaAsistenciaService = actaAsistenciaService;
            this.eventoService = eventoService;
        }

        [HttpGet("all")]
        public ActionResult<ResponseApp> Listar() { 
            return actaAsistenciaService.Listar();
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> Crear([FromBody] ActaAsistenciaDTO dto) {

            if (dto == null) {
                return BadRequest( Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }

            var eventoResponse = eventoService.ListarPorId(dto.IdEvento);

            if (!eventoResponse.Cod.Equals(CONSTANTES.COD_OK)) {
                return BadRequest(Utils.BadResponse($"EL EVENTO NO EXISTE {dto.IdEvento}"));
            }

            Tevento evento = EventoMapper.convertEntity(eventoResponse.Data);

            return Ok(actaAsistenciaService.InsertarAsistencia(AsistenciaMapper.toEntity(dto, evento)));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> Buscar([FromBody] Dictionary<string, object> request) {

            if (!request.TryGetValue("idActa", out var idActaObj) || idActaObj == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!int.TryParse(idActaObj.ToString(), out int idActa))
            {
                return BadRequest(Utils.BadResponse("ID ACTA NO VÁLIDO"));
            }
            return Ok(actaAsistenciaService.BuscarId(idActa));
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
            return Ok(actaAsistenciaService.ELiminar(idActa));
        }


    }
}
