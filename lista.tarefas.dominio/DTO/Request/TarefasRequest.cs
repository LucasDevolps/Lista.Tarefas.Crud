using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lista.tarefas.dominio.DTO.Request
{
    public class TarefasRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataConclusao { get; set; }
        public bool Status { get; set; }
    }
}
