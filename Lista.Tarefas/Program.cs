using Lista.Tarefas.Data.Repositories;
using lista.tarefas.dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lista.Tarefas.Services;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using Lista.Tarefas.Queue;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();

// Configurar a injeção do DbContext
builder.Services.AddDbContext<TarefasContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TarefasConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Número máximo de tentativas
            maxRetryDelay: TimeSpan.FromSeconds(10), // Intervalo entre as tentativas
            errorNumbersToAdd: null // Opcional: erros adicionais que você deseja tratar
        )
    )
);

builder.Services.AddScoped<ITarefasRepository, TarefasRepository>();
builder.Services.AddScoped<ITarefasService, TarefasService>();


// Adicionar o Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona suporte para controllers com views
builder.Services.AddControllersWithViews();

// Configura o Serilog com o Elasticsearch
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) // Altere para a URL do seu Elasticsearch
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-logs-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    })
    .CreateLogger();

// Adiciona o Serilog à aplicação
builder.Host.UseSerilog();

// Adicionando o RabbitMQProducer como Singleton
builder.Services.AddSingleton<RabbitMQProducer>(sp =>
    new RabbitMQProducer("localhost", "filaTarefas", sp.GetRequiredService<ILogger<RabbitMQProducer>>()));


var app = builder.Build();

// Verifica se o banco de dados já existe, e se não, cria-o
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TarefasContext>();
    context.Database.EnsureCreated(); // Garante que o banco de dados será criado se não existir
}

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
