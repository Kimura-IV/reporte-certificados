using certificados.models.Entitys.dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models
{
    public class FiltroCertificado
    {
        public List<Tpersona> Personas { get; set; }
        public List<TformatoCertificado> Plantillas { get; set; }
        public List<string> Tipos { get; set; }
        public List<string> Firmantes { get; set; }
        public FiltroCertificado()
        {
            Personas = new List<Tpersona>();
            Plantillas = new List<TformatoCertificado> ();
            Tipos = new List<string> ();
            Firmantes = new List<string> ();
        }
    }
}
