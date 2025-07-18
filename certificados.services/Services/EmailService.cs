using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class EmailService
    {

        /**
         * Servicio que sirve para el envio de Email
         * **/
        private static string host = "smtp.gmail.com";
        private static int puerto = 587;
        public  ResponseApp SendEmail(string email, int tipo, Tevento evento, byte[]? pdf = null)
        {

            ResponseApp response = Utils.Utils.BadResponse(null);
            try
            {
                using (SmtpClient cliente = new SmtpClient(host, puerto))
                using (MailMessage correo = new MailMessage())
                {

                    correo.From = new MailAddress("tvboxtelevisor95@gmail.com");
                    correo.To.Add(email);
                    correo.Subject = tipo  == 1? "MATRICULACION DE ESTUDIANTES U. GUAYAQUIL": "NOTIFICACION DE CERTIFICADOS U. GUAYAQUIL";
                    correo.Body = tipo == 1 ? BodyMatriculacion(evento) : BodyEmicion(evento);
                    correo.IsBodyHtml = true;

                    if (tipo == 2 && pdf != null) {
                        correo.Attachments.Add(new Attachment(new MemoryStream(pdf), "Certificado.pdf", "application/pdf"));
                    }
                    cliente.Credentials = new NetworkCredential("tvboxtelevisor95@gmail.com", "orpt rnim gqeg vlfn");
                    cliente.EnableSsl = true;
                    cliente.Send(correo);
                    response = Utils.Utils.OkResponse($"CORREO ENVIADO A: {email}");

                }

            }
            catch (Exception ex)
            {
                response.Message = $"ERROR EN EL ENVIO DE MAIL {ex.Message}";
  
                Console.WriteLine($"ERROR AL ENVIAR MAIL: {ex.Message}");

            }

            return response;
        }
        private static string BodyMatriculacion(Tevento evento)
        {
            return $@"
                <html>
                <body>
                    <h1>Notificación de Matriculación</h1>
                    <p>Estimado usuario,</p>
                    <p>Le informamos que su proceso de matriculación en el evento <strong>{evento.Tematica}</strong> ha sido registrado con éxito.</p>
                    <p>Detalles del evento:</p>
                    <ul>
                        <li><strong>Lugar:</strong> {evento.Lugar}</li>
                        <li><strong>Fecha:</strong> {evento.FechaInicio:dd/MM/yyyy} - {evento.FechaFin:dd/MM/yyyy}</li>
                        <li><strong>Duración:</strong> {evento.Horas} horas</li>
                    </ul>
                    <p>¡Esperamos contar con su participación!</p>
                </body>
                </html>
            ";
        }

        private static string BodyEmicion(Tevento evento)
        {

            return $@"
                    <html>
                    <body>
                        <h1>Emisión de Certificado</h1>
                        <p>Estimado usuario,</p>
                        <p>Nos complace informarle que se ha emitido el certificado correspondiente a su participación en el evento <strong>{evento.Tematica}</strong>.</p>
                        <p>Detalles del evento:</p>
                        <ul>
                            <li><strong>Lugar:</strong> {evento.Lugar}</li>
                            <li><strong>Fecha:</strong> {evento.FechaInicio:dd/MM/yyyy} - {evento.FechaFin:dd/MM/yyyy}</li>
                            <li><strong>Duración:</strong> {evento.Horas} horas</li>
                        </ul>
                        <p>Adjunto encontrará su certificado en formato PDF.</p>
                        <p>Gracias por su participación.</p>
                    </body>
                    </html>
            ";
        }

        public ResponseApp SendEmailIndividual(string email, byte[]? pdf = null)
        {
            try
            {
                using (var cliente = new SmtpClient(host, puerto))
                using (var correo = new MailMessage())
                {
                    correo.From = new MailAddress("tvboxtelevisor95@gmail.com");
                    correo.To.Add(email);
                    correo.Subject = "NOTIFICACION DE CERTIFICADOS UNIVERSIDAD DE GUAYAQUIL";
                    correo.Body = BodyNotificacion();
                    correo.IsBodyHtml = true;

                    if (pdf != null)
                    {
                        correo.Attachments.Add(new Attachment(new MemoryStream(pdf), "CERTIFICADO.pdf", "application/pdf"));
                    }

                    cliente.Credentials = new NetworkCredential("tvboxtelevisor95@gmail.com", "orpt rnim gqeg vlfn");
                    cliente.EnableSsl = true;
                    cliente.Send(correo);

                    return Utils.Utils.OkResponse($"CORREO ENVIADO A: {email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL ENVIAR MAIL: {ex.Message}");
                return Utils.Utils.BadResponse($"ERROR EN EL ENVIO DE MAIL: {ex.Message}");
            }
        }

        private static string BodyNotificacion()
        {
            return $@"
                    <html>
                    <body>
                        <h1>Emisión de Certificado</h1>
                        <p>Estimado,</p>
                        </br>
                        <p>Nos complace informarle que se ha emitido el certificado correspondiente a su participación en el evento organizado por la Universidad de Guayaquil</strong>.</p>
                        </br>
                        <p>Adjunto encontrará su certificado en formato PDF.</p>
                        </br>
                        <p>Gracias por su participación.</p>
                    </body>
                    </html>
            ";
        }

    }

}
