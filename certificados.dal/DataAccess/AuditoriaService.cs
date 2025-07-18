using certificados.models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class AuditoriaService
    {
        private readonly AppDbContext _context;

        public AuditoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAuditoriaAsync<TEntidad, TAuditoria>( TEntidad entidad, TAuditoria auditoria) 
            where TEntidad : class  where TAuditoria : class{

            // Copiar propiedades de la entidad a la auditoría
            var propiedadesEntidad = typeof(TEntidad).GetProperties();
            var propiedadesAuditoria = typeof(TAuditoria).GetProperties();

            foreach (var propiedadAuditoria in propiedadesAuditoria)
            {
                var propiedadEntidad = propiedadesEntidad.FirstOrDefault(p =>
                    p.Name == propiedadAuditoria.Name &&
                    p.PropertyType == propiedadAuditoria.PropertyType);

                if (propiedadEntidad != null)
                {
                    propiedadAuditoria.SetValue(auditoria, propiedadEntidad.GetValue(entidad));
                }
            }   

            // Guardar la entrada de auditoría en la base de datos
            _context.Set<TAuditoria>().Add(auditoria);
            await _context.SaveChangesAsync();

        }
    }
}
