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
    public class TactAsistenciaDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;
        public ResponseApp InsertarActaAsistencia(TactaAsistencia tactaAsistencia)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    DetachIfTracked(tactaAsistencia.Tevento, tactaAsistencia.IdEvento);
                    tactaAsistencia.Tevento = context.Tevento.Local.FirstOrDefault(e => e.Idevento == tactaAsistencia.Tevento.Idevento)
                        ?? context.Tevento.Find(tactaAsistencia.IdEvento);


                    context.TactaAsistencia.Add(tactaAsistencia);
                    context.SaveChanges();


                    var actaAudit = AuditHelper.ConvertToAudit<TactaAsistencia, TactaAsistenciaAuditoria>(tactaAsistencia);
                    actaAudit.IdAsistencia = 0;
                    context.TactaAsistenciaAuditoria.Add(actaAudit);
                    context.SaveChanges();

                    transaccion.Commit();
                    response = Utils.OkResponse(tactaAsistencia);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR ACTA DE ASISTENCIA: {ex.Message}");
                    throw new Exception($"ERROR AL INSERTAR ACTA: {ex.Message}");
                }
            }
            return response;
        }


        public ResponseApp ModificarActaAsistencia(TactaAsistencia tactaAsistencia)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var actaExistente = context.TactaAsistencia.FirstOrDefault(a => a.IdAsistencia == tactaAsistencia.IdAsistencia);

                if (actaExistente != null)
                { 
                    actaExistente.IdEvento = tactaAsistencia.IdEvento;
                    actaExistente.ActaDocumento = tactaAsistencia.ActaDocumento;
                    actaExistente.FModificacion = DateTime.Now;
                    actaExistente.UsuarioActualizacion = tactaAsistencia.UsuarioActualizacion;
 
                    context.SaveChanges(); 
                    response = Utils.OkResponse(actaExistente);
                }
                else
                {
                    // Si la acta de asistencia no existe
                    response = Utils.BadResponse("ACTA DE ASISTENCIA NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR ACTA DE ASISTENCIA: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR ACTA: {ex.Message}");
            }
            return response;
        }


        public ResponseApp EliminarActaAsistencia(int idAsistencia)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar la acta de asistencia existente
                var actaExistente = context.TactaAsistencia.FirstOrDefault(a => a.IdAsistencia == idAsistencia);

                if (actaExistente != null)
                {
                    // Eliminar la acta de asistencia
                    context.TactaAsistencia.Remove(actaExistente);
                    context.SaveChanges(); 
                    response = Utils.OkResponse(actaExistente);
                }
                else
                { 
                    response = Utils.BadResponse("ACTA DE ASISTENCIA NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR ACTA DE ASISTENCIA: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR ACTA: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarActasAsistencia()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Obtener todas las actas de asistencia de la base de datos
                var listaActas = context.TactaAsistencia.Include(e => e.Tevento).ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaActas);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR ACTAS DE ASISTENCIA: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR ACTA: {ex.Message}");
            }
            return response;
        }


        public ResponseApp BuscarActaAsistencia(int idAsistencia)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var acta = context.TactaAsistencia.Include(e => e.Tevento).FirstOrDefault(a => a.IdAsistencia == idAsistencia);

                if (acta != null)
                { 
                    response = Utils.OkResponse(acta);
                }
                else
                { 
                    response = Utils.BadResponse("ACTA DE ASISTENCIA NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR ACTA DE ASISTENCIA: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR ACTA: {ex.Message}");
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
