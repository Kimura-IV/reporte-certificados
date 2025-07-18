using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using certificados.web.Models.DTO;
using Newtonsoft.Json;

namespace certificados.web.Controllers.Mappers
{
    public class EventoMapper
    {
        public static Tevento convertEntity(object data) {

            try
            {
                var eventoAux =  JsonConvert.SerializeObject(data);
                var evento = JsonConvert.DeserializeObject<Tevento>(eventoAux);
                if (evento == null)
                    throw new Exception("El objeto evento es NULO");
                return evento;
            }
            catch (Exception ex) {

                throw new Exception($"Error al convertir la entidad {ex.Message}");
                }
        }
        public static Tevento toEntity(EventoDTO dto, Tmodalidad modalidad, TtipoEvento ttipoEvento, Tgrupo grupo, Tdecanato tdecanato) {

            return new Tevento
            {
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Horas = dto.Horas,
                ConCertificado = dto.ConCertificado,
                Periodo = dto.Periodo,
                Tematica =Utils.SafeString(dto.Tematica),
                Dominio = Utils.SafeString(dto.Dominio),
                Lugar = Utils.SafeString(dto.Lugar),
                Facilitador = Utils.SafeString(dto.Facilitador),
                Estado = Utils.SafeString(dto.Estado),
                IdGrupo = dto.IdGrupo,
                IdModalidad = dto.IdModalidad,
                IdTipoEvento = dto.IdTipoEvento,
                IdDecanato = dto.IdDecanato,
                FCreacion = Utils.timeParsed(DateTime.Now),
                UsuarioIngreso = dto.UsuarioIngreso,
                Tmodalidad = modalidad,
                TtipoEvento = ttipoEvento,
                Tgrupo = grupo,
                Tdecanato = tdecanato
            };
        }
        public static Tevento toEntityUpdate(EventoDTO dto, Tmodalidad modalidad, TtipoEvento ttipoEvento, Tgrupo grupo, Tdecanato tdecanato)
        {

            return new Tevento
            {
                Idevento = dto.Idevento,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Horas = dto.Horas,
                Lugar =  dto.Lugar,
                ConCertificado = dto.ConCertificado,
                Periodo = dto.Periodo,
                Tematica = Utils.SafeString(dto.Tematica),
                Dominio = Utils.SafeString(dto.Dominio),
                Facilitador = Utils.SafeString(dto.Facilitador),
                Estado = Utils.SafeString(dto.Estado),
                IdGrupo = dto.IdGrupo,
                IdModalidad = dto.IdModalidad,
                IdTipoEvento = dto.IdTipoEvento,
                IdDecanato = dto.IdDecanato,
                FModificacion = Utils.timeParsed(DateTime.Now),
                UsuarioActualizacion = dto.UsuarioActualizacion,
                Tmodalidad = modalidad,
                TtipoEvento = ttipoEvento,
                Tgrupo = grupo,
                Tdecanato = tdecanato
            };
        }
    }
}
