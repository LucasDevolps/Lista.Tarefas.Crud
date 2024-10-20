using lista.tarefas.dominio.DTO;
using Microsoft.EntityFrameworkCore;

public class TarefasContext : DbContext
{
    public TarefasContext(DbContextOptions<TarefasContext> options) : base(options)
    {
    }
    public DbSet<TarefasDTO> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações adicionais, se necessário.
    }
}
