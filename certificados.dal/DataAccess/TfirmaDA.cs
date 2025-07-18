using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TfirmaDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;
        //AQUI NOS UQUEDAMOS

        public ResponseApp InsertarFirma(Tfirma tfirma) {

            ResponseApp response = Utils.BadResponse(null);

            try
            {
                context.Tfirma.Add(tfirma);
                context.SaveChanges();
                 
                response = Utils.OkResponse(tfirma);
            }
            catch (Exception ex) {
                response = Utils.BadResponse($"ERROR AL INSERTAR FIRMA: {ex.Message}");
                throw new Exception($"ERROR AL INSERTAR FIRMA: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ModificarFirma(Tfirma tfirma) {

            ResponseApp response = Utils.BadResponse(null);
            try
            {
                var firmaExistente = context.Tfirma.FirstOrDefault(f => f.IdFirma == tfirma.IdFirma);

                if (firmaExistente != null)
                {
                    // Actualizar los campos de la firma
                    firmaExistente.Cedula = tfirma.Cedula;
                    firmaExistente.FCaducidad = tfirma.FCaducidad;
                    firmaExistente.Password = tfirma.Password;
                    firmaExistente.Firma = tfirma.Firma;
                    firmaExistente.Cargo = tfirma.Cargo;
                    firmaExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    firmaExistente.UsuarioActualizacion = tfirma.UsuarioActualizacion;

     
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(firmaExistente);
                }
                else
                { 
                    response.Message = "FIRMA NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL MODIFICAR FIRMA: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR FIRMAS: {ex.Message}");
            }


            return response;
        }

        public ResponseApp ListarFirmas()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var listaFirmas = context.Tfirma.ToList();
                 
                response = Utils.OkResponse(listaFirmas);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR FIRMAS: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR FIRMAS: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarFirma(int idFirma)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var firma = context.Tfirma.FirstOrDefault(f => f.IdFirma == idFirma);

                if (firma != null)
                { 
                    response = Utils.OkResponse(firma);
                }
                else
                { 
                    response.Message = "FIRMA NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR FIRMA: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR FIRMAS: {ex.Message}");
            }
            return response;
        }
    }
}
