using certificados.dal.DataAccess;
using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Services;
using certificados.services.Utils;
using certificados.web.Controllers.Mappers;
using certificados.web.Models.DTO;
using iText.Commons.Actions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace certificados.web.Controllers
{
    [Route("api/certificado")]
    public class CertificadoController : Controller
    {
        private readonly CertificadosService certificadosService;
        private readonly EventoService eventoService;
        private readonly FormatoCertificadoService formatoCertificadoService;
        private readonly GrupoService grupoService;
        private readonly GrupoPersonaService grupoPersonaService;
        private readonly DocenteService docenteService;
        private readonly PersonaService personaService;
        private readonly DecanatoService decanatoService;
        private readonly AppDbContext context;

        public CertificadoController(CertificadosService certificadosService, EventoService evento, GrupoService grupoService,
            GrupoPersonaService grupoPersonaService, DocenteService docenteService, FormatoCertificadoService formato, PersonaService personaService, 
            DecanatoService decanatoService, AppDbContext context)
        {
            this.certificadosService = certificadosService;
            this.eventoService = evento;
            this.formatoCertificadoService = formato;
            this.grupoPersonaService = grupoPersonaService;
            this.grupoService = grupoService;
            this.docenteService = docenteService;
            this.personaService = personaService;
            this.decanatoService = decanatoService;
            this.context = context;
        }

        /*
         * Endpoint para crear un CERTIFICADO
         */
        [HttpPost("crear")]
        public ActionResult<ResponseApp> crearCertificado([FromBody] Dictionary<string, object> requestBody)
        {

            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var titulo = (JsonElement)requestBody["Titulo"];
            var imagen = (JsonElement)requestBody["Imagen"];
            var idEvento = (JsonElement)requestBody["IdEvento"];
            var idFormato = (JsonElement)requestBody["IdFormato"];
            var tipo = (JsonElement)requestBody["Tipo"];
            var estado = (JsonElement)requestBody["Estado"];
            var usuarioIngreso = (JsonElement)requestBody["UsuarioIngreso"];

            var eventoResponse = eventoService.ListarPorId(int.Parse(idEvento.ToString()));
            if (!eventoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse("NO EXISTE EL EVENTO");
            }

            var formatoResponse = formatoCertificadoService.ListarFormatoByID(int.Parse(idFormato.ToString()));
            if (!formatoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse("NO EXISTE EL FORMATO");
            }
            Tevento evento = EventoMapper.convertEntity(eventoResponse.Data);
            TformatoCertificado formato = FormatoCertificadoMapper.convertEntity(formatoResponse.Data);

            var tcertificado = new Tcertificado
            {
                Titulo = titulo.ToString(),
                Imagen = imagen.GetBytesFromBase64(),
                IdFormato = int.Parse(idFormato.ToString()),
                Tipo = tipo.GetString(),
                Estado = true,
                UsuarioIngreso = usuarioIngreso.ValueKind == JsonValueKind.Number ? usuarioIngreso.GetInt32().ToString() : usuarioIngreso.GetString(),
                TformatoCertificado = formato
            };

            return Ok(certificadosService.CrearCertificado(tcertificado));
        }

        /*
          * Endpoint para MODIFICAR un CERTIFICADO
          */
        [HttpPost("modificar")]
        public ActionResult<ResponseApp> modificarCertificado([FromBody] Dictionary<string, object> requestBody)
        {

            if (requestBody == null || !requestBody.Any())
            {
                return Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS);
            }

            var idCertificado = (JsonElement)requestBody["idCertificado"];
            var titulo = (JsonElement)requestBody["Titulo"];
            var idEvento = (JsonElement)requestBody["IdEvento"];
            var idFormato = (JsonElement)requestBody["IdFormato"];
            var tipo = (JsonElement)requestBody["Tipo"];
            var estado = (JsonElement)requestBody["Estado"];
            var userModificacion = (JsonElement)requestBody["UserModificacion"];


            var eventoResponse = eventoService.ListarPorId(int.Parse(idEvento.ToString()));
            if (!eventoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse("NO EXISTE EL EVENTO");
            }

            var formatoResponse = formatoCertificadoService.ListarFormatoByID(int.Parse(idFormato.ToString()));
            if (!formatoResponse.Cod.Equals(CONSTANTES.COD_OK))
            {
                return Utils.BadResponse("NO EXISTE EL FORMATO");
            }
            Tevento evento = EventoMapper.convertEntity(eventoResponse.Data);
            TformatoCertificado formato = FormatoCertificadoMapper.convertEntity(formatoResponse.Data);
            var tcertificado = new Tcertificado
            {
                IdCertificado = int.Parse(idCertificado.ToString()),
                Titulo = titulo.ToString(),
                IdFormato = int.Parse(idFormato.ToString()),
                Tipo = tipo.ToString(),
                Estado = System.Boolean.Parse(estado.ToString()),
                UsuarioActualizacion = userModificacion.ValueKind == JsonValueKind.Number ? userModificacion.GetInt32().ToString() : userModificacion.GetString(),
                TformatoCertificado = formato
            };
            return Ok(certificadosService.ActualizarCertificado(tcertificado));
        }
        /*
         * Endpoint para LISTATAR CERTIFICADO
         */
        [HttpGet("all")]
        public ActionResult<ResponseApp> obtenerCertificados()
        {
            return Ok(certificadosService.ListarCertificados());
        }

        [HttpPost("obtener")]
        public ActionResult<ResponseApp> getCertificates([FromBody] FiltroCertificadoDTO filtro)
        {
            var iQueryableCertificado = GenerateIQueryable(filtro);
            var data = iQueryableCertificado.Select(x => new
            {
               
                
                    x.Estado,
                    x.FCreacion,
                    x.FModificacion,
                    x.IdCertificado,
                    x.IdFormato,
                    x.TformatoCertificado,
                    x.Tipo,
                    x.Titulo,
                    x.UsuarioActualizacion,
                    x.UsuarioIngreso,
                    x.TformatoCertificado.NombreFirmanteUno,
                    x.TformatoCertificado.NombreFirmanteDos,
                    x.TformatoCertificado.NombreFirmanteTres,
                    pdfBase64 = Convert.ToBase64String(x.Imagen)               
            }).ToList();
            return Ok(Utils.OkResponse(data));
        }
        [HttpGet("GetFiltrosCertificados")]
        public ActionResult<ResponseApp> GetFiltrosCertificados()
        {                
            return Ok(certificadosService.GetFiltros());
        }
        /*
         * Endpoint para LISTATAR CERTIFICADO
         */
        [HttpPost("id")]
        public ActionResult<ResponseApp> obtenerCertificadosById([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idCertificado", out var idCertificadoObj) || idCertificadoObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idCertificadoObj.ToString(), out int idCertificado))
            {
                return BadRequest(Utils.BadResponse("ID CERTIFICADO NO VÁLIDO"));
            }
            return Ok(certificadosService.ListarCertificadosById(idCertificado));
        }

        /*
         * Endpoint para LISTATAR CERTIFICADO POR EVENTO
         */
        [HttpPost("porEvento")]
        public ActionResult<ResponseApp> obtenerCertificadosByEvento([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idEvento", out var idEventoObj) || idEventoObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idEventoObj.ToString(), out int idEvento))
            {
                return BadRequest(Utils.BadResponse("ID CERTIFICADO NO VÁLIDO"));
            }
            return Ok(certificadosService.ObtenerCertificadosByEvento(idEvento));
        }
        /*
         * Endpoint para eliminar un CERTIFICADO
         */
        [HttpPost("eliminar")]
        public ActionResult<ResponseApp> eliminarCertificado([FromBody] Dictionary<string, object> request)
        {

            if (!request.TryGetValue("idCertificado", out var idCertificadoObj) || idCertificadoObj == null)
            {
                return BadRequest(Utils.BadResponse("FALTAN PARAMETROS"));
            }
            if (!int.TryParse(idCertificadoObj.ToString(), out int idCertificado))
            {
                return BadRequest(Utils.BadResponse("ID CERTIFICADO NO VÁLIDO"));
            }
            return Ok(certificadosService.ElminarCertificado(idCertificado));
        }
       
        [HttpPost("email/notificar")]
        public ActionResult<ResponseApp> enviarMail([FromBody] Dictionary<string, object> request)
        {
            if (!request.TryGetValue("idEvento", out var idEventoObj) || idEventoObj == null)
            {
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            }
            if (!int.TryParse(idEventoObj.ToString(), out int idEvento))
            {
                return BadRequest(Utils.BadResponse("ID GRUPO NO VÁLIDO"));
            }
            var evento = eventoService.ListarPorId(idEvento);
            if (!evento.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("NO EXISTE EL EVENTO"));
            }
            Tevento tevento = EventoMapper.convertEntity(evento.Data);
            var grupo = grupoService.BuscarGrupo(tevento.IdGrupo);
            if (!grupo.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("NO EXISTE EL GRUPO"));
            }

            var grupoResponse = grupoPersonaService.BuscarById(tevento.IdGrupo);
            List<Tpersona> listaPersonas = GrupoPersonaMapper.listadoPersonas(grupoResponse.Data);


            return certificadosService.Notificar(tevento, listaPersonas);
        }

        [HttpPost("emitir")]
        public ActionResult<ResponseApp> Emitir([FromBody] EmitirCertificadoDTO dto)
        {
            if (dto == null)
                return BadRequest(Utils.BadResponse(CONSTANTES.MESSAGE_DATA_ERRORS));
            
            var certificadoRq = certificadosService.CertificadosById(dto.idCertificado);
            if (!certificadoRq.Cod.Equals(CONSTANTES.COD_OK))
            {
                return BadRequest(Utils.BadResponse("NO EXISTE EL EVENTO"));
            }
            Tcertificado certificado = CertificadoMapper.toEntity(certificadoRq.Data);

            List<Tdocente> Listadocente = new List<Tdocente>();
            foreach (var docent in dto.docentes) {
                var requestDocente = docenteService.ObtenerDocentesByCedula(docent);
                if (!requestDocente.Cod.Equals(CONSTANTES.COD_OK))
                    continue;
                Tdocente docente = DocenteMapper.toEntity(requestDocente.Data);
                Listadocente.Add(docente);
                
            }

            return new ResponseApp();
            // return certificadosService.Notificar(tevento, listaPersonas);
        }

        [HttpPost("generarcertificado")]
        public async Task<ActionResult<ResponseApp>> GenerarCertificado([FromBody] Dictionary<string, object> request)
        {
            bool isEnviarEmail = false;

            try
            {
                // Validar que los parámetros requeridos estén presentes
                if (!request.ContainsKey("cedula") || !request.ContainsKey("idFormato") || !request.ContainsKey("idDecanato"))
                {
                    return BadRequest(Utils.BadResponse("Faltan parámetros requeridos en la solicitud."));
                }

                if (request.ContainsKey("isEnviarEmail"))
                {
                    var isEnviarEmailProperty = (JsonElement)request["isEnviarEmail"];
                    if (isEnviarEmailProperty.ValueKind == JsonValueKind.True || isEnviarEmailProperty.ValueKind == JsonValueKind.False)
                    {
                        isEnviarEmail = isEnviarEmailProperty.GetBoolean();
                    }
                }

                // Extraer y convertir los valores del request
                var cedula = ((JsonElement)request["cedula"]).ToString();
                var idFormato = int.Parse(((JsonElement)request["idFormato"]).ToString());
                var idDecanato = int.Parse(((JsonElement)request["idDecanato"]).ToString());

                // Obtener datos necesarios
                var persona = personaService.buscarPersonaPorCedula(cedula);
                var formato = formatoCertificadoService.ListarFormatoByID(idFormato);
                var decanatoResponse = decanatoService.ObtenerDecanatoById(idDecanato);

                // Validar que los datos necesarios se encontraron
                if (persona?.Data == null || formato?.Data == null || decanatoResponse?.Data == null)
                {
                    return BadRequest(Utils.BadResponse("No se encontraron los datos necesarios para generar el certificado."));
                }

                dynamic dataPersona = persona.Data;
                dynamic dataFormato = formato.Data;
                dynamic dataDecanato = decanatoResponse.Data;

                using (var memoryStream = new MemoryStream())
                {
                    var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
                    var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Agregar imagen de fondo si existe
                    if (dataFormato.LineaGrafica != null)
                    {
                        var backgroundImage = iTextSharp.text.Image.GetInstance((byte[])dataFormato.LineaGrafica);
                        backgroundImage.ScaleAbsolute(document.PageSize.Width, document.PageSize.Height);
                        backgroundImage.SetAbsolutePosition(0, 0);
                        writer.DirectContentUnder.AddImage(backgroundImage);
                    }

                    // Agregar logo si existe
                    if (dataFormato.LogoUG != null)
                    {
                        var logoImage = iTextSharp.text.Image.GetInstance((byte[])dataFormato.LogoUG);
                        logoImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        //logoImage.ScaleAbsolute(250f, 60f);
                        document.Add(logoImage);
                        agregarSaltodeLinea(document,1);
                    }

                    // Agregar nombre del decanato
                    document.Add(new iTextSharp.text.Paragraph(dataDecanato.Nombre, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, new BaseColor(39, 72, 104)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    });
                    agregarSaltodeLinea(document, 1);

                    // Agregar texto
                    document.Add(new iTextSharp.text.Paragraph("Confieren el Presente", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(4, 60, 127)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    });

                    // Agregar título del certificado
                    document.Add(new iTextSharp.text.Paragraph(dataFormato.Tipo + " a:", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 30, iTextSharp.text.Font.BOLD, new BaseColor(4, 60, 127)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    });
                    agregarSaltodeLinea(document, 1);

                    // Agregar nombre de la persona
                    document.Add(new iTextSharp.text.Paragraph($"{dataPersona.Apellidos} {dataPersona.Nombres}", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.NORMAL, new BaseColor(11, 48, 90)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    });
                    agregarSaltodeLinea(document, 1);

                    // Agregar descripción del formato
                    document.Add(new iTextSharp.text.Paragraph(dataFormato.Leyenda, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(39, 72, 104)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    });
                    agregarSaltodeLinea(document, 1);

                    // Agregar fecha formateada
                    string fechaFormateada = $"Guayaquil, {DateTime.Now.Day} de {DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"))} de {DateTime.Now.Year}";
                    document.Add(new iTextSharp.text.Paragraph(fechaFormateada, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new BaseColor(39, 72, 104)))
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_RIGHT
                    });
                    agregarSaltodeLinea(document, 4);

                    // Agregar QR si existe
                    if (dataFormato.Qr != null)
                    {
                        var qrImage = iTextSharp.text.Image.GetInstance((byte[])dataFormato.Qr);
                        qrImage.ScaleAbsolute(80, 80);
                        qrImage.SetAbsolutePosition(50, 80); // Posición X, Y para el QR
                        document.Add(qrImage);
                    }

                    // Posiciones iniciales para los firmantes
                    float startX = 300; // Posición X inicial para el primer firmante
                    float startY = 100; // Posición Y común para todos los firmantes
                    float lineHeight = 20;
                    float spacing = 180; // Espacio horizontal entre firmantes

                    // Crear lista de firmantes
                    var firmantes = new[]
                    {
                        (dataFormato.NombreFirmanteUno, dataFormato.CargoFirmanteUno),
                        (dataFormato.NombreFirmanteDos, dataFormato.CargoFirmanteDos),
                        (dataFormato.NombreFirmanteTres, dataFormato.CargoFirmanteTres)
                    };

                    // Agregar firmantes al documento
                    foreach (var firmante in firmantes)
                    {
                        var nombreFirmante = new Paragraph(firmante.Item1, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(11, 48, 90)))
                        {
                            Alignment = Element.ALIGN_CENTER
                        };

                        var cargoFirmante = new Paragraph(firmante.Item2, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new BaseColor(39, 72, 104)))
                        {
                            Alignment = Element.ALIGN_CENTER
                        };

                        // Posicionar nombre del firmante
                        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(nombreFirmante), startX, startY, 0);

                        // Posicionar cargo del firmante
                        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(cargoFirmante), startX, startY - lineHeight, 0);

                        // Mover la posición X para el siguiente firmante
                        startX += spacing;
                    }

                    document.Close();

                    // Convertir el PDF a base64 y retornar la respuesta
                    var pdfBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(pdfBytes);

                    TformatoCertificado formatoEntity = FormatoCertificadoMapper.convertEntity(formato.Data);

                    var idUsuario = ((JsonElement)request["idUsuario"]).ToString();

                    var tcertificado = new Tcertificado
                    {
                        IdCertificado = context.Tcertificado.Count() + 1,
                        Titulo = $"{dataPersona.Apellidos} {dataPersona.Nombres}",
                        Imagen = pdfBytes,
                        IdFormato = idFormato,
                        Tipo = "tipo",
                        Estado = true,
                        FCreacion = Utils.timeParsed(DateTime.Now),
                        FModificacion = Utils.timeParsed(DateTime.Now),
                        UsuarioActualizacion = idUsuario,
                        UsuarioIngreso = idUsuario,
                        TformatoCertificado = dataFormato
                    };
                    await context.Tcertificado.AddAsync(tcertificado);
                    await context.SaveChangesAsync();

                    if (isEnviarEmail)
                    {
                        certificadosService.EnviarCertificadoIndividual(cedula, pdfBytes);
                        return Ok(new ResponseApp { Cod = "OK", Message = "CERTIFICADO ENVIADO AL CORREO ELECTRÓNICO CON ÉXITO", Data = "" });
                    }
                    else 
                    {
                        return Ok(new ResponseApp { Cod = "OK", Message = "CERTIFICADO GENERADO CON ÉXITO", Data = base64String });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(Utils.BadResponse($"Error al generar el certificado: {ex.Message}"));
            }
        }

        private void agregarSaltodeLinea(Document document, int count)
        {
            for (int i = 0; i < count; i++)
            {
                document.Add(new Paragraph("\n"));
            }
        }
        private IQueryable<Tcertificado> GenerateIQueryable(FiltroCertificadoDTO filtro)
        {
            var certificado = context.Tcertificado.AsQueryable();
            if (filtro == null) return certificado;


            var predicate = PredicateBuilder.New<Tcertificado>(true);
            if (!string.IsNullOrEmpty(filtro.Tipo))
            {
                var tipoUpper = filtro.Tipo.ToUpper();
                predicate = predicate.And(x => !string.IsNullOrEmpty(x.Tipo) && x.Tipo.ToUpper() == tipoUpper);
            }

            if (filtro.Estado != null)
            {
                predicate = predicate.And(x => x.Estado == filtro.Estado);
            }

            if (filtro.Emision != null)
            {
                predicate = predicate.And(x => x.FCreacion >= filtro.Emision.Value.Date && x.FCreacion < filtro.Emision.Value.Date.AddDays(1));
            }

            if (filtro.Plantilla != 0)
            {
                predicate = predicate.And(x => x.IdFormato == filtro.Plantilla);
            }

            if (!string.IsNullOrEmpty(filtro.Creador))
            {
                var creadorUpper = filtro.Creador.ToUpper();
                predicate = predicate.And(x => !string.IsNullOrEmpty(x.UsuarioIngreso) && x.UsuarioIngreso == creadorUpper);
            }
            if (!string.IsNullOrEmpty(filtro.Firmante))
            {
                var firmanteUpper = filtro.Firmante.ToUpper();
                predicate = predicate.And(x =>
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteUno) &&  x.TformatoCertificado.CargoFirmanteUno.ToUpper().Equals(firmanteUpper) ||
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteDos) && x.TformatoCertificado.CargoFirmanteDos.ToUpper().Equals(firmanteUpper) ||
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteTres) && x.TformatoCertificado.CargoFirmanteTres.ToUpper().Equals(firmanteUpper));
            }

            return certificado.AsExpandable().Where(predicate);
        }

    }
}
