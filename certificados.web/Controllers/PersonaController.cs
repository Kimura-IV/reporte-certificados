using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace certificados.web.Controllers
{
    /**
     * Controlador dedicado la entidad de TPERSONA
     */
    [Route("api/personas")]
    public class PersonaController : Controller
    {
        private readonly PersonaService personaService;
        private readonly RolService rolService;


        public PersonaController(PersonaService _personaService, RolService _rolService)
        {

            personaService = _personaService ?? throw new ArgumentNullException(nameof(personaService));
            this.rolService = _rolService;
        }

        // GET: PersonaController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("all")]
        public ActionResult<ResponseApp> listarPersonas([FromBody] Dictionary<string, object> requestBody)
        {

            if (!requestBody.TryGetValue("estado", out var idstadoObj))
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return personaService.ListarPersonas(idstadoObj.ToString());
        }

        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearPersona([FromBody] PersonaDTO dto)
        {

            if (dto == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                return BadRequest(Utils.BadResponse(errors));
            }
            //if (!Utils.ValidarCedula(dto.Cedula)) {
            //    return BadRequest(Utils.BadResponse("CEDULA INVALIDA"));
            //}

            var rolRequest = rolService.BuscarRol(dto.idRol);
            if (!rolRequest.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE"));
            }
            var rolEntity = rolRequest.Data as Trol;
            if (rolEntity == null)
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE O DATOS INVÁLIDOS"));
            }

            Tpersona persona = new Tpersona
            {
                Cedula = dto.Cedula,
                Nombres = Utils.SafeString(dto.Nombres),
                Apellidos = Utils.SafeString(dto.Apellidos),
                Edad = dto.Edad,
                Genero = dto.Genero,
                UsuarioIngreso = dto.UsuarioIngreso,
                FechaCreacion = Utils.timeParsed(DateTime.Now),
                FechaModificacion = Utils.timeParsed(DateTime.Now),
            };
            Tusuario usuario = new Tusuario
            {
                Email = dto.email,
                Clave = dto.clave,
                Tpersona = persona,
                Trol = rolEntity,
                UsusarioIngreso = dto.UsuarioIngreso,
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                Estado = "ACT"
            };
            return Ok(personaService.CrearPersona(persona, usuario));
        }

        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarPersona([FromBody]  PersonaDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                return BadRequest(Utils.BadResponse(errors));
            }

            var rolRequest = rolService.BuscarRol(dto.idRol);
            if (!rolRequest.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE"));
            }
            var rolEntity = rolRequest.Data as Trol;
            if (rolEntity == null)
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE O DATOS INVÁLIDOS"));
            }
            Tpersona persona = new Tpersona
            {
                Cedula = dto.Cedula,
                Nombres = Utils.SafeString(dto.Nombres),
                Apellidos = Utils.SafeString(dto.Apellidos),
                Edad = dto.Edad,
                Genero = dto.Genero,
                UsuarioIngreso = dto.UsuarioIngreso,
                UsuarioModificacion = Utils.SafeString(dto.UsuarioActualizacion),
                FechaCreacion = Utils.timeParsed(DateTime.Now),
                FechaModificacion = Utils.timeParsed(DateTime.Now),
            };
            Tusuario usuario = new Tusuario
            {
                Email = dto.email,
                Clave = dto.clave,
                Tpersona = persona,
                Trol = rolEntity,
                UsusarioIngreso = dto.UsuarioIngreso,
                UsuarioModificacion = Utils.SafeString(dto.UsuarioActualizacion),
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                Estado = dto.estado
            };
            return Ok(personaService.ModificarPersona(persona, usuario));
        }

        [HttpPost("modificar/perfil")]
        public ActionResult<ResponseApp> modificarPerfil([FromBody] PersonaDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                 .SelectMany(v => v.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                return BadRequest(Utils.BadResponse(errors));
            }

            var rolRequest = rolService.BuscarRol(dto.idRol);
            if (!rolRequest.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE"));
            }
            var rolEntity = rolRequest.Data as Trol;
            if (rolEntity == null)
            {
                return BadRequest(Utils.BadResponse("ROL NO EXISTE O DATOS INVÁLIDOS"));
            }
            Tpersona persona = new Tpersona
            {
                Cedula = dto.Cedula,
                Nombres = Utils.SafeString(dto.Nombres),
                Apellidos = Utils.SafeString(dto.Apellidos),
                Edad = dto.Edad,
                Genero = dto.Genero,
                UsuarioIngreso = dto.UsuarioIngreso,
                UsuarioModificacion = Utils.SafeString(dto.UsuarioActualizacion),
                FechaCreacion = Utils.timeParsed(DateTime.Now),
                FechaModificacion = Utils.timeParsed(DateTime.Now),
            };
            Tusuario usuario = new Tusuario
            {
                Email = dto.email,
                Clave = dto.clave,
                Tpersona = persona,
                Trol = rolEntity,
                UsusarioIngreso = dto.UsuarioIngreso,
                UsuarioModificacion = Utils.SafeString(dto.UsuarioActualizacion),
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                Estado = dto.estado
            };
            return Ok(personaService.ModificarPerfil(persona, usuario));
        }

        [HttpPost("id")]
        public ActionResult<ResponseApp> buscarPorId([FromBody] Dictionary<string, object> requestBody)
        {
            if (!requestBody.TryGetValue("id", out var cedula))
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(personaService.ObtenerPersona(cedula.ToString()));
        }

        [HttpPost("perfil")]
        public ActionResult<ResponseApp> buscarPorCedulaCompleto([FromBody] Dictionary<string, object> requestBody)
        {
            if (!requestBody.TryGetValue("cedula", out var cedula))
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(personaService.ObtenerPersonaCompleta(cedula.ToString()));
        }

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> Eliminarersonas([FromBody] Dictionary<string, object> requestBody)
        {

            if (!requestBody.TryGetValue("cedula", out var cedula))
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return personaService.ELiminarPersona(cedula.ToString());
        }
    }
}
