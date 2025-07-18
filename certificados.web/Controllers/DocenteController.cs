using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    /**
    * Controlador dedicado la entidad de TDOCENTE
    */
    [Route("api/docente/")]
    public class DocenteController : Controller
    {
        private readonly DocenteService docenteService;
        private readonly EstadoDocenteService estadoDocenteService;
        private readonly PersonaService personaService;

        public DocenteController(DocenteService docenteService, PersonaService personaService, EstadoDocenteService estadoDocenteService) { 
        
            this.docenteService = docenteService;
            this.personaService = personaService;
            this.estadoDocenteService = estadoDocenteService;
        }

        /*
          * Endpoint para crear un DOCENTE
          */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearDocente([FromBody] Dictionary<string, object> requestBody) {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var codDocente = (JsonElement)requestBody["codigoDocente"];
            var cedula = (JsonElement)requestBody["cedula"];
            var titulo = (JsonElement)requestBody["titulo"];
            var facultad = (JsonElement)requestBody["facultad"];
            var carrera = (JsonElement)requestBody["carrera"];
            var estado = (JsonElement)requestBody["estado"];
            var usuarioIngreso = (JsonElement)requestBody["usuarioIngreso"];

            var estadoResponse = estadoDocenteService.ListarById(int.Parse(estado.ToString()));
            var personaResponse = personaService.ObtenerPersona(cedula.ToString());

            if (!estadoResponse.Cod.Equals(CONSTANTES.COD_OK) || !personaResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("NO EXISTE INFORMACIÓN DEL ESTADO O PERSONA"));
            }
            Tpersona persona = PersonaMapper.toEntity(personaResponse.Data);
            TestadoDocente estadoDocente = EstadoDocenteMapper.toEntity(estadoResponse.Data);

            var docente = new Tdocente
            {
                CodigoDocente = codDocente.GetString(),
                Cedula = cedula.GetString(),
                Titulo = titulo.GetString(),
                Facultad = facultad.GetString(),
                Carrera = carrera.GetString(),
                IdEstado = int.Parse(estado.ToString()),
                UsuarioIngreso = usuarioIngreso.ValueKind == JsonValueKind.Number ? usuarioIngreso.GetInt32().ToString() : usuarioIngreso.GetString(),
                TestadoDocente = estadoDocente,
                Tpersona = persona
            };

            return Ok(docenteService.CrearDocente(docente));
        }

        /*
          * Endpoint para MODIFICAR un DOCENTE
          */
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarDocente([FromBody] Dictionary<string, JsonElement> requestBody)
        {
            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var idDocente = (JsonElement)requestBody["idDocente"];
            var codDocente = (JsonElement)requestBody["codigoDocente"];
            var cedula = (JsonElement)requestBody["cedula"];
            var titulo = (JsonElement)requestBody["titulo"];
            var facultad = (JsonElement)requestBody["facultad"];
            var carrera = (JsonElement)requestBody["carrera"];
            var estado = (JsonElement)requestBody["estado"];
            var userModificacion = (JsonElement)requestBody["usuarioActualizacion"];

            var estadoResponse = estadoDocenteService.ListarById(int.Parse(estado.ToString()));
            var personaResponse = personaService.ObtenerPersona(cedula.ToString());

            if (!estadoResponse.Cod.Equals(CONSTANTES.COD_OK) || !personaResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("NO EXISTE INFORMACIÓN DEL ESTADO O PERSONA"));
            }
            Tpersona persona = PersonaMapper.toEntity(personaResponse.Data);
            TestadoDocente estadoDocente = EstadoDocenteMapper.toEntity(estadoResponse.Data);

            var docente = new Tdocente
            {
                CodigoDocente = codDocente.GetString(),
                Cedula = cedula.GetString(),
                Titulo = titulo.GetString(),
                Facultad = facultad.GetString(),
                Carrera = carrera.GetString(),
                IdEstado = int.Parse(estado.ToString()),
                UserModificacion = userModificacion.ValueKind == JsonValueKind.Number ? userModificacion.GetInt32().ToString() : userModificacion.GetString(),
                TestadoDocente = estadoDocente,
                Tpersona = persona
            };

            return Ok(docenteService.ActualizarDocente(docente));
        }

        /*
         * Endpoint para LISTATAR DOCENTE
         */
        [HttpGet("all")]
        public ActionResult<ResponseApp> obtenerDocentes() { 
            return Ok(docenteService.ListarDocentes());
        }

        /*
        * Endpoint para OBTENER un DOCENTE
        */
        [HttpPost("id")]
        public ActionResult<ResponseApp> obtenerDocenteById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("codigoDocente", out var codigoDocenteObj) || codigoDocenteObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            string codigoDocente = codigoDocenteObj.ToString();
            return Ok(docenteService.ObtenerDocentesById(codigoDocente));
        }

        /*
       * Endpoint para ELIMINAR un DOCENTE
       */
        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarDocenteById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idDocente", out var idDocenteObj) || idDocenteObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            string codigoDocente = idDocenteObj.ToString();
            return Ok(docenteService.ElminarDocente(codigoDocente));
        }

        /*
         * Endpoint para OBTENER un DOCENTE
         */
        [HttpPost("cedula")]
        public ActionResult<ResponseApp> obtenerDocenteByCedula([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("cedula", out var cedulaObj) || cedulaObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            string cedula = cedulaObj.ToString();
            return Ok(docenteService.ObtenerDocentesByCedula(cedula));
        }
    }
}
