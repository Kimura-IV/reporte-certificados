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
    public class GrupoPersonaService
    {
        private readonly TgrupoPersonaDA tgrupoPersonaDA;
        private readonly TusuarioDA tusuarioDA;

        public GrupoPersonaService(TgrupoPersonaDA grupoDA, TusuarioDA tusuarioDA) { 
        
            this.tgrupoPersonaDA = grupoDA;
            this.tusuarioDA = tusuarioDA;
        }

        public ResponseApp InsertarPersona(TgrupoPersona grupoPersonaDTO) {
            
            return tgrupoPersonaDA.InsertarGrupoPersona(grupoPersonaDTO);

        }
        public ResponseApp ListarGrupo() { 
        
        return tgrupoPersonaDA.ListarGrupoPersonas();
        }

        public ResponseApp BuscarById(int id, bool estado = false) { 
            return tgrupoPersonaDA.BuscarGrupoPersona(id, estado);
        }
        public ResponseApp BuscarCedulaId(int id, string cedula) { 
            return tgrupoPersonaDA.BuscarCedulaIdGrupo(id, cedula);
        }

        public ResponseApp EliminarGrupo(int id, string cedula) {
            return tgrupoPersonaDA.EliminarGrupoPersona(id,  cedula);
        }
        public ResponseApp AprobarGrupos(List<String> cedulas, int id, string usuarioActualizacion) { 
            return tgrupoPersonaDA.AprobarGruposPersonas(cedulas, id, usuarioActualizacion);
        }

        public ResponseApp BuscarBeneficiarios(int id, int rol, bool estado = false)
        {
            ResponseApp responseApp = tgrupoPersonaDA.BuscarGrupoPersona(id, estado);
            dynamic datosPersona = responseApp.Data;

            // Filtrar los datos que cumplen con la condición usando LINQ
            var datosFiltrados = ((IEnumerable<TgrupoPersona>)datosPersona)
                .Where(gp =>
                {
                    ResponseApp resApp = tusuarioDA.BuscarUsuarioByCedula(gp.Cedula);
                    dynamic usuario = resApp.Data;
                    return usuario.IdRol == rol;
                })
                .ToList();

            // Crear un nuevo ResponseApp con los datos filtrados
            ResponseApp responseAll = new ResponseApp
            {
                Cod = responseApp.Cod,
                Message = responseApp.Message,
                Data = datosFiltrados
            };

            return responseAll;
        }
    }
}
