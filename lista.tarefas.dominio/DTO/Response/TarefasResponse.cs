namespace lista.tarefas.dominio.DTO.Response
{
    public class TarefasResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataConclusao { get; set; }
        public bool Status { get; set; }
    }
}
