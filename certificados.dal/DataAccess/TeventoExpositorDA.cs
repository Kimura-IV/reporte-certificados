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
    public class TeventoExpositorDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarEventoExpositor(TeventoExpositor teventoExpositor)
        {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    context.TeventoExpositor.Add(teventoExpositor);
                    context.SaveChanges();


                    var teventoEx = AuditHelper.ConvertToAudit<TeventoExpositor, TeventoExpositorAuditoria>(teventoExpositor);
                    teventoEx.IdEventoExpositor = 0;
                    context.TeventoExpositorAuditoria.Add(teventoEx);
                    context.SaveChanges();

                    transaccion.Commit();
                    // Retornar respuesta exitosa con el evento expositor creado
                    response = Utils.OkResponse(teventoExpositor);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR EVENTO EXPOSITOR: {ex.Message}");
                    throw new Exception($"ERROR AL insertar evento expositor: {ex.Message}");
                }
            }

            return response;
        }
        public ResponseApp ModificarEventoExpositor(TeventoExpositor eventoExpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var eventoExpositorExistente = context.TeventoExpositor.FirstOrDefault(ee => ee.IdEventoExpositor == eventoExpositor.IdEventoExpositor);

                if (eventoExpositorExistente != null)
                {
                    eventoExpositorExistente.IdEvento = eventoExpositor.IdEvento;
                    eventoExpositorExistente.IdExpositor = eventoExpositor.IdExpositor;
                    eventoExpositorExistente.FModificacion = DateTime.Now;
                    eventoExpositorExistente.UsuarioActualizacion = eventoExpositor.UsuarioActualizacion;
                    context.SaveChanges();

                    response = Utils.OkResponse(eventoExpositorExistente);
                }
                else
                { 
                    response = Utils.BadResponse("EVENTO EXPOSITOR NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR EVENTO EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR evento expositor: {ex.Message}");
            }
            return response;
        }

        public ResponseApp EliminarEventoExpositor(int idEventoExpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el evento expositor existente
                var eventoExpositorExistente = context.TeventoExpositor.FirstOrDefault(ee => ee.IdEventoExpositor == idEventoExpositor);

                if (eventoExpositorExistente != null)
                { 
                    context.TeventoExpositor.Remove(eventoExpositorExistente);
                    context.SaveChanges();
                     
                    response = Utils.OkResponse(eventoExpositorExistente);
                }
                else
                { 
                    response = Utils.BadResponse("EVENTO EXPOSITOR NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR EVENTO EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR evento expositor: {ex.Message}");
            }
            return response;
        }
        public ResponseApp ListarEventosExpositores()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var listaEventosExpositores = context.TeventoExpositor.ToList();

                response = Utils.OkResponse(listaEventosExpositores);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR EVENTOS EXPOSITORES: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR evento expositor: {ex.Message}");
            }
            return response;
        }
        public ResponseApp BuscarEventoExpositor(int idEventoExpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el evento expositor por su IdEventoExpositor
                var eventoExpositor = context.TeventoExpositor.FirstOrDefault(ee => ee.IdEventoExpositor == idEventoExpositor);

                if (eventoExpositor != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(eventoExpositor);
                }
                else
                {
                    // Si el evento expositor no existe
                    response = Utils.BadResponse("EVENTO EXPOSITOR NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR EVENTO EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR evento expositor: {ex.Message}");
            }
            return response;
        }
    }
}
