using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using certificados.models.Helper;
using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TtipoEventoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp insertarTipoEvento(TtipoEvento evento)
        {
            ResponseApp response = Utils.BadResponse(null);

            using (var transaccion = context.Database.BeginTransaction())
            {
                 
                try
                {
                    TtipoEvento nuevoEvento = new TtipoEvento
                    {
                        Idtipoevento = context.TtipoEvento.Count() + 1,
                        Nombre = Utils.SafeString(evento.Nombre),
                        Descripcion = Utils.SafeString(evento.Descripcion),
                        FCreacion = Utils.timeParsed(DateTime.Now),
                        UsuarioIngreso = Utils.SafeString(evento.UsuarioIngreso)
                    };

                    context.TtipoEvento.Add(nuevoEvento);
                    context.SaveChanges();

                    transaccion.Commit();
                    response = Utils.OkResponse(nuevoEvento);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response.Message = $"PROBLEMAS AL INSERTAR EVENTO: {ex.Message}";
                    throw new Exception($"ERROR AL INSERTAR EVENTO: {ex.Message}");
                }
            }
            return response;
        }

        public ResponseApp ModificarTipoEvento(TtipoEvento evento)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var eventoExistente = context.TtipoEvento.FirstOrDefault(e => e.Idtipoevento == evento.Idtipoevento);

                if (eventoExistente != null)
                {
                    eventoExistente.Nombre = Utils.SafeString(evento.Nombre);
                    eventoExistente.Descripcion = Utils.SafeString(evento.Descripcion);
                    eventoExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    eventoExistente.UsuarioActualizacion = Utils.SafeString(evento.UsuarioActualizacion);

                    context.SaveChanges();

                    response = Utils.OkResponse(eventoExistente);
                }
                else
                {
                    response.Message = "TIPO DE EVENTO NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = "ERROR AL MODIFICAR TIPO DE EVENTO: " + ex.Message;
                throw new Exception($"ERROR AL MODIFICAR TIPO DE EVENTO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp EliminarTipoEvento(int idTipoEvento)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var eventoExistente = context.TtipoEvento.FirstOrDefault(e => e.Idtipoevento == idTipoEvento);

                if (eventoExistente != null)
                {
                    context.TtipoEvento.Remove(eventoExistente);
                    context.SaveChanges();

                    response = Utils.OkResponse(eventoExistente);
                }
                else
                {
                    response = Utils.BadResponse("TIPO DE EVENTO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR TIPO DE EVENTO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR TIPO DE EVENTO: {ex.Message}");
            }

            return response;
        }
        public ResponseApp ConsultarTipoEventoId(int idTipoEvento)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var eventoExistente = context.TtipoEvento.FirstOrDefault(e => e.Idtipoevento == idTipoEvento);

                if (eventoExistente != null)
                {
                    response = Utils.OkResponse(eventoExistente);
                }
                else
                {
                    response.Message = "TIPO DE EVENTO NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR TIPO DE EVENTO: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR TIPO DE EVENTO: {ex.Message}");
            }

            return response;
        }
        public ResponseApp ListarTipoEvento()
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var eventos = context.TtipoEvento.ToList();

                if (eventos.Any())
                {
                    response = Utils.OkResponse(eventos);
                }
                else
                {
                    response.Message = "NO EXISTEN TIPOS DE EVENTOS REGISTRADOS";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR TIPOS DE EVENTOS: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR TIPOS DE EVENTOS: {ex.Message}");
            }

            return response;
        }


    }
}
