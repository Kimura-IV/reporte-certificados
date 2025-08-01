using certificados.models.Entitys.dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models.Dtos
{
    public class ReporteCertificadoDto
    {                
        public bool? Estado { get; set; }
        public DateTime? FCreacion {get;set;}
        public DateTime? FModificacion {get;set;}
        public int IdCertificado {get;set;}
        public int IdFormato {get;set;}
        public TformatoCertificado? TformatoCertificado {get;set;}
        public string? Tipo {get;set;}
        public string? Titulo {get;set;}
        public string? UsuarioActualizacion {get;set;}
        public string? UsuarioIngreso {get;set;}
        public string? NombreFirmanteUno {get;set;}
        public string? NombreFirmanteDos {get;set;}
        public string? NombreFirmanteTres {get;set;}
        public string? pdfBase64 { get;set; }
    }
}
