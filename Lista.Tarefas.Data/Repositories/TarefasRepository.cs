using lista.tarefas.dominio.DTO;
using lista.tarefas.dominio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lista.Tarefas.Data.Repositories
{
    public class TarefasRepository : ITarefasRepository
    {
        private readonly TarefasContext _context;

        public TarefasRepository(TarefasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TarefasDTO>> ObterTodasTarefasAsync()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<TarefasDTO> ObterTarefaPorIdAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task AdicionarTarefaAsync(TarefasDTO tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarTarefaAsync(TarefasDTO tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverTarefaAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
