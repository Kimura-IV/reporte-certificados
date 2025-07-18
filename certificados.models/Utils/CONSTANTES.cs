using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Utils
{
    public class CONSTANTES
    {
        //RESPUESTAS HTTP
        public static string COD_OK = "OK";
        public static string MESSAGE_OK = "PROCESO COMPLETADO CON EXITO";

        public static string COD_ERROR = "ERROR01";
        public static string MESSAGE_ERROR = "PROCESO NO COMPLETADO, INTENTE MAS TARDE";

        public static string COD_DEFAULT = "ERROR02";
        public static string MESSAGE_DEFAULT = "SERVICIO NO DISPONIBLE, INTENTE MAS TARDE";

        //Codigos para JWT
        public static string COD_JWT_ERROR = "ERROR04";
        public static string MESSAGE_JWT_ERROR = "TOKEN NO VALIDADO";

        public static string COD_JWT_UNKNOW = "ERROR05";
        public static string MESSAGE_JWT_UNKNOW = "TOKEN DESCONOCIDO";


        //LOGS
        public static string ML_INI = "\nINICIO -->";
        public static string ML_ERROR = "\nERROR ===>";
        public static string ML_FIN = "\nFIN -->";

        //Para datos duplicados
        public static string MESSAGE_DATA_EXISTE = "DATOS DUPLICADOS:  ";
        public static string MESSAGE_DATA_ERRORS = "ERROR EN LOS DATOS DE ENVIO";
        public static List<string> ROLES = new List<string>() { "ADMIN", "FACILITADOR", "DECANO", "VICERRECTOR" };
    }
}
