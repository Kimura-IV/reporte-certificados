using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class CertificadoMapper
    {
        public static Tcertificado toEntity(object data) {

            try
            {
                var eventoAux = JsonConvert.SerializeObject(data);
                var evento = JsonConvert.DeserializeObject<Tcertificado>(eventoAux);
                if (evento == null)
                    throw new Exception("El objeto evento es NULO");
                return evento;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al convertir la entidad {ex.Message}");
            }
        }
        public static Tcertificado toEntity(CertificadoDTO dto, Tevento tevento, TformatoCertificado formato) {

            return new Tcertificado { 

                Titulo = dto.Titulo,
                Imagen = dto.Imagen,
                IdCertificado = dto.IdCertificado,
                Tipo = dto.Tipo,   
                Estado = dto.Estado,
                FCreacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso = dto.UsuarioIngreso,
                TformatoCertificado = formato
            };
        }
        public static Tcertificado toEntityUpdate(CertificadoDTO dto, Tevento tevento, TformatoCertificado formato)
        {

            return new Tcertificado
            {

                Titulo = dto.Titulo,
                Imagen = dto.Imagen,
                IdCertificado = dto.IdCertificado,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioActualizacion = dto.UsuarioActualizacion,
                TformatoCertificado = formato
            };
        }
    }
}
