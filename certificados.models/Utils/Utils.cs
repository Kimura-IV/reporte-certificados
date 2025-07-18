
using certificados.models.Entitys;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace certificados.services.Utils
{
    public class Utils
    {

        /**
        Funcion que genera una Respuesta Correcta
        */
        public static ResponseApp OkResponse(Object data)
        {
            return new ResponseApp(CONSTANTES.COD_OK, CONSTANTES.MESSAGE_OK, data);
        }

        /**
         Funcion que genera una respuesta Incorrecta
         */
        public static ResponseApp BadResponse(Object data)
        {
            return new ResponseApp(CONSTANTES.COD_ERROR, CONSTANTES.MESSAGE_ERROR, data);
        }   

        /**
         Metodo que nos permite asegurar valores de String eliminando caracteres
         */
        public static string SafeString(String data)
        {
            if (string.IsNullOrEmpty(data) || data ==null)
            {
                return "";
            }
            string cleanedData = Regex.Replace(data, @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s]+", "");
            return cleanedData.Trim();
        }

        /**
         Metodo que tranforma cualquier objecto en JSON
         */
        public static string convertToJson(object data)
        {

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            return JsonSerializer.Serialize(data, options);

        }

        /**
         Funcion que sirve para devolver una fecha Parseada
         */
        public static DateTime timeParsed(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, fecha.Hour, fecha.Minute, fecha.Second);
        }
        /**
         Funcion que devuelve un Genero de la persona segun el ID de Genero
         */
        public static string GetGenero(int id)
        {
            string genero = "N";
            switch (id)
            {
                case 1:
                    genero = "M";
                    break;
                case 2:
                    genero = "F";
                    break;
                case 3:
                    genero = "N";
                    break;
            }
            return genero;
        }
        /**
         Funcion que permite verificar si la cedula esta mal
         */
        public static bool ValidarCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula) || cedula.Length != 10 || !cedula.All(char.IsDigit))
                return false;

            try
            {
                int provincia = int.Parse(cedula.Substring(0, 2));
                if (provincia < 1 || provincia > 24)
                    return false;

                int tercerDigito = int.Parse(cedula[2].ToString());
                if (tercerDigito < 0 || tercerDigito > 6)
                    return false;

                int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
                int verificador = int.Parse(cedula[9].ToString());
                int suma = 0;

                for (int i = 0; i < 9; i++)
                {
                    int valor = int.Parse(cedula[i].ToString()) * coeficientes[i];
                    suma += valor > 9 ? valor - 9 : valor;
                }

                int digitoVerificador = 10 - (suma % 10);
                if (digitoVerificador == 10) digitoVerificador = 0;

                return digitoVerificador == verificador;
            }
            catch
            {
                return false;
            }
        }
    }
}
