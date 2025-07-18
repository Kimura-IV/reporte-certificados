using certificados.models.Entitys;
using certificados.services.Services;
using certificados.services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        }

        [HttpPost("login")]
        public ActionResult<ResponseApp> LogeoController([FromBody] Dictionary<string, string> requestBody)
        {
            if (!requestBody.TryGetValue("email", out var email) || string.IsNullOrWhiteSpace(email) ||
                !requestBody.TryGetValue("password", out var password) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("FALTAN DATOS");
            }

            var response = _usuarioService.LoginUsuario(email, password);

            if (response == null)
            {
                return Unauthorized("Credenciales incorrectas");
            }

            var redirectTo = "/Dashboard/Index";

            return Ok(new { response, redirectTo });
        }
    }
}
