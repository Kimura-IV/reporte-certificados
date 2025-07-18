namespace certificados.web.Models.DTO
{
    public class AprobarParticipantesDTO
    {
        public int IdGrupo { get; set; }
        public List<string> Aprobados { get; set; }
        public string usuarioActualizacion { get; set; }
    }
}
