using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class GeneralMapper
    {
        //Permite el mapeo de cualquier entidad
        public static T ConvertToEntity<T>(object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("La conversión resultó en un objeto nulo");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir el objeto a entidad {typeof(T).Name}: {ex.Message}");
            }
        }

    }
}
