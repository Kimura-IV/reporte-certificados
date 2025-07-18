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
    public class ActaCalificacionService
    {
        private readonly TactaCalificacionDA tactaCalificacionDA;
        private readonly TeventoDA eventoDA;

        public ActaCalificacionService(TeventoDA tevento, TactaCalificacionDA calificacion) { 
        
            this.eventoDA = tevento;
            this.tactaCalificacionDA = calificacion;
        }

        public ResponseApp Insertar(TactaCalificacion calificacion) {

            return tactaCalificacionDA.InsertarActaCalificacion(calificacion);
         }
        public ResponseApp Listar() {

            return tactaCalificacionDA.ListarActasCalificacion();
        }

        public ResponseApp BuscarId(int id)
        {

            return tactaCalificacionDA.BuscarActaCalificacion(id);

        }

        public ResponseApp Eliminar(int id) {


            return tactaCalificacionDA.EliminarActaCalificacion(id);

        }

    }
}
