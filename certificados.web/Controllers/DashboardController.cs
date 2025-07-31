using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("Dashboard"); 
        }

        public IActionResult Inicio()
        {
            return PartialView("~/Views/Inicio/_Home.cshtml");
        }
        public IActionResult Personas()
        {
            return PartialView("~/Views/Mantenimiento/_Personas.cshtml");
        }
        public IActionResult Roles()
        {
            return PartialView("~/Views/Mantenimiento/_Roles.cshtml");
        }
        public IActionResult Grupo()
        {
            return PartialView("~/Views/Mantenimiento/_Grupo.cshtml");
        }
        public IActionResult Docente()
        {
            return PartialView("~/Views/Mantenimiento/_Docente.cshtml");
        }

        public IActionResult Decanato()
        {
            return PartialView("~/Views/Mantenimiento/_Decanatos.cshtml");
        }

        public IActionResult Modalidad()
        {
            return PartialView("~/Views/Mantenimiento/_Modalidad.cshtml");
        }
        public IActionResult Eventos()
        {
            return PartialView("~/Views/Mantenimiento/_TipoEvento.cshtml");
        }        
        public IActionResult Certificados()
        {
            return PartialView("~/Views/Mantenimiento/_Certificados.cshtml");
        }
        public IActionResult Ciclo()
        {
            return PartialView("~/Views/Mantenimiento/_Ciclo.cshtml");
        }
        public IActionResult Planificacion()
        {
            return PartialView("~/Views/Planificacion/_RegistrarPlanificacion.cshtml");
        }
        public IActionResult Expositor()
        {
            return PartialView("~/Views/Expositor/_RegistrarExpositor.cshtml");
        }
        public IActionResult VerFacilitador()
        {
            return PartialView("~/Views/Facilitador/VerFacilitador.cshtml");
        }
        public IActionResult Facilitador()
        {
            return PartialView("~/Views/Facilitador/Facilitador.cshtml");
        }
        public IActionResult ActasCalificacion()
        {
            return PartialView("~/Views/Actas/_ActasCalificacion.cshtml");
        }
        public IActionResult ActasAsistencia()
        {
            return PartialView("~/Views/Actas/_ActasAsistencia.cshtml");
        }
        public IActionResult MiPerfil()
        {
            return PartialView("~/Views/Config/_MiPerfil.cshtml");
        }
        public IActionResult Configuracion()
        {
            return PartialView("~/Views/Config/_Configuracion.cshtml");
        }
        public IActionResult Certificado()
        {
            return PartialView("~/Views/Certificacion/_Certificado.cshtml");
      
        }
        public IActionResult VerCertificados()
        {
            return PartialView("~/Views/Certificacion/VerCertificados.cshtml");

        }
        public IActionResult Estadistica()
        {
            return PartialView("~/Views/Certificacion/Estadistica.cshtml");

        }
        public IActionResult Formatos()
        {
            return PartialView("~/Views/Certificacion/_FormatoCertificado.cshtml");
        }
        public IActionResult AprobarParticipantes()
        {
            return PartialView("~/Views/Facilitador/_AprobarParticipantes.cshtml");
        }       
        public IActionResult GenerarCertificado()
        {
            return PartialView("~/Views/Certificacion/_GenerarCertificados.cshtml");
        }
    }
}
