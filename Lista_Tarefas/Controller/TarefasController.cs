using Microsoft.AspNetCore.Mvc;

namespace Lista_Tarefas.Controller
{
    public class TarefasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
