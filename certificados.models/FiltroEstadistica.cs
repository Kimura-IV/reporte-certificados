using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models
{
    public class FiltroEstadistica
    {
        public DateTime? FechaInicio {  get; set; }
        public DateTime? FechaFin {  get; set; }
        public int Plantilla { get; set; }
        public string? Firmante { get; set; }
        public string? Tipo { get; set; }
        public string? Creador { get; set; }
        public bool? Estado { get; set; }
    }
}
