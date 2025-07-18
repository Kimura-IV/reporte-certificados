using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/evento")]
    public class EventoController : Controller
    {
        private readonly EventoService eventoService;
        private readonly GrupoService grupoService;
        private readonly ModalidadService modalidadService;
        private readonly TipoEventoService tipoEventoService;
        private readonly DecanatoService decanatoService;

        public EventoController(EventoService eventoService, GrupoService grupoService, ModalidadService modalidadService
            , TipoEventoService tipoEventoService, DecanatoService decanatoService) {

            this.eventoService = eventoService;
            this.grupoService = grupoService;
            this.modalidadService = modalidadService;
            this.tipoEventoService = tipoEventoService;
            this.decanatoService = decanatoService;
        }

        [HttpGet("all")]
        public ActionResult<ResponseApp> listarEstados()
        {
            return Ok(eventoService.ListarEventos());
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearEvento([FromBody] EventoDTO dto) {
            if (dto == null || dto.Horas <0) {

                return Utils.BadResponse("PARAMETROS INCORRECTOS");
            }
            var grupoResponse = grupoService.BuscarGrupo(dto.IdGrupo);
            if (!grupoResponse.Cod.Equals(CONSTANTES.COD_OK)) {
                return Utils.BadResponse($"NO EXISTE EL GRPO {dto.IdGrupo}");
            }
            Tgrupo grupo = GrupoPersonaMapper.toEntityGrupo(grupoResponse.Data);

            var modalidadResponse = modalidadService.ConsultarModalidad(dto.IdModalidad);
            if (!modalidadResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL MODALIDAD {dto.IdModalidad}");
            }
            Tmodalidad modalidad = TmodalidadMapper.toEntity(modalidadResponse.Data);
            var tipoResponse = tipoEventoService.ConsultarTipoEvento(dto.IdTipoEvento);
            if (!tipoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL TIPO EVENTO {dto.IdTipoEvento}");
            }
            TtipoEvento tipoEvento = TipoEventoMapper.toEntity(tipoResponse.Data);

            var decanatoResponse = decanatoService.ObtenerDecanatoById(dto.IdDecanato);
            if (!decanatoResponse.Cod.Equals(CONSTANTES.COD_OK)) {
                return Utils.BadResponse($"NO EXISTE EL DECANATO {dto.IdDecanato}");
            }

            Tdecanato tdecanato = DecanatoMapper.toEntityObject(decanatoResponse.Data);
            Tevento evento = EventoMapper.toEntity(dto, modalidad, tipoEvento, grupo, tdecanato);
            return Ok(eventoService.CrearEvento(evento));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarEvento([FromBody] EventoDTO dto)
        {
            if (dto == null || dto.Horas <0)
            {

                return Utils.BadResponse("PARAMETROS INCORRECTOS");
            }

            var grupoResponse = grupoService.BuscarGrupo(dto.IdGrupo);
            if (!grupoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL GRPO {dto.IdGrupo}");
            }
            Tgrupo grupo = GrupoPersonaMapper.toEntityGrupo(grupoResponse.Data);

            var modalidadResponse = modalidadService.ConsultarModalidad(dto.IdModalidad);
            if (!modalidadResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL MODALIDAD {dto.IdModalidad}");
            }
            Tmodalidad modalidad = TmodalidadMapper.toEntity(modalidadResponse.Data);
            var tipoResponse = tipoEventoService.ConsultarTipoEvento(dto.IdTipoEvento);
            if (!tipoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL TIPO EVENTO {dto.IdTipoEvento}");
            }
            TtipoEvento tipoEvento = TipoEventoMapper.toEntity(tipoResponse.Data);

            var decanatoResponse = decanatoService.ObtenerDecanatoById(dto.IdDecanato);
            if (!decanatoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse($"NO EXISTE EL DECANATO {dto.IdDecanato}");
            }

            Tdecanato tdecanato = DecanatoMapper.toEntityObject(decanatoResponse.Data);
            Tevento tevento = EventoMapper.toEntityUpdate(dto, modalidad, tipoEvento, grupo, tdecanato);
            return Ok(eventoService.ActualizarEvento(tevento));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> listarById([FromBody] Dictionary<string, object> request) { 
        
            if(!request.TryGetValue("idEvento",out var idEventoObj) || idEventoObj == null){

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idEventoObj.ToString(), out int idEvento)) {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(eventoService.ListarPorId(idEvento));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idEvento", out var idEventoObj) || idEventoObj == null)
            {

                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }

            if (!int.TryParse(idEventoObj.ToString(), out int idEvento))
            {

                return BadRequest(Utils.BadResponse("ID EVENTO NO VÁLIDO"));
            }
            return Ok(eventoService.EliminarEvento(idEvento));
        }

        [HttpPost("dinamico")]
        public ActionResult<ResponseApp> ListarEventosPorParametro([FromBody] Dictionary<string, object> request)
        { 
            if (!request.TryGetValue("propiedad", out var propiedadObj) || propiedadObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTA EL PARÁMETRO 'PROPIEDAD'"));
            }
             
            if (!request.TryGetValue("valor", out var valorObj) || valorObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTA EL PARÁMETRO 'VALOR'"));
            }

            try
            {
                var propiedad = propiedadObj.ToString();
                var valor = valorObj;
              
                var response = eventoService.ListarPorParametro(propiedad, valor);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Utils.BadResponse($"ERROR AL LISTAR EVENTOS: {ex.Message}"));
            }
        }



    }
}
