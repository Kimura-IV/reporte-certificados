using certificados.dal.DataAccess;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class ModalidadService
    {
        private readonly TmodalidadDA tmodalidadDA;

        public ModalidadService(TmodalidadDA modalidadDA) { 
        
            this.tmodalidadDA = modalidadDA;
        }
        // Insertar Modalidad
        public ResponseApp InsertarModalidad(Tmodalidad modalidad)
        {
            ResponseApp grupoResponse = ConsultarModalidadPorNombre(modalidad.Nombre);
            dynamic data = grupoResponse.Data;

            if ( data !=null && modalidad.Nombre.Equals(data.Nombre))
            {
                return Utils.Utils.BadResponse("MODALIDAD " + modalidad.Nombre.ToString() + " YA EXISTENTE");
            }

            return tmodalidadDA.InsertarModalidad(modalidad);
        }

        // Modificar Modalidad
        public ResponseApp ModificarModalidad(Tmodalidad modalidad)
        {
            return tmodalidadDA.ModificarModalidad(modalidad);
        }

        // Eliminar Modalidad
        public ResponseApp EliminarModalidad(int idModalidad)
        {
            return tmodalidadDA.EliminarModalidad(idModalidad);
        }

        // Consultar Modalidad por ID
        public ResponseApp ConsultarModalidad(int idModalidad)
        {
            return tmodalidadDA.ConsultarModalidadById(idModalidad);
        }

        // Consultar Todas las Modalidades
        public ResponseApp ListarModalidades()
        {
            return tmodalidadDA.ListarModalidades();
        }

        // Consultar Modalidad por NOMBRE
        public ResponseApp ConsultarModalidadPorNombre(String nombre)
        {
            return tmodalidadDA.ConsultarModalidadPorNombre(nombre);
        }
    }
}
