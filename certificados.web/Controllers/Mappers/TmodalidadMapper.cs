using certificados.models.Entitys.dbo;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class TmodalidadMapper
    {
        public static Tmodalidad toEntity(object data) {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var modalidad = JsonConvert.DeserializeObject<Tmodalidad>(json);
                if(modalidad == null )
                    throw new Exception("La conversión resultó en un objeto nulo");
                return modalidad;
            }
            catch (Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TMODALIDAD: {ex.Message}");
            }
        }
    }
}
