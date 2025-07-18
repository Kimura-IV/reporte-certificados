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
    public class TcertificadoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarCertificado(Tcertificado tcertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tcertificado.TformatoCertificado = context.TformatoCertificado.Local.FirstOrDefault(e => e.idFormato == tcertificado.TformatoCertificado.idFormato)
                                                        ?? context.TformatoCertificado.Find(tcertificado.IdFormato);


                    context.Tcertificado.Add(tcertificado);
                    context.SaveChanges();

                    var certifiAudit = AuditHelper.ConvertToAudit<Tcertificado, TcertificadoAuditoria>(tcertificado);
                    certifiAudit.IdCertificado = 0;
                    context.TcertificadoAuditoria.Add(certifiAudit);
                    context.SaveChanges();

                    transaction.Commit();
                    response = Utils.OkResponse(tcertificado);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse("Error al insertar CERTIFICADO");

                    throw new Exception($"ERROR AL PROCESAR LA INSERACION DE UN CERTIFICADO {ex.Message}");
                }
                return response;
            }


        }

        public ResponseApp ModificarCertificado(Tcertificado certificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Buscar el certificado existente
                    var certificadoExistente = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == certificado.IdCertificado);

                    if (certificadoExistente != null)
                    {


                        certificadoExistente.TformatoCertificado = context.TformatoCertificado.Find(certificado.TformatoCertificado.idFormato) ?? certificado.TformatoCertificado;

                        // Actualizar los campos del certificado
                        certificadoExistente.Titulo = certificado.Titulo;
                        certificadoExistente.Imagen = certificado.Imagen;
                        certificadoExistente.IdFormato = certificado.IdFormato;
                        certificadoExistente.Tipo = certificado.Tipo;
                        certificadoExistente.Estado = certificado.Estado;
                        certificadoExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                        certificadoExistente.UsuarioActualizacion = certificado.UsuarioActualizacion;

                        // Guardar los cambios
                        context.SaveChanges();
                        transaction.Commit();
                        response = Utils.OkResponse(certificadoExistente);
                    }
                    else
                    {
                        // Si el certificado no existe
                        response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL MODIFICAR CERTIFICADO: {ex.Message}");
                    throw new Exception($"ERROR AL MODIFICAR CERTIFICADO: {ex.Message}");
                }
                return response;
            }

        }

        public ResponseApp EliminarCertificado(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el certificado existente
                var certificadoExistente = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == idCertificado);

                if (certificadoExistente != null)
                {
                    // Eliminar el certificado
                    context.Tcertificado.Remove(certificadoExistente);
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(certificadoExistente);
                }
                else
                {
                    // Si el certificado no existe
                    response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR CERTIFICADO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarCertificados()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado.ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarCertificadosPorEvento(int idEvento)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado
                    .Where(c => c.IdCertificado == idEvento)
                    .ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }
        public ResponseApp CertificadosById(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado
                    .Include(gp => gp.TformatoCertificado)  
                    
                    .Where(c => c.IdCertificado == idCertificado)
                    .FirstOrDefault();
                if (listaCertificados == null)
                    return Utils.BadResponse("NO EXISTE EL CERTIDICADO");

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarCertificado(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el certificado por su IdCertificado
                var certificado = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == idCertificado);

                if (certificado != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(certificado);
                }
                else
                {
                    // Si el certificado no existe
                    response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR CERTIFICADO: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR CERTIFICADO: {ex.Message}");
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
