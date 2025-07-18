using certificados.dal.DataAccess;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using certificados.services.Utils;

namespace certificados.services.Services
{
    public class GrupoService
    {
        private readonly TgrupoDA tgrupoDA;

        public GrupoService(TgrupoDA tgrupoDA)
        {
            this.tgrupoDA = tgrupoDA;
        }

        // Insertar un nuevo grupo
        public ResponseApp InsertarGrupo(Tgrupo tgrupo)
        {
            ResponseApp grupoResponse = BuscarGrupoPorNombre(tgrupo.Nombre);
            dynamic data = grupoResponse.Data;

            if (  grupoResponse.Cod == CONSTANTES.COD_OK && tgrupo.Nombre.Equals(data.Nombre)) {
                return Utils.Utils.BadResponse("GRUPO " + tgrupo.Nombre.ToString() + " YA EXISTENTE");
            }

            if (tgrupo.Cantidad <= 0 || tgrupo.Nombre.Length <= 1)
            {
                return Utils.Utils.BadResponse("PARAMETROS DE CREACION NO VALIDO");
            }
            return tgrupoDA.InsertarGrupo(tgrupo);
        }

        // Modificar un grupo existente
        public ResponseApp ModificarGrupo(Tgrupo tgrupo)
        {
            if (tgrupo.Cantidad <= 0 || tgrupo.Nombre.Length <= 1) {
                return Utils.Utils.BadResponse("PARAMETROS DE ACTUALIZACION NO VALIDO");
            }
            return tgrupoDA.ModificarGrupo(tgrupo);
        }

        // Eliminar un grupo por su ID
        public ResponseApp EliminarGrupo(int idGrupo)
        {
            return tgrupoDA.EliminarGrupo(idGrupo);
        }

        // Buscar un grupo por su ID
        public ResponseApp BuscarGrupo(int idGrupo)
        {
            return tgrupoDA.BuscarGrupo(idGrupo);
        }

        // Consultar todos los grupos
        public ResponseApp ListarGrupos()
        {
            try
            {
                return tgrupoDA.ListarGrupos();
            }
            catch (Exception ex)
            {
                return Utils.Utils.BadResponse($"ERROR AL CONSULTAR TODOS LOS GRUPOS: {ex.Message}");
            }
        }

        // Buscar un grupo por nombre
        public ResponseApp BuscarGrupoPorNombre(String nombre)
        {
            return tgrupoDA.BuscarGrupoPorNombre(nombre);
        }
    }
}
