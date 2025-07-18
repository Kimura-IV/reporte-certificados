using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace certificados.web.Controllers.Mappers
{
    public class DecanatoMapper
    {
        public static Tdecanato toEntityCreate(Tdecanato entity) {
             
            return new Tdecanato{
                Nombre = Utils.SafeString(entity.Nombre),
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso = Utils.SafeString(entity.UsuarioIngreso),
            };
        }

        public static Tdecanato toEntityUpdate(Tdecanato entity)
        {

            return new Tdecanato
            {
                IdDecanato = entity.IdDecanato,
                Nombre = Utils.SafeString(entity.Nombre),
                FModificacion = Utils.timeParsed(DateTime.Now), 
                UsuarioActualizacion = Utils.SafeString(entity.UsuarioActualizacion)
            };
        }
        public static Tdecanato toEntityObject(object data) {

            try
            {
                var json = JsonConvert.SerializeObject(data);
                var decanato = JsonConvert.DeserializeObject<Tdecanato>(json);
                if (decanato == null)
                    throw new Exception("La conversión resultó en un objeto nulo");
                return decanato;

            }
            catch (Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TESTADOCENTE: {ex.Message}");
            }
        
        }
    }
}
