using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class FormatoCertificadoMapper
    {
        public static TformatoCertificado convertEntity(object data)
        {

            try
            {
                var formatoAux = JsonConvert.SerializeObject(data);
                var formato = JsonConvert.DeserializeObject<TformatoCertificado>(formatoAux);
                if (formato == null)
                    throw new Exception("LA ENTIDAD RESULTO NULA");
                return formato;
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR AL CONVERTIR A ENTIDAD: ${ex.Message}");
            }
        }
        public static TformatoCertificado toEntityCreate(Dictionary<string, string> data)
        {
            return new TformatoCertificado
            {
                NombrePlantilla = data.ContainsKey("NombrePlantilla") ? data["NombrePlantilla"].ToString() : null,
                LineaGrafica = data.ContainsKey("LineaGrafica") ? Convert.FromBase64String(data["LineaGrafica"]) : null,
                LogoUG = data.ContainsKey("LogoUG") ? Convert.FromBase64String(data["LogoUG"]) : null,
                Origen = data.ContainsKey("Origen") ? data["Origen"].ToString() : null,
                Tipo = data.ContainsKey("Tipo") ? data["Tipo"].ToString() : null,
                Leyenda = data.ContainsKey("Leyenda") ? data["Leyenda"].ToString() : null,
                Qr = data.ContainsKey("Qr") ? Convert.FromBase64String(data["Qr"]) : null,
                CargoFirmanteUno = data.ContainsKey("CargoFirmanteUno") ? data["CargoFirmanteUno"].ToString() : null,
                NombreFirmanteUno = data.ContainsKey("NombreFirmanteUno") ? data["NombreFirmanteUno"].ToString() : null,
                CargoFirmanteDos = data.ContainsKey("CargoFirmanteDos") ? data["CargoFirmanteDos"].ToString() : null,
                NombreFirmanteDos = data.ContainsKey("NombreFirmanteDos") ? data["NombreFirmanteDos"].ToString() : null,
                CargoFirmanteTres = data.ContainsKey("CargoFirmanteTres") ? data["CargoFirmanteTres"].ToString() : null,
                NombreFirmanteTres = data.ContainsKey("NombreFirmanteTres") ? data["NombreFirmanteTres"].ToString() : null,
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso = data.ContainsKey("UsuarioIngreso") ? data["UsuarioIngreso"] : "admin"
            };
        }

        public static TformatoCertificado toEntityUpdate(TformatoCertificado tformatoCertificado)
        {

            return new TformatoCertificado
            {
                idFormato = tformatoCertificado.idFormato,
                NombrePlantilla = tformatoCertificado.NombrePlantilla,
                LineaGrafica = tformatoCertificado.LineaGrafica,
                LogoUG = tformatoCertificado.LogoUG,
                Origen = tformatoCertificado.Origen,
                Tipo = tformatoCertificado.Tipo,
                Leyenda = tformatoCertificado.Leyenda,
                Qr = tformatoCertificado.Qr,
                CargoFirmanteUno = tformatoCertificado.CargoFirmanteUno,
                NombreFirmanteUno = tformatoCertificado.NombreFirmanteUno,
                CargoFirmanteDos = tformatoCertificado.CargoFirmanteDos,
                NombreFirmanteDos = tformatoCertificado.NombreFirmanteDos,
                CargoFirmanteTres = tformatoCertificado.CargoFirmanteTres,
                NombreFirmanteTres = tformatoCertificado.NombreFirmanteTres,
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioActualizacion = tformatoCertificado.UsuarioActualizacion
            };
        }
    }
}
