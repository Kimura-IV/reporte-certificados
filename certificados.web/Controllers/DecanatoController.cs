using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
     /**
     * Controlador dedicado la entidad de TDECANATO
     */
    [Route("api/decanato")]
    public class DecanatoController:Controller
    {
        private DecanatoService serviceDecanato;

        public DecanatoController(DecanatoService serviceDecanato)
        {
            this.serviceDecanato = serviceDecanato;
        }

        /*
         * Endpoint para crear un DECANATO
         */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearDecanato([FromBody] Tdecanato tdecanato) {

            if (tdecanato == null) {

                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(serviceDecanato.CrearDecanato(DecanatoMapper.toEntityCreate(tdecanato)));
        }

        /**
         * Enpoint para modificar un DECANATO
         */
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarDecanato([FromBody] Tdecanato tdecanato) {

            if (tdecanato == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            return Ok(serviceDecanato.ModificarDecanato(DecanatoMapper.toEntityUpdate(tdecanato)));
        }
        /***
         * Endpoint que lista todos los DECANATOS 
         */
        [HttpGet("all")]
        public ActionResult<ResponseApp> listarDecanatos()
        {
            return Ok(serviceDecanato.ListarDecanatos());
        }

        /*
         * Endpoint para obtener un decanato
         */

        [HttpPost("id")]
        public ActionResult<ResponseApp> obtenerExpositorById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idDecanato", out var idDecanatoObj) || idDecanatoObj == null
                ||
                !int.TryParse(idDecanatoObj.ToString(), out int idDecanato))
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            return Ok(serviceDecanato.ObtenerDecanatoById(idDecanato));
        }

        /*
         * Endpoint para eliminar un decanato
         */

        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarDecanatoById([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idDecanato", out var idDecanatoObj) || idDecanatoObj == null
                ||
                !int.TryParse(idDecanatoObj.ToString(), out int idDecanato))
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            return Ok(serviceDecanato.EliminarDecanato(idDecanato));
        }
    }
}
