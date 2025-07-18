using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class EstadoDocenteMapper
    {
        public static TestadoDocente toEntity(object data) {


            try
            {
                var json = JsonConvert.SerializeObject(data);
                var estadoDocente = JsonConvert.DeserializeObject<TestadoDocente>(json);
                if (estadoDocente == null)
                    throw new Exception("La conversión resultó en un objeto nulo");
                return estadoDocente;
            }
            catch (Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TESTADOCENTE: {ex.Message}");
            }
        }
        public static TestadoDocente toEntityCreate(TestadoDocente estadoDocente) {

            return new TestadoDocente { 
                Nombre = Utils.SafeString(estadoDocente.Nombre),
            };
        }
        public static TestadoDocente toEntityUpdate(TestadoDocente estadoDocente)
        {

            return new TestadoDocente
            {
                IdEstado = estadoDocente.IdEstado,
                Nombre = Utils.SafeString(estadoDocente.Nombre),
            };
        }
    }
}
