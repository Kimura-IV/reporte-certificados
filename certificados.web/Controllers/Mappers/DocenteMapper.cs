using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class DocenteMapper
    {

        public static Tdocente toEntity(object data) {

            try { 
                var json = JsonConvert.SerializeObject(data);
                var docente = JsonConvert.DeserializeObject<Tdocente>(json);
                if (docente == null)
                    throw new Exception("ERROR AL CREAR OBJETO DE TDOCENTE");
                return docente;
            }
            catch(Exception ex) {
                throw new Exception($"Error al convertir el objeto a entidad TESTADOCENTE: {ex.Message}");
            }
        }
        public static Tdocente toEntityCreate(DocenteDTO tdocente, TestadoDocente testadoDocente, Tpersona tpersona) {
            return new Tdocente {
            
                CodigoDocente = tdocente.CodigoDocente,
                Cedula = tdocente.Cedula,
                Titulo = Utils.SafeString(tdocente.Titulo),
                Facultad = Utils.SafeString(tdocente.Facultad),
                Carrera = tdocente.Carrera,
                FCreacion = Utils.timeParsed(DateTime.Now),
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso =  tdocente.UsuarioIngreso,
                IdEstado = tdocente.IdEstado,
                Tpersona = tpersona,
                TestadoDocente = testadoDocente,
            };
        }
        public static Tdocente toEntityUpdate(Tdocente tdocente, TestadoDocente testadoDocente, Tpersona tpersona)
        {
            return new Tdocente
            { 
                CodigoDocente = tdocente.CodigoDocente,
                Cedula = tdocente.Cedula,
                Titulo = Utils.SafeString(tdocente.Titulo),
                Facultad = Utils.SafeString(tdocente.Facultad),
                Carrera = tdocente.Carrera, 
                FModificacion = Utils.timeParsed(DateTime.Now),
                UserModificacion = tdocente.UserModificacion,
                IdEstado = tdocente.IdEstado,
                Tpersona = tpersona,
                TestadoDocente = testadoDocente,
            };
        }
    }
}
