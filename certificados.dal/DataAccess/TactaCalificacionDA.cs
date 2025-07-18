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
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TactaCalificacionDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;
        public ResponseApp InsertarActaCalificacion(TactaCalificacion tactaCalificacion)
        {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    DetachIfTracked(tactaCalificacion.Tevento, tactaCalificacion.IdEvento);
                    tactaCalificacion.Tevento = context.Tevento.Local.FirstOrDefault(e => e.Idevento == tactaCalificacion.Tevento.Idevento)
                        ?? context.Tevento.Find(tactaCalificacion.IdEvento);

                    context.TactaCalificacion.Add(tactaCalificacion);
                    context.SaveChanges();

                    var actaAudit = AuditHelper.ConvertToAudit<TactaCalificacion, TactaCalificacionAuditoria>(tactaCalificacion);
                    actaAudit.IdCalificacion = 0;
                    context.TactaCalificacionAuditoria.Add(actaAudit);
                    context.SaveChanges();

                    transaction.Commit();
                    response = Utils.OkResponse(tactaCalificacion);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR ACTA DE CALIFICACIÓN: {ex.Message}");
                    throw new Exception($"ERROR AL INSERTAR ACTA: {ex.Message}");
                }
                return response;
            }


        }

        public ResponseApp ModificarActaCalificacion(TactaCalificacion tactaCalificacion)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var actaExistente = context.TactaCalificacion.FirstOrDefault(a => a.IdCalificacion == tactaCalificacion.IdCalificacion);

                if (actaExistente != null)
                {

                    actaExistente.IdEvento = tactaCalificacion.IdEvento;
                    actaExistente.ActaDocumento = tactaCalificacion.ActaDocumento;
                    actaExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    actaExistente.UsuarioActualizacion = tactaCalificacion.UsuarioActualizacion;
                    context.SaveChanges();

                    response = Utils.OkResponse(actaExistente);
                }
                else
                {

                    response = Utils.BadResponse("ACTA DE CALIFICACIÓN NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR ACTA DE CALIFICACIÓN: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR ACTA: {ex.Message}");
            }
            return response;
        }


        public ResponseApp EliminarActaCalificacion(int idCalificacion)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar la acta de calificación existente
                var actaExistente = context.TactaCalificacion.FirstOrDefault(a => a.IdCalificacion == idCalificacion);

                if (actaExistente != null)
                {
                    // Eliminar la acta de calificación
                    context.TactaCalificacion.Remove(actaExistente);
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(actaExistente);
                }
                else
                {
                    // Si la acta de calificación no existe
                    response = Utils.BadResponse("ACTA DE CALIFICACIÓN NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR ACTA DE CALIFICACIÓN: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR ACTA: {ex.Message}");
            }
            return response;
        }


        public ResponseApp ListarActasCalificacion()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Obtener todas las actas de calificación de la base de datos
                var listaActas = context.TactaCalificacion.Include(e => e.Tevento).ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaActas);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR ACTAS DE CALIFICACIÓN: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR ACTA: {ex.Message}");
            }
            return response;
        }


        public ResponseApp BuscarActaCalificacion(int idCalificacion)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var acta = context.TactaCalificacion.Include(e =>e.Tevento).FirstOrDefault(a => a.IdCalificacion == idCalificacion);

                if (acta != null)
                {
                    response = Utils.OkResponse(acta);
                }
                else
                {
                    response = Utils.BadResponse("ACTA DE CALIFICACIÓN NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR ACTA DE CALIFICACIÓN: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR ID ACTA : {ex.Message}");
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
