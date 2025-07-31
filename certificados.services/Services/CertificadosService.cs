using certificados.dal.DataAccess;
using certificados.models;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace certificados.services.Services
{
    public class CertificadosService
    {
        private readonly TcertificadoDA tcertificadoDA;
        private readonly TeventoDA eventoDA;
        private readonly EmailService emailService;
        private readonly PersonaService personaService;
        private readonly PdfService pdfService;

        public CertificadosService(TcertificadoDA tcertificadoDA, PersonaService personaService, 
            EmailService emailService, TeventoDA eventoDA, PdfService pdfService)
        {
            this.tcertificadoDA = tcertificadoDA;
            this.eventoDA = eventoDA;
            this.emailService = emailService;
            this.personaService = personaService;
            this.pdfService = pdfService;
        }

        public ResponseApp ListarCertificados()
        {

            return tcertificadoDA.ListarCertificados();

        }

        public ResponseApp ListarCertificadosById(int idCertificado)
        {
            return tcertificadoDA.ListarCertificadosPorEvento(idCertificado);

        }
        public ResponseApp CertificadosById(int idCertificado)
        {
            return tcertificadoDA.CertificadosById(idCertificado);

        }

        public ResponseApp ObtenerCertificadosByEvento(int idCertificado)
        {


            return tcertificadoDA.BuscarCertificado(idCertificado);

        }

        public ResponseApp CrearCertificado(Tcertificado tcertificado)
        {

            ResponseApp response = Utils.Utils.BadResponse(null);
            return response;

        }

        public ResponseApp ActualizarCertificado(Tcertificado tcertificado)
        {

            ResponseApp response = Utils.Utils.BadResponse(null);
            if (!eventoDA.BuscarEvento(tcertificado.IdCertificado).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {

                response = Utils.Utils.BadResponse($"NO EXISTE EL EVENTO ASOCIADO");
            }
            else
            {
                response = tcertificadoDA.ModificarCertificado(tcertificado);
            }

            return response;
        }

        public ResponseApp ElminarCertificado(int idCertificado)
        {

            return tcertificadoDA.EliminarCertificado(idCertificado);

        }
        public ResponseApp Notificar(Tevento tevento, List<Tpersona> listaPersonas)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<string> notificados = new List<string>();
            List<string> Nonotificados = new List<string>();

            foreach (var persona in listaPersonas)
            {

                try
                {
                    var PersonaRquest = personaService.ObtenerPersona(persona.Cedula);
                    if (!PersonaRquest.Cod.Equals(Utils.CONSTANTES.COD_OK))
                    {
                        Nonotificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                    string jsonString = JsonSerializer.Serialize(PersonaRquest);

                    var json = JsonDocument.Parse(jsonString);

                    string email = json.RootElement.
                        GetProperty("data")
                        .GetProperty("mDatos")
                        .GetProperty("Email").GetString();
                     
                    var sendEmail = emailService.SendEmail(email, 1, tevento);
                    if (sendEmail.Cod.Equals(Utils.CONSTANTES.COD_OK))
                    {

                        notificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                    else
                    {
                        Nonotificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Nonotificados.Add(persona.Nombres + " " + persona.Apellidos + $" (Error: {ex.Message})");
                }
            }

            dict.Add("Notificados", notificados);
            dict.Add("NoNotificados", Nonotificados);

            return Utils.Utils.OkResponse(dict);
        }

        public ResponseApp Emitir(Tevento tevento, List<Tpersona> listaPersonas, Tcertificado certificado, List<Tdocente> docentes, Tdecanato decanato)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<string> notificados = new List<string>();
            List<string> Nonotificados = new List<string>();

            foreach (var persona in listaPersonas)
            {

                try
                {
                    var PersonaRquest = personaService.ObtenerPersona(persona.Cedula);
                    if (!PersonaRquest.Cod.Equals(Utils.CONSTANTES.COD_OK))
                    {
                        Nonotificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                    string jsonString = JsonSerializer.Serialize(PersonaRquest);

                    var json = JsonDocument.Parse(jsonString);

                    string email = json.RootElement.
                        GetProperty("data")
                        .GetProperty("mDatos")
                        .GetProperty("Email").GetString();

                    var pdfCreate = pdfService.GenerarCertificado(certificado.TformatoCertificado, tevento, persona, docentes, decanato);
                     
                    var sendEmail = emailService.SendEmail(email, 2, tevento, pdfCreate);
                    if (sendEmail.Cod.Equals(Utils.CONSTANTES.COD_OK))
                    {

                        notificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                    else
                    {
                        Nonotificados.Add(persona.Nombres + " " + persona.Apellidos);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Nonotificados.Add(persona.Nombres + " " + persona.Apellidos + $" (Error: {ex.Message})");
                }
            }

            dict.Add("Notificados", notificados);
            dict.Add("NoNotificados", Nonotificados);

            return Utils.Utils.OkResponse(dict);
        }
        public ResponseApp EnviarCertificadoIndividual(string cedula, byte[]? pdf)
        {
            var persona = personaService.ObtenerPersona(cedula);
            if (persona == null)
            {
                return Utils.Utils.BadResponse("Persona no encontrada");
            }

            string jsonString = JsonSerializer.Serialize(persona);
            var json = JsonDocument.Parse(jsonString);
            string email = json.RootElement.GetProperty("data").GetProperty("mDatos").GetProperty("Email").GetString();

            if (string.IsNullOrEmpty(email))
            {
                return Utils.Utils.BadResponse("Correo electrónico no disponible");
            }

            return Utils.Utils.OkResponse(emailService.SendEmailIndividual(email, pdf));
        }
        public  ResponseApp GetFiltros()
        {                        
            return Utils.Utils.OkResponse(tcertificadoDA.GetFiltros());
        }

        public ResponseApp GetEstadistica(FiltroEstadistica filtro)
        {
            return Utils.Utils.OkResponse(tcertificadoDA.GetEstadistica(filtro));
        }
    }

   
}
