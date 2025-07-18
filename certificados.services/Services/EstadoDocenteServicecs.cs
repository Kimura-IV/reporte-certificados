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
    public class EstadoDocenteService
    {
        private readonly TestadoDocenteDA testadoDocenteDA;

        public EstadoDocenteService(TestadoDocenteDA estadoDocenteDA) {
            this.testadoDocenteDA = estadoDocenteDA;
        }

        public ResponseApp CrearEstadoDocente(TestadoDocente testado) {
            if (testadoDocenteDA.ListarByNombre(testado.Nombre).Cod.Equals(Utils.CONSTANTES.COD_OK)){
                return Utils.Utils.BadResponse("YA EXISTE ESTE ESTADO");
            }
            return testadoDocenteDA.InsertarEstadoDocente(testado);
        }

        public ResponseApp ActualizarEstadoDocente(TestadoDocente testado)
        {
            if (!testadoDocenteDA.ListarById(testado.IdEstado).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                return Utils.Utils.BadResponse($"NO  EXISTE ESTE ESTADO {testado.IdEstado}");
            }
            return testadoDocenteDA.ModificarEstadoDocente(testado);
        }
        public ResponseApp ListarEstados() { 
            return testadoDocenteDA.ListarEstados();
        }

        public ResponseApp ListarById(int id) {
            return testadoDocenteDA.ListarById(id);
        }

        public ResponseApp EliminarEstado(int id) {
            if (!testadoDocenteDA.ListarById(id).Cod.Equals(Utils.CONSTANTES.COD_OK))
            {
                return Utils.Utils.BadResponse($"NO  EXISTE ESTE ESTADO {id}");
            }
            return testadoDocenteDA.EliminarEstadoDocente(id);
        }
    }
}
