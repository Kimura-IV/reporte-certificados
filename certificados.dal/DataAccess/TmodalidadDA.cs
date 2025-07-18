using certificados.models.Context;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using certificados.models.Entitys.auditoria;
using certificados.models.Helper;

namespace certificados.dal.DataAccess
{
    public class TmodalidadDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;
        // Insertar Modalidad
        public ResponseApp InsertarModalidad(Tmodalidad modalidad)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaccion = context.Database.BeginTransaction())
            {

                try
                {
                    Tmodalidad nuevaModalidad = new Tmodalidad
                    {
                        IdModalidad = context.Tmodalidad.Count() + 1,
                        Nombre = Utils.SafeString(modalidad.Nombre),
                        Descripcion = Utils.SafeString(modalidad.Descripcion),
                        Fcreacion = Utils.timeParsed(DateTime.Now),
                        UsusarioIngreso = Utils.SafeString(modalidad.UsusarioIngreso)
                    };

                    context.Tmodalidad.Add(nuevaModalidad);
                    context.SaveChanges();

                    transaccion.Commit();
                    response = Utils.OkResponse(nuevaModalidad);
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    response.Message = $"PROBLEMAS AL INSERTAR MODALIDAD: {ex.Message}";
                    throw new Exception($"ERROR AL INSERTAR MODALIDAD: {ex.Message}");
                }
            }


            return response;
        }

        // Modificar Modalidad
        public ResponseApp ModificarModalidad(Tmodalidad modalidad)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var modalidadExistente = context.Tmodalidad.FirstOrDefault(m => m.IdModalidad == modalidad.IdModalidad);

                if (modalidadExistente != null)
                {
                    modalidadExistente.Nombre = Utils.SafeString(modalidad.Nombre);
                    modalidadExistente.Descripcion = Utils.SafeString(modalidad.Descripcion);
                    modalidadExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    modalidadExistente.UsuarioActualizacion = Utils.SafeString(modalidad.UsuarioActualizacion);

                    context.SaveChanges();

                    response = Utils.OkResponse(modalidadExistente);
                }
                else
                {
                    response.Message = "MODALIDAD NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = "ERROR AL MODIFICAR MODALIDAD: " + ex.Message;
                throw new Exception($"ERROR AL MODIFICAR MODALIDAD: {ex.Message}");
            }
            return response;
        }

        // Eliminar Modalidad
        public ResponseApp EliminarModalidad(int idModalidad)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var modalidadExistente = context.Tmodalidad.FirstOrDefault(m => m.IdModalidad == idModalidad);

                if (modalidadExistente != null)
                {
                    context.Tmodalidad.Remove(modalidadExistente);
                    context.SaveChanges();

                    response = Utils.OkResponse(modalidadExistente);
                }
                else
                {
                    response = Utils.BadResponse("MODALIDAD NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR MODALIDAD: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR MODALIDAD: {ex.Message}");
            }

            return response;
        }

        // Consultar Modalidad por ID
        public ResponseApp ConsultarModalidadById(int idModalidad)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var modalidadExistente = context.Tmodalidad.FirstOrDefault(m => m.IdModalidad == idModalidad);

                if (modalidadExistente != null)
                {
                    response = Utils.OkResponse(modalidadExistente);
                }
                else
                {
                    response.Message = "MODALIDAD NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR MODALIDAD: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR MODALIDAD: {ex.Message}");
            }

            return response;
        }

        // Consultar Todas las Modalidades
        public ResponseApp ListarModalidades()
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var modalidades = context.Tmodalidad.ToList();

                if (modalidades.Any())
                {
                    response = Utils.OkResponse(modalidades);
                }
                else
                {
                    response.Message = "NO EXISTEN MODALIDADES REGISTRADAS";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR MODALIDADES: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR MODALIDADES: {ex.Message}");
            }

            return response;
        }

        // Consultar Modalidad por ID
        public ResponseApp ConsultarModalidadPorNombre(String nombre)
        {
            ResponseApp response = Utils.BadResponse(null);

            try
            {
                var modalidadExistente = context.Tmodalidad.FirstOrDefault(m => m.Nombre == nombre);

                if (modalidadExistente != null)
                {
                    response = Utils.OkResponse(modalidadExistente);
                }
                else
                {
                    response.Message = "MODALIDAD NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL CONSULTAR MODALIDAD: {ex.Message}";
                throw new Exception($"ERROR AL CONSULTAR MODALIDAD: {ex.Message}");
            }

            return response;
        }
    }
}
