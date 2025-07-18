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
    public class EventoService
    {
        private readonly TeventoDA teventoDA;

        public EventoService(TeventoDA teventoDA) { 
        
            this.teventoDA = teventoDA;
        }

        public ResponseApp ListarEventos() { 
        
             return teventoDA.ListarEventos();
        }

        public ResponseApp ListarPorId(int id) {
            return teventoDA.BuscarEvento(id);
        }

        public ResponseApp ListarPorParametro<T>(string propiedad, T valor)
        {
            return teventoDA.ListarEventosPorParametro(propiedad, valor);
        }

        public ResponseApp CrearEvento(Tevento tevento) {
            //var listaResponse = teventoDA.ListarEventosPorParametro("Periodo", tevento.Periodo);
            //var eventoResponse = teventoDA.ListarEventosPorParametro("Dominio", tevento.Dominio);
            //if (listaResponse.Cod.Equals(Utils.CONSTANTES.COD_OK) && 
            //    eventoResponse.Cod.Equals(Utils.CONSTANTES.COD_OK)) {
            //    return Utils.Utils.BadResponse($"YA EXISTE UN EVENTO {tevento.Dominio} en el PERIODO: {tevento.Periodo}");
            //}
            return teventoDA.InsertarEvento(tevento);
        }

        public ResponseApp ActualizarEvento(Tevento tevento)
        {
            if (!teventoDA.BuscarEvento(tevento.Idevento).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                return Utils.Utils.BadResponse($"NO  EXISTE  EVENTO {tevento.Idevento}");
            }
            return teventoDA.ModificarEvento(tevento);
        }

        public ResponseApp EliminarEvento(int id)
        {
            if (!teventoDA.BuscarEvento(id).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                return Utils.Utils.BadResponse($"NO  EXISTE  EVENTO {id}");
            }
            return teventoDA.EliminarEvento(id);
        }
    }
}
