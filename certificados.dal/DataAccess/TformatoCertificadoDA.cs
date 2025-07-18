using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using certificados.models.Helper;
using certificados.services.Utils;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class TformatoCertificadoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarCertificado(TformatoCertificado tformatoCertificado) {

            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tformatoCertificado.idFormato = context.TformatoCertificado.Count() + 1;
                    tformatoCertificado.FCreacion = Utils.timeParsed(DateTime.Now);
                    context.TformatoCertificado.Add(tformatoCertificado);
                    context.SaveChanges();

                    transaction.Commit();
                    
                    response = Utils.OkResponse(tformatoCertificado);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL INSERTAR FORMATO: {ex.Message}");
                    throw new Exception($"ERROR AL INSERTAR FORMATO CERTIFICADO: {ex.Message}");
                }

                return response;
            } 
        }

        public ResponseApp ModificarFormatoCertificado(TformatoCertificado formato)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction()) { 
            
            try
            { 
                var formatoExistente = context.TformatoCertificado.FirstOrDefault(f => f.idFormato == formato.idFormato);

                if (formatoExistente != null)
                { 
                    formatoExistente.NombrePlantilla = formato.NombrePlantilla;
                    formatoExistente.LineaGrafica = formato.LineaGrafica;
                    formatoExistente.LogoUG = formato.LogoUG;
                    formatoExistente.Origen = formato.Origen;
                    formatoExistente.Tipo = formato.Tipo;
                    formatoExistente.Leyenda = formato.Leyenda;
                    formatoExistente.Qr = formato.Qr;
                    formatoExistente.CargoFirmanteUno = formato.CargoFirmanteUno;
                    formatoExistente.NombreFirmanteUno = formato.NombreFirmanteUno;
                    formatoExistente.CargoFirmanteDos = formato.CargoFirmanteDos;
                    formatoExistente.NombreFirmanteDos = formato.NombreFirmanteDos;
                    formatoExistente.CargoFirmanteTres = formato.CargoFirmanteTres;
                    formatoExistente.NombreFirmanteTres = formato.NombreFirmanteTres;
                    formatoExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                    formatoExistente.UsuarioActualizacion = formato.UsuarioActualizacion;
                     
                    context.SaveChanges();
                     
                    response = Utils.OkResponse(formatoExistente);
                }
                else
                { 
                    response = Utils.BadResponse("FORMATO NO EXISTE");

                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response = Utils.BadResponse($"ERROR AL MODIFICAR FORMATO: {ex.Message}");
                throw new Exception($"ERROR AL MODIFICAR FORMATO: {ex.Message}");
            }
            return response;
            }
              
        }

        public ResponseApp EliminarFormatoCertificado(int idFormato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var formatoExistente = context.TformatoCertificado.FirstOrDefault(f => f.idFormato == idFormato);

                if (formatoExistente != null)
                { 
                    context.TformatoCertificado.Remove(formatoExistente);
                    context.SaveChanges(); 
                    response = Utils.OkResponse(formatoExistente);
                }
                else
                { 
                    response = Utils.BadResponse("FORMATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR FORMATO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR FIRMAS: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarFormatosCertificados()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var listaFormatos = context.TformatoCertificado.ToList(); 
                response = Utils.OkResponse(listaFormatos);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR FORMATOS: {ex.Message}");
                throw new Exception($"ERROR AL LISTAR FIRMAS: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarFormatoCertificado(int idFormato)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            { 
                var formato = context.TformatoCertificado.FirstOrDefault(f => f.idFormato == idFormato);

                if (formato != null)
                { 
                    response = Utils.OkResponse(formato);
                }
                else
                { 
                    response = Utils.BadResponse("FORMATO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR FORMATO: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR FORMATO: {ex.Message}");
            }
            return response;
        }


    }
}
