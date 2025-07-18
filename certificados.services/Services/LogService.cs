using certificados.models.Context;
using certificados.models.Entitys.dbo;
using certificados.services.Utils;

namespace certificados.services.Services
{
    /**
     Servicio que inserta los LOGS de la aplicacion en cada TRY CATCH...
     */
    public class LogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }
        /**
         Funcion que nos permite registrar los LOGS..s
         */
        public async Task RegistrarExcepcion(Exception ex)
        {
            try
            {
                var log = new Tlog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    ErrorType = ex.GetType().ToString(),
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                await _context.Tlog.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                // Solo registramos en consola si falla el registro en BD
                Console.WriteLine($"ERROR AL REGISTRAR LOG: {logEx.Message}");
            }
        }
    }
}
