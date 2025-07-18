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
    public class ActaAsistenciaService
    {
        private readonly TactAsistenciaDA tactAsistenciaDA;


        public ActaAsistenciaService(TactAsistenciaDA asistenciaDA) { 
            this.tactAsistenciaDA = asistenciaDA;
        }

        public ResponseApp InsertarAsistencia(TactaAsistencia acta) {

            return tactAsistenciaDA.InsertarActaAsistencia(acta);
        }

        public ResponseApp ModificarAsistencia(TactaAsistencia acta) {

            return tactAsistenciaDA.InsertarActaAsistencia(acta);
        }

        public ResponseApp Listar() { 
            return tactAsistenciaDA.ListarActasAsistencia();    
        }

        public ResponseApp BuscarId(int id) { 
            return tactAsistenciaDA.BuscarActaAsistencia(id);
        }

        public ResponseApp ELiminar(int id) {
            return tactAsistenciaDA.EliminarActaAsistencia(id);
        }
    }
}
