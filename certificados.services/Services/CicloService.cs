using certificados.dal.DataAccess;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using certificados.services.Utils;

namespace certificados.services.Services
{
    public class CicloService
    {
        private readonly TcicloDA cicloDA;

        public CicloService(TcicloDA cicloDA) { 
        
            this.cicloDA = cicloDA;
        }
        // Insertar Ciclo
        public ResponseApp InsertarCiclo(Tciclo ciclo)
        {
            ResponseApp grupoResponse = ConsultarCicloPorNombre(ciclo.Nombre);
            dynamic data = grupoResponse.Data;

            if ( grupoResponse.Cod == CONSTANTES.COD_OK && ciclo.Nombre.Equals(data.Nombre))
            {
                return Utils.Utils.BadResponse("CICLO " + ciclo.Nombre.ToString() + " YA EXISTENTE");
            }

            return cicloDA.insertarCiclo(ciclo);
        }

        // Modificar Ciclo
        public ResponseApp ModificarCiclo(Tciclo ciclo)
        {
            return cicloDA.modificarCiclo(ciclo);
        }

        // Eliminar Ciclo
        public ResponseApp EliminarCiclo(int idCiclo)
        {
            return cicloDA.eliminarCiclo(idCiclo);
        }

        // Consultar Ciclo por ID
        public ResponseApp ConsultarCiclo(int idCiclo)
        {
            return cicloDA.consultarCicloById(idCiclo);
        }

        // Consultar Todas las Ciclos
        public ResponseApp ListarCiclos()
        {
            return cicloDA.listarCiclos();
        }

        // Consultar Ciclo por ID
        public ResponseApp ConsultarCicloPorNombre(String nombre)
        {
            return cicloDA.consultarCicloPorNombre(nombre);
        }
    }
}
