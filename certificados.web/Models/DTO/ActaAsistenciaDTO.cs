using certificados.models.Entitys.dbo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace certificados.web.Models.DTO
{
    public class ActaAsistenciaDTO
    {
        public int IdAsistencia { get; set; }

        public int IdEvento { get; set; }

        public byte[]? ActaDocumento { get; set; }


        public string? UsuarioIngreso { get; set; }

        public string? UsuarioActualizacion { get; set; }

    }
}
