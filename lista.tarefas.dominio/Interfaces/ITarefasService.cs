using lista.tarefas.dominio.DTO;

namespace lista.tarefas.dominio.Interfaces
{
    public interface ITarefasService
    {
        Task<IEnumerable<TarefasDTO>> ObterTodasTarefasAsync();
        Task<TarefasDTO> ObterTarefaPorIdAsync(int id);
        Task AdicionarTarefaAsync(TarefasDTO tarefa);
        Task AtualizarTarefaAsync(TarefasDTO tarefa);
        Task RemoverTarefaAsync(int id);
    }
}
