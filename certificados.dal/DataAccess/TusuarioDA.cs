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
    public class TusuarioDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarUsuario(Tusuario tusuario)
        {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    context.Tusuario.Add(tusuario);

                    var userAudit = AuditHelper.ConvertToAudit<Tusuario, TusuarioAuditoria>(tusuario);
                    context.TusuarioAuditoria.Add(userAudit);
                    context.SaveChanges();
                    transaccion.Commit();
                    response = Utils.OkResponse(tusuario);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR USUARIO: {ex.Message}");
                    throw new Exception($"ERROR AL INSERTAR USUARIO: {ex.Message}");
                }
            }

            return response;
        }

        public ResponseApp ModificarUsuario(Tusuario tusuario)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var usuarioExistente = context.Tusuario.FirstOrDefault(u => u.idUsuario == tusuario.idUsuario);

                if (usuarioExistente != null)
                {
                    usuarioExistente.Email = tusuario.Email;
                    usuarioExistente.Clave = tusuario.Clave;
                    usuarioExistente.Cedula = tusuario.Cedula;
                    usuarioExistente.Estado = tusuario.Estado;
                    usuarioExistente.FModificacion = DateTime.Now;
                    usuarioExistente.UsuarioModificacion = tusuario.UsuarioModificacion;
                    usuarioExistente.IdRol = tusuario.IdRol;

                    context.SaveChanges();
                    response = Utils.OkResponse(usuarioExistente);
                }
                else
                {
                    response = Utils.BadResponse("USUARIO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR USUARIO: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR USUARIO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarUsuarioByCedula(String cedula)
        {

            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el usuario por su ID
                var usuario = context.Tusuario.FirstOrDefault(u => u.Cedula == cedula);

                if (usuario != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(usuario);
                }
                else
                {
                    // Si el usuario no existe
                    response = Utils.BadResponse("USUARIO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR USUARIO: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR USUARIO: {ex.Message}");
            }
            return response;
        }
        public ResponseApp ListarUsuario()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var listarUsuarios = context.Tusuario.ToList();
                response = Utils.OkResponse(listarUsuarios);

            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR USUARIOS: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR USUARIO: {ex.Message}");
            }
            return response;
        }
        public ResponseApp EliminarUsuario(String cedula)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var usuarioExistente = context.Tusuario.FirstOrDefault(u => u.Cedula == cedula);

                if (usuarioExistente != null)
                {
                    usuarioExistente.Estado = "INA";
                    context.SaveChanges();
                    response = Utils.OkResponse(null);
                }
                else
                {
                    response = Utils.BadResponse("USUARIO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR USUARIO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR USUARIO: {ex.Message}");
            }
            return response;
        }
        public ResponseApp BuscarPorEmail(string email)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var usuario = context.Tusuario.Include(pg => pg.Trol).FirstOrDefault(u => u.Email == email);

                if (usuario != null)
                {
                    return Utils.OkResponse(usuario);
                }

            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR USUARIOS POR EMIAL: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR USUARIOS POR EMIAL {ex.Message}");
            }
            return response;
        }

    }

}