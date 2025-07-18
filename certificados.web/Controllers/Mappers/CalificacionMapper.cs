using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;

namespace certificados.web.Controllers.Mappers
{
    public class CalificacionMapper
    {
        public static TactaCalificacion toEntity(ActaCalificacionDTO dto, Tevento tevento) {

            return new TactaCalificacion {
                IdEvento = dto.IdEvento,
                ActaDocumento = dto.ActaDocumento,
                FCreacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso = dto.UsuarioIngreso,
                Tevento = tevento
            
            };
        
         }
    }
}
