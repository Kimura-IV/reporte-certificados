using certificados.dal.DataAccess;
using certificados.models;
using certificados.models.Dtos;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public ResponseApp GetReporteCertificado(FiltroReporteDto filtro)
        {
            return Utils.Utils.OkResponse(tcertificadoDA.GetReporteCertificado(filtro));
        }
        public ResponseApp GetEstadistica(FiltroReporteDto filtro)
        {
            return Utils.Utils.OkResponse(tcertificadoDA.GetEstadistica(filtro));
        }
        public byte[] GenerarExcelReporte(FiltroReporteDto filtro)
        {
            byte[] data = null;
            var result = tcertificadoDA.GetReporteCertificado(filtro);

                ExcelPackage.License.SetNonCommercialPersonal("<Your Name>");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"Reporte-Certificado");
                worksheet.Cells[1,1].Value = "Estado";
                worksheet.Cells[1,2].Value = "FCreacion";
                worksheet.Cells[1,3].Value = "FModificacion";
                worksheet.Cells[1,4].Value = "Formato";         
                worksheet.Cells[1,5].Value = "Tipo";
                worksheet.Cells[1,6].Value = "Titulo";
                worksheet.Cells[1,7].Value = "NombreFirmanteUno";
                worksheet.Cells[1,8].Value = "NombreFirmanteDos";
                worksheet.Cells[1,9].Value = "NombreFirmanteTres";
                for(int i = 0; i <result.Count; i++)
                {
                    worksheet.Cells[i+2, 1].Value = result[i].Estado;
                    worksheet.Cells[i+2, 2].Value = result[i].FCreacion;
                    worksheet.Cells[i+2, 3].Value = result[i].FModificacion;
                    worksheet.Cells[i+2, 4].Value = result[i].NombreFormato;
                    worksheet.Cells[i+2, 5].Value = result[i].Tipo;
                    worksheet.Cells[i+2, 6].Value = result[i].Titulo;   
                    worksheet.Cells[i+2, 7].Value = result[i].NombreFirmanteUno;
                    worksheet.Cells[i+2, 8].Value = result[i].NombreFirmanteDos;
                    worksheet.Cells[i+2, 9].Value = result[i].NombreFirmanteTres;
                }                        
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                var headerRange = worksheet.Cells[1, 1, 1, 11];
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                data = package.GetAsByteArray();
            }
            return data;
        }   

        public byte[] GenerarCertificadoEstadistica(EstadisticaReporteExcel estadisticaReporteExcel)
        {
            var estadisticas = tcertificadoDA.GetEstadistica(estadisticaReporteExcel.Filtro);
            ExcelPackage.License.SetNonCommercialPersonal("<Your Name>");

            using var package = new ExcelPackage();

            int row = 1;
           foreach (var grafico in estadisticaReporteExcel.Graficos)
            {
                if (grafico.Equals("plantilla")){
                    var ws = package.Workbook.Worksheets.Add("Plantillas");

                    // === Pie Chart - Plantillas ===
                    ws.Cells[row, 1].Value = "Plantilla";
                    ws.Cells[row, 2].Value = "Cantidad";
                    for (int i = 0; i < estadisticas.Plantillas.Count; i++)
                    {
                        ws.Cells[row + i + 1, 1].Value = estadisticas.Plantillas[i].NombrePlantilla;
                        ws.Cells[row + i + 1, 2].Value = estadisticas.Plantillas[i].Count;
                    }

                    var pieChart = ws.Drawings.AddChart("chart_pie", eChartType.PieExploded3D) as ExcelPieChart;
                    pieChart.Title.Text = "Distribución por Plantilla";
                    pieChart.SetPosition(0, 0, 3, 0);
                    pieChart.SetSize(500, 400);
                    pieChart.Series.Add(ws.Cells[row + 1, 2, row + estadisticas.Plantillas.Count, 2],
                                        ws.Cells[row + 1, 1, row + estadisticas.Plantillas.Count, 1]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                }
                if (grafico.Equals("dia"))
                {
                    var ws = package.Workbook.Worksheets.Add("Dias");

                    // === Line Chart - Lapso por Día ===
                    ws.Cells[row, 1].Value = "Fecha";
                    ws.Cells[row, 2].Value = "Cantidad";
                    for (int i = 0; i < estadisticas.LapsoDias.Count; i++)
                    {
                        ws.Cells[row + i + 1, 1].Value = estadisticas.LapsoDias[i].Dia;
                        ws.Cells[row + i + 1, 2].Value = estadisticas.LapsoDias[i].Count;
                    }

                    ws.Column(1).Style.Numberformat.Format = "yyyy-mm-dd";

                    var lineChart = ws.Drawings.AddChart("chart_line", eChartType.LineMarkers) as ExcelLineChart;
                    lineChart.Title.Text = "Cantidad por Día";
                    lineChart.SetPosition(0, 0, 3, 0);
                    lineChart.SetSize(600, 300);
                    lineChart.Series.Add(ws.Cells[row + 1, 2, row + estadisticas.LapsoDias.Count, 2],
                                         ws.Cells[row + 1, 1, row + estadisticas.LapsoDias.Count, 1]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                }
                if (grafico.Equals("anio"))
                {
                    var ws = package.Workbook.Worksheets.Add("Años");

                    // === Line Chart - Lapso por Anio ===
                    ws.Cells[row, 1].Value = "Fecha";
                    ws.Cells[row, 2].Value = "Cantidad";
                    for (int i = 0; i < estadisticas.LapsoAnios.Count; i++)
                    {
                        ws.Cells[row + i + 1, 1].Value = estadisticas.LapsoAnios[i].Anio;
                        ws.Cells[row + i + 1, 2].Value = estadisticas.LapsoAnios[i].Count;
                    }

      

                    var lineChart = ws.Drawings.AddChart("chart_line", eChartType.LineMarkers) as ExcelLineChart;
                    lineChart.Title.Text = "Cantidad por Año";
                    lineChart.SetPosition(0, 0, 3, 0);
                    lineChart.SetSize(600, 300);
                    lineChart.Series.Add(ws.Cells[row + 1, 2, row + estadisticas.LapsoAnios.Count, 2],
                                         ws.Cells[row + 1, 1, row + estadisticas.LapsoAnios.Count, 1]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                }
                if (grafico.Equals("semana"))
                {
                    var ws = package.Workbook.Worksheets.Add("Semanas");

                    // === Line Chart - Lapso por Anio ===
                    ws.Cells[row, 1].Value = "Fecha";
                    ws.Cells[row, 2].Value = "Cantidad";
                    for (int i = 0; i < estadisticas.LapsoSemanas.Count; i++)
                    {
                        ws.Cells[row + i + 1, 1].Value = estadisticas.LapsoSemanas[i].SemanaInicio;
                        ws.Cells[row + i + 1, 2].Value = estadisticas.LapsoSemanas[i].Count;
                    }

                    ws.Column(1).Style.Numberformat.Format = "yyyy-mm-dd";

                    var lineChart = ws.Drawings.AddChart("chart_line", eChartType.LineMarkers) as ExcelLineChart;
                    lineChart.Title.Text = "Cantidad por Semana";
                    lineChart.SetPosition(0, 0, 3, 0);
                    lineChart.SetSize(600, 300);
                    lineChart.Series.Add(ws.Cells[row + 1, 2, row + estadisticas.LapsoSemanas.Count, 2],
                                         ws.Cells[row + 1, 1, row + estadisticas.LapsoSemanas.Count, 1]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                }
                if (grafico.Equals("firmante"))
                {
                    var ws = package.Workbook.Worksheets.Add("Firmantes");

                    // === Bar Chart - Firmantes ===
                    ws.Cells[row, 1].Value = "Firmante";
                    ws.Cells[row, 2].Value = "Cantidad";
                    for (int i = 0; i < estadisticas.Firmantes.Count; i++)
                    {
                        ws.Cells[row + i + 1, 1].Value = estadisticas.Firmantes[i].NombreFirmante;
                        ws.Cells[row + i + 1, 2].Value = estadisticas.Firmantes[i].Count;
                    }

                    var barChart = ws.Drawings.AddChart("chart_bar", eChartType.ColumnClustered) as ExcelBarChart;
                    barChart.Title.Text = "Firmantes";
                    barChart.SetPosition(0, 0, 3, 0);
                    barChart.SetSize(600, 400);
                    barChart.Series.Add(ws.Cells[row + 1, 2, row + estadisticas.Firmantes.Count, 2],
                                        ws.Cells[row + 1, 1, row + estadisticas.Firmantes.Count, 1]);
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                }
            }
            return package.GetAsByteArray();
        }
    }
}
