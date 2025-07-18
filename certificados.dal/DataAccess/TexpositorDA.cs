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
    public class TexpositorDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarExpositor(Texpositor texpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction()) {
                try
                {
                    var trackedEntity = context.ChangeTracker.Entries<Tpersona>()
                        .FirstOrDefault(e => e.Entity.Cedula == texpositor.Tpersona.Cedula);

                    if (trackedEntity != null)
                    {
                        trackedEntity.State = EntityState.Detached;
                    }

                    if (context.Tpersona.Any(p => p.Cedula == texpositor.Tpersona.Cedula))
                    {
                        context.Attach(texpositor.Tpersona);
                    }
                    else
                    {
                        response = Utils.BadResponse($"NO EXISTE LA PERSONA EN LOS REGISTROS");
                        return response;
                    }
                    var existeExpositor = context.Texpositor.FirstOrDefault(p => p.Tpersona.Cedula == texpositor.Cedula);

                    if (existeExpositor != null)
                    {
                        return Utils.BadResponse($"ESTE USUARIO {texpositor.Cedula} ya esta registrado como Expositor ");
                    }
                    context.Texpositor.Add(texpositor);
                    context.SaveChanges();
                    var expositorAudit = AuditHelper.ConvertToAudit<Texpositor, TexpositorAuditoria>(texpositor);
                    expositorAudit.IdExpositor = 0;
                    context.TexpositorAuditoria.Add(expositorAudit);
                    context.SaveChanges();

                    transaccion.Commit();
                    response = Utils.OkResponse(texpositor);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR EXPOSITOR: {ex.Message}");
                    throw new Exception($"ERROR AL INSERTAR expositor: {ex.Message}", ex);
                }
            }
            

            return response;
        }



        public ResponseApp ModificarExpositor(Texpositor expositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var expositorExistente = context.Texpositor.FirstOrDefault(e => e.IdExpositor == expositor.IdExpositor);

                if (expositorExistente != null)
                { 
                    expositorExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    expositorExistente.UsuarioActualizacion = expositor.UsuarioActualizacion;

                    context.SaveChanges();
                    response = Utils.OkResponse(expositorExistente);
                }
                else
                {
                    response = Utils.BadResponse($"EXPOSITOR {expositor.IdExpositor} NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR expositor: {ex.Message}");
            }
            return response;
        }

        public ResponseApp EliminarExpositor(int idExpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var expositorExistente = context.Texpositor.FirstOrDefault(e => e.IdExpositor == idExpositor);

                if (expositorExistente != null)
                {
                    context.Texpositor.Remove(expositorExistente);
                    context.SaveChanges();

                    response = Utils.OkResponse(expositorExistente);
                }
                else
                {
                    response = Utils.BadResponse("EXPOSITOR NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR expositor: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarExpositores()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var listaExpositores = context.Texpositor.Include(e =>e.Tpersona).ToList();

                response = Utils.OkResponse(listaExpositores);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR EXPOSITORES: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR expositor: {ex.Message}");
            }
            return response;
        }
        public ResponseApp BuscarExpositor(int idExpositor)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var expositor = context.Texpositor.Include(e => e.Tpersona).FirstOrDefault(e => e.IdExpositor == idExpositor);

                if (expositor != null)
                {
                    response = Utils.OkResponse(expositor);
                }
                else
                {
                    response = Utils.BadResponse("EXPOSITOR NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR EXPOSITOR: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR expositor: {ex.Message}");
            }
            return response;
        }

    }
}
