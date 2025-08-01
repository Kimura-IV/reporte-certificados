using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models.Dtos
{
    public class FiltroReporteDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<int>? Plantilla { get; set; }
        public List<string>? Firmante { get; set; }
        public List<string>? Tipo { get; set; }
        public List<string>? Creador { get; set; }
        public List<bool>? Estado { get; set; }
    }
}
