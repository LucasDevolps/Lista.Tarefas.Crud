using lista.tarefas.dominio.DTO;
using lista.tarefas.dominio.Interfaces;

namespace Lista.Tarefas.Services
{
    public class TarefasService : ITarefasService
    {
        private readonly ITarefasRepository _tarefasRepository;

        public TarefasService(ITarefasRepository tarefasRepository)
        {
            _tarefasRepository = tarefasRepository;
        }

        public async Task<IEnumerable<TarefasDTO>> ObterTodasTarefasAsync()
        {
            return await _tarefasRepository.ObterTodasTarefasAsync();
        }

        public async Task<TarefasDTO> ObterTarefaPorIdAsync(int id)
        {
            return await _tarefasRepository.ObterTarefaPorIdAsync(id);
        }

        public async Task AdicionarTarefaAsync(TarefasDTO tarefa)
        {
            await _tarefasRepository.AdicionarTarefaAsync(tarefa);
        }

        public async Task AtualizarTarefaAsync(TarefasDTO tarefa)
        {
            await _tarefasRepository.AtualizarTarefaAsync(tarefa);
        }

        public async Task RemoverTarefaAsync(int id)
        {
            await _tarefasRepository.RemoverTarefaAsync(id);
        }
    }

}
