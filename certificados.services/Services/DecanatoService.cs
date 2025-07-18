using Azure;
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
    public class DecanatoService
    {
        private readonly TdecanatoDA tdecanatoDA;


        public DecanatoService(TdecanatoDA tdecanatoDA)
        {
            this.tdecanatoDA = tdecanatoDA;
        }

        public ResponseApp ListarDecanatos() {
            return tdecanatoDA.ListarDecanatos();
        }

        public ResponseApp ObtenerDecanatoById(int idDecanato)
        { 

            return tdecanatoDA.BuscarDecanato(idDecanato);
        }

        public ResponseApp CrearDecanato(Tdecanato tdecanato)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (tdecanatoDA.BuscarDecanatoByNombre(tdecanato.Nombre).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response.Message = $"DECANATO {tdecanato.Nombre} YA EXISTE";
            }
            else { 
               response = tdecanatoDA.InsertarDecanato(tdecanato);
            }
            return response;
        }

        public ResponseApp ModificarDecanato(Tdecanato tdecanato)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (!tdecanatoDA.BuscarDecanato(tdecanato.IdDecanato).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response.Message = $"DECANATO {tdecanato.IdDecanato} NO EXISTE";
            }
            else
            {
                response = tdecanatoDA.ModificarDecanato(tdecanato);
            }
            return response;
        }

        public ResponseApp EliminarDecanato(int idDecanato)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            if (!tdecanatoDA.BuscarDecanato(idDecanato).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                response.Message = $"DECANATO {idDecanato} NO EXISTE";
            }
            else
            {
                response = tdecanatoDA.EliminarDecanato(idDecanato);
            }
            return response;
        }
    }
}
