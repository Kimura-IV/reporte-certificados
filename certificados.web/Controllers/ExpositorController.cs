using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace certificados.web.Controllers
{
    /**
     * Controlador dedicado la entidad de TEXPOSITOR
     */
    [ApiController]
    [Route("api/expositor")]
    public class ExpositorController : Controller
    {
        //
        private readonly ExpositorService expositorService;
        private readonly PersonaService personaService;

        public ExpositorController(ExpositorService expositorService, PersonaService _personaService)
        {
            this.expositorService = expositorService;
            this.personaService = _personaService;
        }
        /*
         * Endpoint para crear un expositor
         */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearExpositor([FromBody] ExpositorDTO dto) {
            if (dto == null)
            {

                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            var requestPersona = personaService.ObtenerPersona(dto.Cedula); 
            if (!requestPersona.Cod.Equals(CONSTANTES.COD_OK)){
                return BadRequest(Utils.BadResponse("USUARIO NO EXISTE"));
            }

            Tpersona persona = PersonaMapper.toEntity(requestPersona.Data);

            Texpositor expositor = ExpositorMapper.toDtoToEntity(dto, persona);
            return Ok(expositorService.crearExpositor(expositor));
        }
        /*
         * Endpoint para modificar un expositor
         */
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarExpositor([FromBody] ExpositorDTO dto)
        {
            if (dto == null)
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            var requestPersona = personaService.ObtenerPersona(dto.Cedula);
            if (!requestPersona.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("USUARIO NO EXISTE"));
            }

            int idExpositor = dto.IdExpositor ?? 0; 
            if (!expositorService.buscarPorID(idExpositor).Cod.Equals(CONSTANTES.COD_OK)) {
                return BadRequest(Utils.BadResponse($"EXPOSITOR {idExpositor} NO EXISTE"));
            }
            Tpersona persona = PersonaMapper.toEntity(requestPersona.Data);
            Texpositor expositor = ExpositorMapper.toDtoToEntity( persona, dto);

            return Ok(expositorService.modificarExpositor(expositor));
        }
        /*
         * Endpoint para obtener todos los expositor
         */

        [HttpGet("all")]
        public ActionResult<ResponseApp> listarExpositores() {
            return Ok(expositorService.listarExpositores());
        }

        /*
         * Endpoint para obtener un expositor
         */

        [HttpPost("id")]
        public ActionResult<ResponseApp> obtenerExpositorById([FromBody] Dictionary<string, object> request) {

            if (!request.TryGetValue("idExpositor", out var idExpositorObj) || idExpositorObj == null
                ||
                !int.TryParse(idExpositorObj.ToString(), out int idExpositor)) {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            } 
            return Ok(expositorService.buscarPorID(idExpositor));
        }

    }
}
