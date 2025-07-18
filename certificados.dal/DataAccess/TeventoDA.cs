using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using certificados.models.Helper;
using certificados.services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TeventoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarEvento(Tevento tevento)
        {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    DetachIfTracked(tevento.Tmodalidad, tevento.IdModalidad);
                    DetachIfTracked(tevento.TtipoEvento, tevento.IdTipoEvento);
                    DetachIfTracked(tevento.Tgrupo, tevento.IdGrupo);
                    DetachIfTracked(tevento.Tdecanato, tevento.IdDecanato);

                    tevento.Idevento = context.Tevento.Count() + 1;
                    tevento.Tmodalidad = context.Tmodalidad.Find(tevento.IdModalidad) ?? tevento.Tmodalidad;
                    tevento.TtipoEvento = context.TtipoEvento.Find(tevento.IdTipoEvento) ?? tevento.TtipoEvento;
                    tevento.Tgrupo = context.Tgrupo.Find(tevento.IdGrupo) ?? tevento.Tgrupo;
                    tevento.Tdecanato = context.Tdecanato.Find(tevento.IdDecanato) ?? tevento.Tdecanato;


                    context.Tevento.Add(tevento);
                    context.SaveChanges();

                    //var eventoAudit = AuditHelper.ConvertToAudit<Tevento, TeventoAuditoria>(tevento);
                    //eventoAudit.Idevento = 0;
                    //context.TeventoAuditoria.Add(eventoAudit);
                    //context.SaveChanges();

                    transaction.Commit();
                    response = Utils.OkResponse(tevento);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Error al insertar EVENTO " + ex.Message;
                    throw new Exception($"ERROR AL INSERTAR EVENTO: {ex.Message}");

                }

                return response;
            }

        }
        public ResponseApp ModificarEvento(Tevento tevento)
        {
            ResponseApp response = Utils.BadResponse(null);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Detach entidades relacionadas si ya están siendo rastreadas
                    DetachIfTracked(tevento.Tmodalidad, tevento.IdModalidad);
                    DetachIfTracked(tevento.TtipoEvento, tevento.IdTipoEvento);
                    DetachIfTracked(tevento.Tgrupo, tevento.IdGrupo);
                    DetachIfTracked(tevento.Tdecanato, tevento.IdDecanato);

                    // Buscar la entidad existente y sus relaciones
                    var existingEvento = context.Tevento.Find(tevento.Idevento);
                    if (existingEvento == null)
                    {
                        throw new Exception("Evento no encontrado.");
                    }

                    // Actualizar propiedades de la entidad existente
                    existingEvento.Tmodalidad = context.Tmodalidad.Find(tevento.IdModalidad) ?? tevento.Tmodalidad;
                    existingEvento.TtipoEvento = context.TtipoEvento.Find(tevento.IdTipoEvento) ?? tevento.TtipoEvento;
                    existingEvento.Tgrupo = context.Tgrupo.Find(tevento.IdGrupo) ?? tevento.Tgrupo;
                    existingEvento.Tdecanato = context.Tdecanato.Find(tevento.IdDecanato) ?? tevento.Tdecanato;

                    existingEvento.FechaInicio = tevento.FechaInicio;
                    existingEvento.FechaFin = tevento.FechaFin;
                    existingEvento.Horas = tevento.Horas;
                    existingEvento.Lugar = tevento.Lugar;
                    existingEvento.ConCertificado = tevento.ConCertificado;
                    existingEvento.Periodo = Utils.SafeString( tevento.Periodo);
                    existingEvento.Tematica = Utils.SafeString(tevento.Tematica);
                    existingEvento.Dominio = Utils.SafeString(tevento.Dominio);
                    existingEvento.Estado = Utils.SafeString(tevento.Estado);
                    existingEvento.Facilitador = Utils.SafeString(tevento.Facilitador);
                    existingEvento.UsuarioActualizacion = tevento.UsuarioActualizacion;
                    existingEvento.FModificacion = Utils.timeParsed(DateTime.Now);

                    // Marcar la entidad como modificada
                    context.Tevento.Update(existingEvento);

                    // Guardar cambios y confirmar transacción
                    context.SaveChanges();
                    transaction.Commit();

                    response = Utils.OkResponse(existingEvento);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL MODIFICAR EVENTO: {ex.Message}");
                    throw new Exception($"ERROR AL MODIFICAR EVEMTO: {ex.Message}");
                }
            }

            return response;
        }


        public ResponseApp EliminarEvento(int idEvento)
        {
            ResponseApp response = Utils.BadResponse(null);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Busca el evento por su ID
                    var eventoExistente = context.Tevento.FirstOrDefault(e => e.Idevento == idEvento);

                    if (eventoExistente != null)
                    {
                        // Cambia el estado del evento a "Inactivo"
                        eventoExistente.Estado = "INA"; 

                        // Guarda los cambios en la base de datos
                        context.SaveChanges();
                        transaction.Commit();

                        response = Utils.OkResponse(eventoExistente);
                    }
                    else
                    {
                        response = Utils.BadResponse("EVENTO NO EXISTE");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL CAMBIAR EL ESTADO DEL EVENTO: {ex.Message}");
                    throw new Exception($"ERROR AL CAMBIAR EL ESTADO DEL EVENTO: {ex.Message}");
                }
            }

            return response;
        }

        public ResponseApp ListarEventos()
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var listaEventos = context.Tevento.Include(gp => gp.TtipoEvento)
                    .Include(gp => gp.Tmodalidad)
                    .Include(gp => gp.Tdecanato).ToList();

                response = Utils.OkResponse(listaEventos);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR EVENTOS: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR EVEMTO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarEventosPorPeriodo(string periodo)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var listaEventos = context.Tevento.Include(gp => gp.TtipoEvento)
                    .Include(gp => gp.Tmodalidad)
                    .Include(gp => gp.Tdecanato).Where(e => e.Periodo == periodo).ToList();

                response = Utils.OkResponse(listaEventos);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR EVENTOS: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR EVEMTO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarEventosPorParametro<T>(string propiedad, T valor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Construir dinámicamente la consulta
                var parameter = Expression.Parameter(typeof(Tevento), "e");
                var property = Expression.Property(parameter, propiedad);
                var constant = Expression.Constant(valor, typeof(T));
                var equality = Expression.Equal(property, constant);

                var lambda = Expression.Lambda<Func<Tevento, bool>>(equality, parameter);

                // Ejecutar la consulta
                var listaEventos = context.Tevento.Include(gp => gp.TtipoEvento)
                    .Include(gp => gp.Tmodalidad)
                    .Include(gp => gp.Tdecanato).Where(lambda).ToList();
                if (listaEventos.Count > 0) { 
                 response = Utils.OkResponse(listaEventos);
                }
               
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR EVENTOS: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR EVEMTO: {ex.Message}");
            }
            return response;
        }



        public ResponseApp BuscarEvento(int idEvento)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var evento = context.Tevento.
                    Include(gp => gp.Tgrupo)
                    .Include(gp => gp.TtipoEvento)
                    .Include(gp => gp.Tmodalidad)
                    .Include(gp => gp.Tdecanato)
                    .FirstOrDefault(e => e.Idevento == idEvento);

                if (evento != null)
                {
                    response = Utils.OkResponse(evento);
                }
                else
                {
                    response = Utils.BadResponse("EVENTO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR EVENTO: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR EVEMTO: {ex.Message}");
            }
            return response;
        }

        private void DetachIfTracked<T>(T entity, int id) where T : class
        {
            if (entity != null)
            {
                var trackedEntity = context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == entity);
                if (trackedEntity != null)
                {
                    trackedEntity.State = EntityState.Detached;
                }
            }
        }
    }
}
