using lista.tarefas.dominio.DTO.Request;
using lista.tarefas.dominio.DTO.Response;
using lista.tarefas.dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using lista.tarefas.dominio.DTO;

namespace lista.tarefas.Controllers
{
    [Route("Tarefas")]
    public class TarefasController : Controller
    {
        private readonly ITarefasService _tarefasService;

        public TarefasController(ITarefasService tarefasService)
        {
            _tarefasService = tarefasService;
        }

        // Exibe a lista de tarefas e retorna uma lista de TarefasResponse
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tarefas = await _tarefasService.ObterTodasTarefasAsync();

            var response = tarefas.Select(t => new TarefasResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                DataConclusao = t.DataConclusao,
                Status = t.Status
            });

            return View(response);
        }

        // Método GET para exibir o formulário de criação
        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            return View();  // Certifique-se de que a View "Criar.cshtml" existe
        }

        [HttpPost("Criar")]
        public async Task<IActionResult> Criar(TarefasRequest request)
        {
            // Validação para verificar se a data de conclusão é maior que a data atual
            if (request.DataConclusao <= DateTime.Now)
            {
                ModelState.AddModelError("DataConclusao", "A data de conclusão deve ser maior que a data atual.");
            }

            if (ModelState.IsValid)
            {
                var tarefa = new TarefasDTO
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    DataConclusao = request.DataConclusao,
                    Status = request.Status
                };

                await _tarefasService.AdicionarTarefaAsync(tarefa);

                return RedirectToAction("Index");
            }

            return View(request);
        }


        // Exibe o formulário de edição de uma tarefa
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var tarefa = await _tarefasService.ObterTarefaPorIdAsync(id);

            if (tarefa == null)
                return NotFound();

            var request = new TarefasRequest
            {
                Nome = tarefa.Nome,
                Descricao = tarefa.Descricao,
                DataConclusao = tarefa.DataConclusao,
                Status = tarefa.Status
            };

            return View(request);
        }

        // Atualiza a tarefa recebendo um TarefasRequest
        [HttpPost("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, TarefasRequest request)
        {
            if (ModelState.IsValid)
            {
                var tarefa = new TarefasDTO
                {
                    Id = id, // Mantenha o ID da tarefa
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    DataConclusao = request.DataConclusao,
                    Status = request.Status
                };

                await _tarefasService.AtualizarTarefaAsync(tarefa);

                return RedirectToAction("Index");
            }

            return View(request);
        }
    }
}
