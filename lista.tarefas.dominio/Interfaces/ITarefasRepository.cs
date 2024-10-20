using lista.tarefas.dominio.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lista.tarefas.dominio.Interfaces
{
    public interface ITarefasRepository
    {
        Task<IEnumerable<TarefasDTO>> ObterTodasTarefasAsync();
        Task<TarefasDTO> ObterTarefaPorIdAsync(int id);
        Task AdicionarTarefaAsync(TarefasDTO tarefa);
        Task AtualizarTarefaAsync(TarefasDTO tarefa);
        Task RemoverTarefaAsync(int id);
    }
}
