using certificados.models.Context;
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
    [Route("api/grupoPersona")]
    public class GrupoPersonaController : Controller
    {
        private readonly GrupoPersonaService grupoPersonaService;
        private readonly GrupoService grupoService;
        private readonly PersonaService personaService;
        private readonly AppDbContext context;

        public GrupoPersonaController(GrupoPersonaService grupo, GrupoService grupoService, PersonaService personaService, AppDbContext appDbContext) { 
        
            this.grupoPersonaService = grupo;   
            this.personaService = personaService;   
            this.grupoService = grupoService;
            this.context = appDbContext;
        }

        [HttpGet("listar")]
        public ActionResult<ResponseApp> ListarGrupo() {
            return grupoPersonaService.ListarGrupo();
        }

        [HttpPost("buscarPersona")]
        public ActionResult<ResponseApp> BuscarCedula([FromBody] Dictionary<string, object> request)
        {
            if (!request.ContainsKey("idGrupo") || !request.ContainsKey("cedula"))
            {
                return Utils.BadResponse("Faltan parámetros en la solicitud.");
            }

            string cedula = request["cedula"]?.ToString();
            if (string.IsNullOrEmpty(cedula))
            {
                return Utils.BadResponse("El parámetro 'cedula' no puede estar vacío.");
            }
            if (!int.TryParse(request["idGrupo"]?.ToString(), out int id))
            {
                return Utils.BadResponse("El parámetro 'idGrupo' no es válido.");
            }

            return grupoPersonaService.BuscarCedulaId(id, cedula);
        }



        [HttpPost("buscar")]
        public ActionResult<ResponseApp> BuscarGrupo([FromBody] Dictionary<string, object> request)
        {
            if (!request.ContainsKey("idGrupo"))
            {
                return Utils.BadResponse("Faltan parámetros en la solicitud.");
            }
            int id;

            if (!int.TryParse(request["idGrupo"]?.ToString(), out id))
            {
                return Utils.BadResponse("El parámetro 'id' no es válido.");
            }
            return grupoPersonaService.BuscarById(id, false);
        }

        [HttpPost("pendientes")]
        public ActionResult<ResponseApp> BuscarGrupoPreAprobar([FromBody] Dictionary<string, object> request)
        {
            if (!request.ContainsKey("idGrupo"))
            {
                return Utils.BadResponse("Faltan parámetros en la solicitud.");
            }
            int id;
            Boolean estado = !request.ContainsKey("estado") ? true : false;

            if (!int.TryParse(request["idGrupo"]?.ToString(), out id))
            {
                return Utils.BadResponse("El parámetro 'id' no es válido.");
            }

            if (!int.TryParse(request["idRol"]?.ToString(), out int idRol))
            {
                return Utils.BadResponse("El parámetro 'idRol' no es válido.");
            }

            return grupoPersonaService.BuscarBeneficiarios(id, idRol, estado);
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> CrearGrupoPersona([FromBody] GrupoPersonaDTO dto) {
            ResponseApp response = Utils.BadResponse(null);
            if (dto == null || dto.Cedulas == null || !dto.Cedulas.Any()) {

                return Utils.BadResponse("FALTAN PARAMETROS");
            }

            try
            {
                List<string> cedulasInsertadas = new List<string>();
                List<string> cedulasFallidas = new List<string>();
                var grupoResponse = grupoService.BuscarGrupo(dto.IdGrupo);
                if (!grupoResponse.Cod.Equals(CONSTANTES.COD_OK)) {
                    response.Message = $"NO EXISTE EL GRUPO {dto.IdGrupo}";
                    return response;
                }
                Tgrupo grupo = GrupoPersonaMapper.toEntityGrupo(grupoResponse.Data);
                
                foreach(var cedula in dto.Cedulas) {

                    var personaResponse = personaService.ObtenerPersona(cedula);
                    if (personaResponse.Cod.Equals(CONSTANTES.COD_OK)) { 
                        Tpersona persona = PersonaMapper.toEntity(personaResponse.Data);
                        TgrupoPersona tgrupoPersonaEntity = GrupoPersonaMapper.toEntity(dto, persona, grupo, cedula);
                        if (grupoPersonaService.BuscarCedulaId(grupo.IdGrupo, cedula).Cod.Equals(CONSTANTES.COD_OK)) {
                            continue;
                        }

                        tgrupoPersonaEntity.IdGrupoPersona = context.TgrupoPersona.Count() + 1;
                        var responseGrupo = grupoPersonaService.InsertarPersona(tgrupoPersonaEntity);
                        if (responseGrupo.Cod.Equals(CONSTANTES.COD_OK)) {
                            cedulasInsertadas.Add(cedula);
                        } else
                        {
                            cedulasFallidas.Add(cedula);
                        }
                    } else
                    {
                        cedulasFallidas.Add($"NO EXISTE PERSONA CON {cedula}");
                    }
                }
                response = Utils.OkResponse(new {
                    cedulasInsertadas, 
                    cedulasFallidas
                });

            }
            catch (Exception ex) {
                response = Utils.BadResponse($"ERROR AL CREAR GRUPO PERSONA: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR GRUPO: {ex.Message}");
            }
            return response;
        }
        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> EliminarGrupo([FromBody] Dictionary<string, object> request) {
            if (!request.ContainsKey("idGrupoPersona") || !request.ContainsKey("cedula"))
            {
                return Utils.BadResponse("Faltan parámetros en la solicitud.");
            }

            string cedula = request["cedula"]?.ToString();
            if (string.IsNullOrEmpty(cedula))
            {
                return Utils.BadResponse("El parámetro 'cedula' no puede estar vacío.");
            }
            if (!int.TryParse(request["idGrupoPersona"]?.ToString(), out int id))
            {
                return Utils.BadResponse("El parámetro 'idGrupoPersona' no es válido.");
            }
            return grupoPersonaService.EliminarGrupo(id, cedula );
        }
        [HttpPost("aprobar")]
        public ActionResult<ResponseApp> AprobarGrupos([FromBody] AprobarParticipantesDTO data) {

            if (data == null) {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }
            if (data.Aprobados.Count <= 0) {
                return Utils.BadResponse("NO EXISTEN DATOS DE APROBACION");
            }
            return grupoPersonaService.AprobarGrupos(data.Aprobados, data.IdGrupo, data.usuarioActualizacion);
        }
    }
}
