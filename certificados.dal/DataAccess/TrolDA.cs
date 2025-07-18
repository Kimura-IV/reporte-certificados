using System.Data;
using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using certificados.models.Helper;
using certificados.services.Utils;
using Microsoft.EntityFrameworkCore;

namespace certificados.dal.DataAccess

{
    public class TrolDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp insertarRol(Trol tRol)
        {
            ResponseApp response = Utils.BadResponse(null);

            using (var transaccion = context.Database.BeginTransaction()) { 
             try
                {
                    var existeRol = context.Trol.Any(e => e.Nombre.ToLower() == tRol.Nombre.ToLower());
                    if (existeRol)
                    {
                        response.Message = "El nombre de este ROL ya Existe";
                        return response;
                    }
                    Trol insertRol = new Trol();

                    insertRol.IdRol = context.Trol.Count() + 1;
                    insertRol.Nombre = Utils.SafeString(tRol.Nombre);
                    insertRol.Observacion = Utils.SafeString(tRol.Observacion);
                    insertRol.FCreacion = Utils.timeParsed(DateTime.Now);
                    insertRol.UsuarioIngreso = Utils.SafeString(tRol.UsuarioIngreso);
                    insertRol.Estado = tRol.Estado;
                    context.Trol.Add(insertRol);
                    context.SaveChanges();
                    transaccion.Commit();
                    response = Utils.OkResponse(tRol);

                }
                catch (Exception ex)
                { 
                    transaccion.Rollback();
                    response.Message = $"PROBLEMAS AL INSERTAR ROL: {ex.Message}";

                    throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
                }
            }
               
            
            return response;
        }

        public ResponseApp ModificarRol(Trol tRol)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var rolExistente = context.Trol.FirstOrDefault(t => t.IdRol == tRol.IdRol);

                if (rolExistente != null)
                {
                    rolExistente.Nombre = Utils.SafeString(tRol.Nombre);
                    rolExistente.Observacion = tRol.Observacion;
                    rolExistente.Estado = tRol.Estado;
                    rolExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    rolExistente.UsuarioModificacion = tRol.UsuarioModificacion; 

                    context.SaveChanges();

                    response = Utils.OkResponse(rolExistente);
                }
                else
                {
                     response.Message = "ROL NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = "ERROR AL MODIFICAR ROL: " + ex.Message;
                throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
            }
            return response;
        }


        public ResponseApp EliminarRol(int idRol)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var rolExistente = context.Trol.FirstOrDefault(t => t.IdRol == idRol);

                if (rolExistente != null)
                {
                    rolExistente.Estado = false;
                    context.SaveChanges();
                    response = Utils.OkResponse(rolExistente);
                }
                else
                {
                    response = Utils.BadResponse("ROL NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR ROL: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
            }

            return response;
        }


        public ResponseApp ListarRol(){
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var listaRoles = context.Trol.Where(e => e.Estado == true || e.Estado == false).ToList();
                if (listaRoles.Any())
                {
                    response = Utils.OkResponse(listaRoles);
                }
                else
                {
                    response = Utils.BadResponse("NO SE ENCONTRARON ROLES.");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"SE PRODUJO UN ERROR AL LISTAR LOS ROLES: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
            }

            return response;
        }

        public ResponseApp BuscarRol(int idRol)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var trol = context.Trol.FirstOrDefault(t => t.IdRol == idRol);

                if (trol != null)
                {
                    response = Utils.OkResponse(trol);
                }
                else
                {
                    response = Utils.BadResponse($"EL ROL CON ID {idRol} NO EXISTE.");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR ROL: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
            }

            return response;
        }
        public ResponseApp BuscarRolByNombre(string nombreRol)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var trol = context.Trol.FirstOrDefault(t => t.Nombre == nombreRol);

                if (trol != null)
                {
                    response = Utils.OkResponse(trol);
                }
                else
                {
                    response = Utils.BadResponse($"EL ROL CON ID {nombreRol} NO EXISTE.");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR ROL: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR ROL: {ex.Message}");
            }

            return response;
        }
    }
}
