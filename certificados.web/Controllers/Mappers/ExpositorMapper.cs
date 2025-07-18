using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;

namespace certificados.web.Controllers.Mappers
{
    public class ExpositorMapper
    {
        //Para la creacion
        public static Texpositor toDtoToEntity(ExpositorDTO dto, Tpersona persona)
        {
            try
            {
                return new Texpositor
                {
                    Cedula = dto.Cedula, 
                    UsuarioActualizacion = Utils.SafeString(dto.UsuarioActualizacion),
                    FCreacion = Utils.timeParsed(DateTime.Now),
                    Tpersona = persona
                };

            }
            catch (Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TEXPOSITOR: {ex.Message}");
            }

        }
        //Para la actualizacion
        public static Texpositor toDtoToEntity(Tpersona persona, ExpositorDTO dto)
        {
            try
            {
                return new Texpositor
                {
                    IdExpositor = (int) dto.IdExpositor,
                    Cedula = dto.Cedula,
                    UsusarioIngreso = dto.UsuarioIngreso,
                    UsuarioActualizacion = Utils.SafeString(dto.UsuarioActualizacion),
                    FCreacion = Utils.timeParsed(DateTime.Now),
                    Tpersona = persona,
                    FModificacion = Utils.timeParsed(DateTime.Now)
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir el objeto a entidad TEXPOSITOR: {ex.Message}");
            }

        }

    }
}
