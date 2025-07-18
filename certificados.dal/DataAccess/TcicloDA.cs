using certificados.models.Context;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using certificados.services.Utils;
using certificados.models.Entitys.auditoria;
using certificados.models.Helper;

namespace certificados.dal.DataAccess
{
    public class TcicloDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;
        // Insertar Ciclo
        public ResponseApp insertarCiclo(Tciclo ciclo)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    Tciclo nuevoCiclo = new Tciclo
                    {
                        IdCiclo = context.Tciclo.Count() + 1,
                        Nombre = Utils.SafeString(ciclo.Nombre),
                        Descripcion = Utils.SafeString(ciclo.Descripcion),
                        FCreacion = Utils.timeParsed(DateTime.Now),
                        UsuarioIngreso = Utils.SafeString(ciclo.UsuarioIngreso)
                    };

                    context.Tciclo.Add(nuevoCiclo);
                    context.SaveChanges();  

                    transaccion.Commit();
                    response = Utils.OkResponse(nuevoCiclo);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response.Message = $"PROBLEMAS AL INSERTAR CICLO: {ex.Message}";
                    throw new Exception($"ERROR AL INSERTAR CICLO: {ex.Message}");
                }
            }
            return response;
        }

        // Modificar Ciclo
        public ResponseApp modificarCiclo(Tciclo ciclo)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var cicloExistente = context.Tciclo.FirstOrDefault(m => m.IdCiclo == ciclo.IdCiclo);

                if (cicloExistente != null)
                {
                    cicloExistente.Nombre = Utils.SafeString(ciclo.Nombre);
                    cicloExistente.Descripcion = Utils.SafeString(ciclo.Descripcion);
                    cicloExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    cicloExistente.UserModificacion = Utils.SafeString(ciclo.UserModificacion);

                    context.SaveChanges();

                    response = Utils.OkResponse(cicloExistente);
                }
                else
                {
                    response.Message = "CICLO NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = "ERROR AL MODIFICAR CICLO: " + ex.Message;
                throw new Exception($"ERROR AL MODIFICAR CICLO: {ex.Message}");
            }
            return response;
        }

        // Eliminar Ciclo
        public ResponseApp eliminarCiclo(int idCiclo)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var cicloExistente = context.Tciclo.FirstOrDefault(m => m.IdCiclo == idCiclo);

                if (cicloExistente != null)
                {
                    context.Tciclo.Remove(cicloExistente);
                    context.SaveChanges();

                    response = Utils.OkResponse(cicloExistente);
                }
                else
                {
                    response = Utils.BadResponse("CICLO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR CICLO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR CICLO: {ex.Message}");
            }

            return response;
        }

        // Consultar Ciclo por ID
        public ResponseApp consultarCicloById(int idCiclo)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var cicloExistente = context.Tciclo.FirstOrDefault(m => m.IdCiclo == idCiclo);

                if (cicloExistente != null)
                {
                    response = Utils.OkResponse(cicloExistente);
                }
                else
                {
                    response.Message = "CICLO NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR CICLO: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR CICLO: {ex.Message}");
            }

            return response;
        }

        // Consultar Todas los Ciclos
        public ResponseApp listarCiclos()
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var ciclo = context.Tciclo.ToList();

                if (ciclo.Any())
                {
                    response = Utils.OkResponse(ciclo);
                }
                else
                {
                    response.Message = "NO EXISTEN CICLOS REGISTRADAS";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR CICLOS: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR CICLOS: {ex.Message}");
            }

            return response;
        }

        // Consultar Ciclo por ID
        public ResponseApp consultarCicloPorNombre(String nombre)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var cicloExistente = context.Tciclo.FirstOrDefault(m => m.Nombre == nombre);

                if (cicloExistente != null)
                {
                    response = Utils.OkResponse(cicloExistente);
                }
                else
                {
                    response.Message = "CICLO NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR CICLO: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR CICLO: {ex.Message}");
            }

            return response;
        }
    }
}
