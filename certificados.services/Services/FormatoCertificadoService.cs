using certificados.dal.DataAccess;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class FormatoCertificadoService
    {
        private readonly TformatoCertificadoDA tformatoCertificadoDA;
        public FormatoCertificadoService(TformatoCertificadoDA formatoCertificadoDA) { 
        
            this.tformatoCertificadoDA = formatoCertificadoDA;
        }

        public ResponseApp ListarFormatos() { 
            
            return tformatoCertificadoDA.ListarFormatosCertificados();
        }

        public ResponseApp ListarFormatoByID(int idFormato) {

            return tformatoCertificadoDA.BuscarFormatoCertificado(idFormato);
        }
        public ResponseApp CrearFormato(TformatoCertificado tformato) { 
           
            return tformatoCertificadoDA.InsertarCertificado(tformato);
        }

        public ResponseApp EliminarFormato(int idFormato) {

            return tformatoCertificadoDA.EliminarFormatoCertificado(idFormato);
        }
        public ResponseApp modificarFormato(TformatoCertificado tformato)
        {
            return tformatoCertificadoDA.ModificarFormatoCertificado(tformato);
        }
    }
}
