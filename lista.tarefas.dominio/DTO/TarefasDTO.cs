namespace lista.tarefas.dominio.DTO
{
    public class TarefasDTO
    {
        public int Id { get; set; }          // Coluna para o identificador único da tarefa
        public string Nome { get; set; }     // Coluna para o nome da tarefa
        public string Descricao { get; set; } // Coluna para a descrição da tarefa
        public DateTime DataConclusao { get; set; } // Coluna para a data de conclusão da tarefa
        public bool Status { get; set; }     // Coluna para o status da tarefa (concluída ou não)
    }
}
