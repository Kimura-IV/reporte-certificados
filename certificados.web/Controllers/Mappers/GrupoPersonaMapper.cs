using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace certificados.web.Controllers.Mappers
{
    public class GrupoPersonaMapper
    {
        public static Tgrupo toEntityGrupo(object data) {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var persona = JsonConvert.DeserializeObject<Tgrupo>(json);
                if (persona == null)
                    throw new Exception("La conversión resultó en un objeto nulo");

                return persona;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir el objeto a entidad TGRUPOPERSONA: {ex.Message}");
            }

        }
        public static TgrupoPersona toEntity(object data) {

            try
            {
                var json = JsonConvert.SerializeObject(data);
                var persona = JsonConvert.DeserializeObject<TgrupoPersona>(json);
                if (persona == null)
                    throw new Exception("La conversión resultó en un objeto nulo");

                return persona;
            }
            catch (Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TGRUPOPERSONA: {ex.Message}");
            }
        }
        public static TgrupoPersona toEntity(GrupoPersonaDTO dto, Tpersona persona, Tgrupo grupo, string cedula)
        {

            return new TgrupoPersona
            {
                IdGrupo = dto.IdGrupo,
                Cedula = cedula,
                FCreacion = Utils.timeParsed(DateTime.Now),
                UsusarioIngreso = dto.UsuarioIngreso,
                Tpersona = persona,
                Tgrupo = grupo,
                Estado = "PEN"
            };
        }

        public static List<Tpersona> listadoPersonas(object data)
        {
 
            if (data is List<TgrupoPersona> grupos)
            { 
                return grupos
                    .Select(grupoPersona => grupoPersona.Tpersona)  
                    .Where(persona => persona != null)  
                    .ToList();  
            }
             
            return new List<Tpersona>();
        }
 
    }
}
