using certificados.models.Entitys;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    /*
     * Enpoint para indicar que el servicio esta activo
     */
    [Route("api/isalive")]
    public class IsAliveController : Controller
    {
        [HttpGet]
                
        public ResponseApp isAlive() {

            return Utils.OkResponse("SERVICIO ACTIVO");
        }
    }
}
