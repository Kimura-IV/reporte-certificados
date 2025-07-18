using certificados.models.Entitys.dbo;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace certificados.services.Services
{
    public class PdfService
    {
         
        public  byte[] GenerarCertificado(TformatoCertificado formato, Tevento evento, Tpersona persona, List< Tdocente >docentes, Tdecanato decanato) {

            using (MemoryStream ms = new MemoryStream()) {
                Document documento = new Document(new iTextSharp.text.Rectangle(842, 595));
                PdfWriter writer = PdfWriter.GetInstance(documento, ms);
                documento.Open();

                PdfContentByte cb = writer.DirectContentUnder;
                AgregarBackGround(cb, formato.LineaGrafica, documento.PageSize);

                if (formato.LogoUG != null && formato.LogoUG.Length > 0)
                {
                    byte[] imageBytes = formato.LogoUG;
                    using (MemoryStream imgMs = new MemoryStream(imageBytes))
                    {
                        Image logo = Image.GetInstance(imgMs);
                        float logoHeight = 90f;
                        float marginBottom = 40f;
                        float ratio = logo.Width / logo.Height;
                        logo.ScaleAbsolute(logoHeight * ratio, logoHeight);
                        logo.SetAbsolutePosition(
                            (documento.PageSize.Width - (logoHeight * ratio)) / 2,
                            documento.PageSize.Height - 120);
                        documento.Add(logo);
                        documento.Add(new Paragraph(" ") { SpacingAfter = marginBottom });
                    }
                }
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));
                AgregarTexto(documento, "La " + decanato.Nombre, 18, true, Element.ALIGN_CENTER);
                AgregarTexto(documento, "Confieren el presente", 15, true, Element.ALIGN_CENTER);
                documento.Add(new Paragraph(" ")); 

                AgregarTexto(documento, "CERTIFICADO ", 25, true, Element.ALIGN_CENTER);
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));

                // Contenido principal
                AgregarTexto(documento, persona.Nombres.ToUpper() +" " +  persona.Apellidos.ToUpper(), 19, true, Element.ALIGN_CENTER);
                documento.Add(new Paragraph(" "));
                AgregarTexto(documento, $"Por haber aprobado el " + evento.TtipoEvento.Nombre.ToUpper() + " de " + evento.Dominio.ToUpper()  + " con una duración de " + evento.Horas  + $" horas, realizado desde {evento.FechaInicio:dd 'de' MMMM 'del' yyyy}  hasta el {evento.FechaFin:dd 'de' MMMM 'del' yyyy}.", 15 , false, Element.ALIGN_CENTER);

                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));
                AgregarTexto(documento, $"Dado en Guayaquil, el {DateTime.Now:dd 'de' MMMM 'del' yyyy}.", 11, false, Element.ALIGN_RIGHT);
                documento.Add(new Paragraph(" "));


                PdfContentByte contenido = writer.DirectContent;
                float startY = 120f; 
                float lineWidth = 150f;

   
                float centroPagina = documento.PageSize.Width / 2;
                float espacioEntreFirmas = 200f;
                float startX = centroPagina - ((Math.Min(docentes.Count, 3) * espacioEntreFirmas) / 2);

                for (int i = 0; i < docentes.Count && i < 3; i++)
                {
                    float xPos = startX + (espacioEntreFirmas * i);
                    float espacioEntreElementos = 25f;

                    contenido.MoveTo(xPos, startY);
                    contenido.LineTo(xPos + lineWidth, startY);
                    contenido.Stroke();

                    Paragraph firmaNombre = new Paragraph(docentes[i].Tpersona.Nombres,
                        FontFactory.GetFont(FontFactory.HELVETICA, 11, Font.NORMAL));
                    firmaNombre.Alignment = Element.ALIGN_CENTER;

                    ColumnText ct = new ColumnText(contenido);
                    ct.SetSimpleColumn(xPos, startY - espacioEntreElementos, xPos + lineWidth, startY - 5);
                    ct.AddElement(firmaNombre);
                    ct.Go();

                    if (!string.IsNullOrEmpty(docentes[i].Tpersona.Nombres))
                    {
                        Paragraph firmaCargo = new Paragraph(docentes[i].Titulo,
                            FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.ITALIC));
                        firmaCargo.Alignment = Element.ALIGN_CENTER;
                        ct = new ColumnText(contenido);
                        ct.SetSimpleColumn(xPos, startY - (espacioEntreElementos * 2), xPos + lineWidth, startY - espacioEntreElementos);
                        ct.AddElement(firmaCargo);
                        ct.Go();
                    }
                }

                documento.Close();
                return ms.ToArray();

            }
        }

        private void AgregarBackGround(PdfContentByte cb, byte[] marcaAguaBase64, iTextSharp.text.Rectangle pageSize, float opacidad = 1f)
        {
            if (marcaAguaBase64 == null || marcaAguaBase64.Length <= 0)
            {
                return;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(marcaAguaBase64))
                {
                    Image marca = Image.GetInstance(ms);

                    // Calculamos proporciones para mantener el aspect ratio
                    float imageAspect = marca.Width / marca.Height;
                    float pageAspect = pageSize.Width / pageSize.Height;
                     
                    if (imageAspect > pageAspect)
                    {
                        marca.ScaleAbsoluteHeight(pageSize.Height);
                        marca.ScaleAbsoluteWidth(pageSize.Height * imageAspect);
                    }
                    else
                    {
                        marca.ScaleAbsoluteWidth(pageSize.Width);
                        marca.ScaleAbsoluteHeight(pageSize.Width / imageAspect);
                    }
                     
                    float x = (pageSize.Width - marca.ScaledWidth) / 2;
                    float y = (pageSize.Height - marca.ScaledHeight) / 2;
                    marca.SetAbsolutePosition(x, y);
                     
                    PdfGState gstate = new PdfGState();
                    gstate.FillOpacity = opacidad;
                    cb.SaveState();
                    cb.SetGState(gstate);

                    cb.AddImage(marca);
                    cb.RestoreState();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al aplicar el fondo del documento: {ex.Message}", ex);
            }
        }

        private void AgregarTexto(Document doc, string texto, float tamano, bool negrita, int alineacion)
        {
            Font fuente = FontFactory.GetFont(FontFactory.HELVETICA, tamano, negrita ? Font.BOLD : Font.NORMAL);
            Paragraph parrafo = new Paragraph(texto, fuente);
            parrafo.Alignment = alineacion;
            doc.Add(parrafo);
        }
    }
}
