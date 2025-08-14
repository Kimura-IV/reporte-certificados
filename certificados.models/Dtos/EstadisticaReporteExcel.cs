using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models.Dtos
{
    public class EstadisticaReporteExcel
    {
        public List<string> Graficos { get; set; }
        public FiltroReporteDto Filtro { get; set; }
        public EstadisticaReporteExcel()
        {
            Graficos = new List<string>();
            Filtro = new FiltroReporteDto();
        }
    }
}
