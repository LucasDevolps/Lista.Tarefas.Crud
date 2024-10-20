# Lista de Tarefas - .NET 8 Web API

Este projeto é uma aplicação ASP.NET Core Web API desenvolvida com .NET 8. Ele permite o gerenciamento de uma lista de tarefas, fornecendo operações CRUD (Criar, Ler, Atualizar, Deletar) e integrações com o RabbitMQ para envio de mensagens sobre eventos importantes das tarefas (criação, edição e exclusão).

## Funcionalidades

- **CRUD de Tarefas**: Criação, leitura, atualização e exclusão de tarefas.
- **Integração com RabbitMQ**: Mensagens são enviadas para uma fila RabbitMQ sempre que uma tarefa é criada, editada ou deletada.
- **Validação de Data**: Valida se a data de conclusão de uma tarefa é maior que a data atual no momento da criação.
- **Exibição da Data Atual**: Ao criar uma tarefa, a data atual é exibida como readonly.
- **Log de Eventos com Serilog**: Todas as operações importantes da aplicação são logadas.
- **Conexão com Banco de Dados SQL Server**: A aplicação está configurada para conectar-se a um banco de dados SQL Server.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [RabbitMQ](https://www.rabbitmq.com/) (instalado localmente)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

## Instalação

1. Clone o repositório para sua máquina local:

   git clone https://github.com/seu-repositorio/lista-tarefas.git 

2. Entre no diretório do projeto:
	
	cd lista-tarefas

3. Instale as dependências de pacotes NuGet:

	dotnet restore

4. Configure o RabbitMQ:

	Instale o RabbitMQ localmente em sua máquina. Certifique-se de que ele está rodando na porta padrão (5672).
	
	Para verificar se o RabbitMQ está funcionando, acesse o painel de administração em http://localhost:15672 (usuário: guest, senha: guest).
	
5. Configure o banco de dados SQL Server:

	No arquivo appsettings.json, configure a string de conexão para o seu SQL Server local:
		
6. Rode as migrações ou configure o banco de dados conforme sua necessidade para garantir que a tabela de tarefas seja criada.


### Configuração
1. Conexão com o Banco de Dados

	No arquivo appsettings.json, configure a string de conexão para o SQL Server conforme explicado acima.

2. Configuração do RabbitMQ

	No arquivo Program.cs, configure o RabbitMQProducer:
	
	var builder = WebApplication.CreateBuilder(args);

	// Configuração do RabbitMQProducer
	builder.Services.AddSingleton<RabbitMQProducer>(sp => 
		new RabbitMQProducer("localhost", "filaTarefas", sp.GetRequiredService<ILogger<RabbitMQProducer>>()));

	var app = builder.Build();

	app.UseRouting();
	app.MapControllers();
	app.Run();

### Uso
Inicializar a Aplicação
Para rodar a aplicação, execute:

	dotnet run

A aplicação estará disponível em http://localhost:5000.

### Funcionalidades Principais
1. Listar Tarefas
	Acesse /Tarefas para visualizar a lista de tarefas criadas.

2. Criar Tarefa
	Acesse /Tarefas/Criar para criar uma nova tarefa. A data de conclusão da tarefa deve ser maior que a data atual.

3. Editar Tarefa
	Acesse /Tarefas/Editar/{id} para editar uma tarefa existente.

4. Deletar Tarefa
	Acesse /Tarefas/Deletar/{id} para deletar uma tarefa.

### RabbitMQ
	O sistema envia mensagens para o RabbitMQ nos seguintes eventos:


	Criação de Tarefa: Envia uma mensagem com os detalhes da tarefa criada.
	Edição de Tarefa: Envia uma mensagem com as alterações feitas na tarefa.
	Exclusão de Tarefa: Envia uma mensagem ao excluir uma tarefa.
Exemplo de RabbitMQProducer
	using RabbitMQ.Client;
	using System.Text;
	using Microsoft.Extensions.Logging;

	namespace Lista.Tarefas.Queue
	{
		public class RabbitMQProducer
		{
			private readonly string _hostname;
			private readonly string _queueName;
			private readonly ILogger<RabbitMQProducer> _logger;

			public RabbitMQProducer(string hostname, string queueName, ILogger<RabbitMQProducer> logger)
			{
				_hostname = hostname;
				_queueName = queueName;
				_logger = logger;
			}

			public void SendMessage(string message)
			{
				var factory = new ConnectionFactory() { HostName = _hostname };

				try
				{
					using (var connection = factory.CreateConnection())
					using (var channel = connection.CreateModel())
					{
						channel.QueueDeclare(queue: _queueName,
											 durable: false,
											 exclusive: false,
											 autoDelete: false,
											 arguments: null);

						var body = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish(exchange: "",
											 routingKey: _queueName,
											 basicProperties: null,
											 body: body);

						_logger.LogInformation($"[x] Enviado: {message}");
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Erro ao tentar enviar mensagem para o RabbitMQ: {ex.Message}");
				}
			}
		}
	}
	
### Estrutura do Projeto
	O projeto é organizado em várias class libraries para uma arquitetura limpa:

	Lista.Tarefas.WebAPI: Contém a aplicação Web API.
	Lista.Tarefas.Dominio: Contém os objetos de transferência de dados (DTOs) e interfaces.
	Lista.Tarefas.Queue: Contém a implementação do RabbitMQProducer e RabbitMQConsumer.
	Lista.Tarefas.Queue
	A class library Lista.Tarefas.Queue implementa a comunicação com o RabbitMQ.

### Lista.Tarefas.Dominio
	Contém as classes de DTO para requisição e resposta.

### Exemplo de TarefasRequest
namespace Lista.Tarefas.Dominio.DTO.Request
{
    public class TarefasRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool Status { get; set; }
    }
}
### Exemplo de TarefasResponse
namespace Lista.Tarefas.Dominio.DTO.Response
{
    public class TarefasResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool Status { get; set; }
    }
}
### Lista.Tarefas.WebAPI
	Esta biblioteca contém o TarefasController, que gerencia todas as operações de tarefas.

	Exemplo de TarefasController
	
	
	using Lista.Tarefas.Dominio.DTO.Request;
	using Lista.Tarefas.Dominio.DTO.Response;
	using Lista.Tarefas.Dominio.Interfaces;
	using Lista.Tarefas.Queue;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using System.Threading.Tasks;

	namespace Lista.Tarefas.WebAPI.Controllers
	{
		[Route("Tarefas")]
		public class TarefasController : Controller
		{
			private readonly ITarefasService _tarefasService;
			private readonly ILogger<TarefasController> _logger;
			private readonly RabbitMQProducer _rabbitMQProducer;

			public TarefasController(ITarefasService tarefasService, ILogger<TarefasController> logger, RabbitMQProducer rabbitMQProducer)
			{
				_tarefasService = tarefasService;
				_logger = logger;
				_rabbitMQProducer = rabbitMQProducer;
			}

			[HttpPost("Criar")]
			public async Task<IActionResult> Criar(TarefasRequest request)
			{
				// Lógica para criar tarefa e enviar mensagem ao RabbitMQ...
			}

			// Outros métodos para Editar e Deletar tarefas...
		}
	}
### Licença
	Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.


### Explicações e Ajustes:

- **Instalação do RabbitMQ**: Instruções para garantir que o RabbitMQ está rodando localmente sem mencionar Docker.
- **Configuração SQL Server**: Instruções claras para configurar a conexão com o banco de dados SQL Server.
- **RabbitMQ Producer**: Código de exemplo para o RabbitMQProducer.
- **Estrutura do Projeto**: Explicação sobre como o projeto está organizado em diferentes class libraries.

Se precisar de mais ajustes, fique à vontade para pedir!

