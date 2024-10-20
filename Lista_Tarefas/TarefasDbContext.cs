using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lista_Tarefas
{
    public class TarefasDbContext : DbContext
    {
        public TarefasDbContext(DbContextOptions<TarefasDbContext> options) : base(options)
        {
        }

        // Representa a tabela 'Tarefas' no banco de dados
        public DbSet<Tarefas> Tarefas { get; set; }
    }
}
