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
    public class TdecanatoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarDecanato(Tdecanato tdecanato)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tdecanato.IdDecanato = context.Tdecanato.Count() + 1;
                    context.Tdecanato.Add(tdecanato);
                    context.SaveChanges();

                    transaction.Commit();

                    response = Utils.OkResponse(tdecanato);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR DECANATO: {ex.Message}");

                    throw new Exception("ERROR AL PROCESAR LA INSERACION DECANATO");
                }
                return response;
            }

        }
        public ResponseApp ModificarDecanato(Tdecanato decanato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el decanato existente
                var decanatoExistente = context.Tdecanato.FirstOrDefault(d => d.IdDecanato == decanato.IdDecanato);

                if (decanatoExistente != null)
                {
                    // Actualizar los campos del decanato
                    decanatoExistente.Nombre = decanato.Nombre;
                    decanatoExistente.FModificacion = DateTime.Now;
                    decanatoExistente.UsuarioActualizacion = decanato.UsuarioActualizacion;

                    // Guardar los cambios
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(decanatoExistente);
                }
                else
                {
                    // Si el decanato no existe
                    response = Utils.BadResponse("DECANATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR DECANATO: {ex.Message}");
                throw new Exception("ERROR AL MODIFICAR DECANATO");
            }
            return response;
        }

        public ResponseApp EliminarDecanato(int idDecanato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el decanato existente
                var decanatoExistente = context.Tdecanato.FirstOrDefault(d => d.IdDecanato == idDecanato);

                if (decanatoExistente != null)
                {
                    // Eliminar el decanato
                    context.Tdecanato.Remove(decanatoExistente);
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(decanatoExistente);
                }
                else
                {
                    // Si el decanato no existe
                    response = Utils.BadResponse("DECANATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR DECANATO: {ex.Message}");
                throw new Exception("ERROR AL ELIMINAR DECANATO");
            }
            return response;
        }
        public ResponseApp ListarDecanatos()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Obtener todos los decanatos de la base de datos
                var listaDecanatos = context.Tdecanato.ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaDecanatos);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR DECANATOS: {ex.Message}");
                throw new Exception("ERROR AL LISTAR DECANATOS");
            }
            return response;
        }

        public ResponseApp BuscarDecanato(int idDecanato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el decanato por su IdDecanato
                var decanato = context.Tdecanato.FirstOrDefault(d => d.IdDecanato == idDecanato);

                if (decanato != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(decanato);
                }
                else
                {
                    // Si el decanato no existe
                    response = Utils.BadResponse("DECANATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR DECANATO: {ex.Message}");
                throw new Exception("ERROR AL BUSCAR DECANATO");
            }
            return response;
        }

        public ResponseApp BuscarDecanatoByNombre(string nombreDecanato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el decanato por su IdDecanato
                var decanato = context.Tdecanato.FirstOrDefault(d => d.Nombre == nombreDecanato);

                if (decanato != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(decanato);
                }
                else
                {
                    // Si el decanato no existe
                    response = Utils.BadResponse("DECANATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR DECANATO: {ex.Message}");
                throw new Exception("ERROR AL BUSCAR DECANATO");
            }
            return response;
        }
    }
}
