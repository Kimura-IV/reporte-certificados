namespace certificados.web.Models.DTO
{
    public class GrupoPersonaDTO
    {
        public int IdGrupo { get; set; }
        public List<string> Cedulas { get; set; } = new List<string>();
        public string UsuarioIngreso { get; set; } = string.Empty;
    }
}
