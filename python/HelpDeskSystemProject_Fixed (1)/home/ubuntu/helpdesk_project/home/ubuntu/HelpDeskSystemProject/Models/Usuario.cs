namespace HelpDeskSystemFixed.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public string SenhaHash { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
    }
    
    public enum TipoUsuario
    {
        Colaborador = 1,
        Tecnico = 2,
        Administrador = 3
    }
}
