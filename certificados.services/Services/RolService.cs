using certificados.dal.DataAccess;
using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Utils;

namespace certificados.services.Services
{
    public class RolService
    {
        private readonly TrolDA rolDataAcces;

        public RolService(TrolDA _rolAcces)
        {
            this.rolDataAcces = _rolAcces;
        }

        public ResponseApp CrearRol(Trol rol)
        {
            if (rolDataAcces.BuscarRolByNombre(rol.Nombre).Cod == CONSTANTES.COD_OK) {
                return Utils.Utils.BadResponse($"ROL {rol.Nombre} YA EXISTE");
            }
            return rolDataAcces.insertarRol(rol);
        }

        // Modificar Rol
        public ResponseApp ModificarRol(Trol tRol)
        {
            return rolDataAcces.ModificarRol(tRol);
        }

        // Eliminar Rol
        public ResponseApp EliminarRol(int idRol)
        {
            return rolDataAcces.EliminarRol(idRol);
        }

        // Listar Todos los Roles
        public ResponseApp ListarRoles()
        {
            return rolDataAcces.ListarRol();
        }

        // Buscar Rol por ID
        public ResponseApp BuscarRol(int idRol)
        {
            return rolDataAcces.BuscarRol(idRol);
        }

    }

}
