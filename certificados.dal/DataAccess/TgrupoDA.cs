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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TgrupoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarGrupo(Tgrupo tgrupo)
        {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {

                try
                {
                    tgrupo.IdGrupo = context.Tgrupo.Count() + 1;
                    tgrupo.FCreacion = Utils.timeParsed(DateTime.Now);
                    tgrupo.FModificacion = Utils.timeParsed(DateTime.Now);
                    context.Tgrupo.Add(tgrupo);
                    context.SaveChanges();

                    transaction.Commit();

                    response = Utils.OkResponse(tgrupo);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = $"ERROR AL INSERTAR GRUPO: {ex.Message}";
                    throw new Exception($"ERROR AL INSERTAR GRUPO: {ex.Message}");
                }

                return response;
            }

        }

        public ResponseApp ModificarGrupo(Tgrupo grupo)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var grupoExistente = context.Tgrupo.FirstOrDefault(g => g.IdGrupo == grupo.IdGrupo);

                    if (grupoExistente != null)
                    {
                        grupoExistente.Nombre = grupo.Nombre;
                        grupoExistente.Cantidad = grupo.Cantidad;
                        grupoExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                        grupoExistente.UsuarioActualizacion = grupo.UsuarioActualizacion;

                        context.SaveChanges();
                        transaction.Commit();

                        response = Utils.OkResponse(grupoExistente);
                    }
                    else
                    {
                        response = Utils.BadResponse("GRUPO NO EXISTE");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL MODIFICAR GRUPO: {ex.Message}");
                    throw new Exception($"ERROR AL MODIFICAR GRUPO: {ex.Message}");
                }
                return response;
            }

        }

        public ResponseApp EliminarGrupo(int idGrupo)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Buscar el grupo existente por su ID
                    var grupoExistente = context.Tgrupo.FirstOrDefault(g => g.IdGrupo == idGrupo);

                    if (grupoExistente != null)
                    {
                        // Eliminar el grupo
                        context.Tgrupo.Remove(grupoExistente);
                        context.SaveChanges();
                        transaction.Commit( );

                        // Retornar respuesta exitosa
                        response = Utils.OkResponse(grupoExistente);
                    }
                    else
                    {
                        // Si el grupo no existe
                        response = Utils.BadResponse("GRUPO NO EXISTE");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback( );
                    response.Message = $"ERROR AL ELIMINAR GRUPO: {ex.Message}";
                    throw new Exception($"ERROR AL ELIMINAR GRUPO: {ex.Message}");
                }
                return response;

            }

        }

        public ResponseApp ListarGrupos()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var listaGrupos = context.Tgrupo.ToList();
                response = Utils.OkResponse(listaGrupos);
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL LISTAR GRUPOS: {ex.Message}";
                throw new Exception($"ERROR AL LISTAR GRUPO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarGrupo(int idGrupo)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var grupo = context.Tgrupo.FirstOrDefault(g => g.IdGrupo == idGrupo);

                if (grupo != null)
                {
                    response = Utils.OkResponse(grupo);
                }
                else
                {
                    response = Utils.BadResponse("GRUPO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL BUSCAR GRUPO: {ex.Message}";
                throw new Exception($"ERROR AL BUSCAR GRUPO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarGrupoPorNombre(String nombre)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var grupo = context.Tgrupo.FirstOrDefault(g => g.Nombre == nombre);

                if (grupo != null)
                {
                    response = Utils.OkResponse(grupo);
                }
                else
                {
                    response = Utils.BadResponse("GRUPO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL BUSCAR GRUPO: {ex.Message}";
                throw new Exception($"ERROR AL BUSCAR GRUPO: {ex.Message}");
            }
            return response;
        }

    }
}
