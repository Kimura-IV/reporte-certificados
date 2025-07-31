using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models
{
    public class EstadisticaModel
    {
        public List<Plantilla> Plantillas { get; set; }
        public List<LapsoDia> LapsoDias { get; set; }
        public List<LapsoSemana> LapsoSemanas { get; set; }
        public List<LapsoAnio> LapsoAnios { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Firmante> Firmantes { get; set; }

        public EstadisticaModel()
        {
            Plantillas = new List<Plantilla>();
            LapsoSemanas = new List<LapsoSemana>();
            LapsoAnios = new List<LapsoAnio>();
            LapsoDias = new List<LapsoDia>();
            Estados = new List<Estado>();
            Firmantes = new List<Firmante>();

        }

    }
    public class Plantilla
    {
        public string? NombrePlantilla {get;set;}
        public int Count {get;set;}
    }
    public class LapsoDia
    {
        public DateTime? Dia { get; set; }
        public int Count { get;set; }
    }

    public class LapsoAnio
    {
        public int Anio { get; set; }
        public int Count { get; set; }
    }
    public class LapsoSemana
    {
        public DateTime? SemanaInicio { get; set; } 
        public int Count { get; set; }
    }
    public class Estado
    {
        public string? Tipo { get; set; }
        public int Count { get; set; }
    }
    public class Firmante
    {
        public string? NombreFirmante { get; set; }
        public int Count { get; set; }

    }
}
