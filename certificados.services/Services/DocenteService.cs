using certificados.dal.DataAccess;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace certificados.services.Services
{
    public class DocenteService
    {
        private readonly TdocenteDA tdocenteDA;

        public DocenteService(TdocenteDA tdocenteDA)
        {
            this.tdocenteDA = tdocenteDA;
        }

        public ResponseApp ListarDocentes()
        {
            
            return tdocenteDA.ListarDocentes();
        }

        public ResponseApp ObtenerDocentesById(string codigoDocente)
        {
            return tdocenteDA.BuscarDocente(codigoDocente);
        }
        public ResponseApp ObtenerDocentesByCedula(string cedula)
        {
            return tdocenteDA.BuscarDocenteCedula(cedula);
        }


        public ResponseApp CrearDocente(Tdocente tdocente)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (!tdocenteDA.BuscarDocente(tdocente.CodigoDocente).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response = tdocenteDA.InsertarDocente(tdocente);
            }
            else {
                response.Message = "DOCENTE YA EXISTE";
            }
            return response;
        }

        public ResponseApp ActualizarDocente(Tdocente tdocente)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (tdocenteDA.BuscarDocente(tdocente.CodigoDocente).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response = tdocenteDA.ModificarDocente(tdocente);
            }
            else
            {
                response.Message = "DOCENTE NO EXISTE";
            }
            return response;
        }

        public ResponseApp ElminarDocente(string codigoDocente)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (tdocenteDA.BuscarDocente(codigoDocente).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response = tdocenteDA.EliminarDocente(codigoDocente);
            }
            else
            {
                response.Message = "DOCENTE NO EXISTE";
            }
            return response;
        }
    }
}
