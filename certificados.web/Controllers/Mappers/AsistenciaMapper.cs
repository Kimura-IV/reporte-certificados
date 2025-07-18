using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;

namespace certificados.web.Controllers.Mappers
{
    public class AsistenciaMapper
    {
        public static TactaAsistencia toEntity(ActaAsistenciaDTO dto, Tevento evento) {

            return new TactaAsistencia {
                IdEvento = dto.IdEvento,
                ActaDocumento = dto.ActaDocumento,
                UsuarioIngreso = dto.UsuarioIngreso,
                FCreacion = Utils.timeParsed(DateTime.Now), 
                Tevento = evento
            };
        }
        public static TactaAsistencia toEntityUpdate(ActaAsistenciaDTO dto, Tevento evento)
        {

            return new TactaAsistencia
            {
                IdEvento = dto.IdEvento,
                ActaDocumento = dto.ActaDocumento,
                UsuarioIngreso = dto.UsuarioIngreso, 
                FModificacion = Utils.timeParsed(DateTime.Now),
                Tevento = evento
            };
        }
    }
}
