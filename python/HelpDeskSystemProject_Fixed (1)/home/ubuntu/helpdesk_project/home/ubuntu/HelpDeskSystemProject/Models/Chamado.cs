namespace HelpDeskSystemFixed.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public int IdColaborador { get; set; }
        public int? IdTecnico { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public StatusChamado Status { get; set; } = StatusChamado.Pendente;
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        
        // Propriedades de navegação
        public Usuario? Colaborador { get; set; }
        public Usuario? Tecnico { get; set; }
    }
    
    public enum StatusChamado
    {
        Pendente = 1,
        EmAndamento = 2,
        Concluido = 3
    }
}
