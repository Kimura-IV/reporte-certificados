using certificados.models.Entitys.dbo;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class PersonaMapper
    {
        public static Tpersona toEntity(object data) {

            try
            {
                var json = JsonConvert.SerializeObject(data);
                var persona = JsonConvert.DeserializeObject<Tpersona>(json);
                if (persona == null)
                    throw new Exception("La conversión resultó en un objeto nulo");

                return persona;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir el objeto a entidad Tpersona: {ex.Message}");
            }

        }
    }
}
