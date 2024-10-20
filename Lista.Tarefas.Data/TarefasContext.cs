using Microsoft.EntityFrameworkCore;
using lista.tarefas.dominio.DTO;

namespace lista.tarefas.dominio.Data
{
    public class TarefasContext : DbContext
    {
        public TarefasContext(DbContextOptions<TarefasContext> options) : base(options)
        {
        }

        public DbSet<TarefasDTO> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aqui você pode adicionar configurações adicionais, se necessário
        }
    }
}
