using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace certificados.dal.DataAccess
{
    internal class Conexion(IConfiguration configuration)
    {
        private readonly string? _cadenaConexion = configuration.GetConnectionString("DefaultConnection");

        public SqlConnection Conectar() {
            SqlConnection conected;
            conected = new SqlConnection(_cadenaConexion);
            return conected;
        }
    }
}
