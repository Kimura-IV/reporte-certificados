using certificados.dal.DataAccess;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class ExpositorService
    {
        private readonly TexpositorDA expositorDataAcces;
        private readonly TpersonaDA personaDataAcces;

        public ExpositorService(TexpositorDA expositorService, TpersonaDA personaDataAcces)
        {
            this.expositorDataAcces = expositorService;
            this.personaDataAcces = personaDataAcces;
        }

        public ResponseApp crearExpositor(Texpositor expositor)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);
            try
            {
                response = expositorDataAcces.InsertarExpositor(expositor);

            }
            catch (Exception ex) {
                response.Message = $"ERROR AL CREAR EXPOSITOR: {expositor.Tpersona.Cedula}";
                throw new Exception($"ERROR AL CREAR EXPOSITOR: {ex.Message}", ex);
            }

            return response;

        }

        public ResponseApp modificarExpositor(Texpositor texpositor) {

            ResponseApp response = Utils.Utils.BadResponse(null);
            try
            {
                response = expositorDataAcces.ModificarExpositor(texpositor);

            }
            catch (Exception ex) {

                response.Message = $"ERROR AL MODIFICAR EXPOSITOR: {texpositor.Tpersona.Cedula}";
                throw new Exception($"ERROR AL MODIFICAR EXPOSITOR: {ex.Message}", ex);
            }
            return response;

        }

        public ResponseApp listarExpositores() {

            return expositorDataAcces.ListarExpositores();
        }

        public ResponseApp buscarPorID(int id) {

            return expositorDataAcces.BuscarExpositor(id);
        }

        public ResponseApp eliminarExpositor(Texpositor texpositor)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);

            try
            {

                if (!expositorDataAcces.BuscarExpositor(texpositor.IdExpositor).Cod.Equals(CONSTANTES.COD_OK) || !personaDataAcces.BuscarPersona(texpositor.Cedula).Cod.Equals(CONSTANTES.COD_OK )){
                    response.Message = "EXPOSITOR NO EXISTE";
                }

                response = expositorDataAcces.EliminarExpositor(texpositor.IdExpositor);
            }
            catch (Exception ex) {
                response.Message = $"ERROR AL ELMINAR EXPOSITOR: {texpositor.Tpersona.Cedula}";
                throw new Exception($"ERROR AL MODIFICAR EXPOSITOR: {ex.Message}", ex);
            }
            return response;

        }
    }
}
