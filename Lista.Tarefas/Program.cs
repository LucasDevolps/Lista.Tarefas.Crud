using Lista.Tarefas.Data.Repositories;
using lista.tarefas.dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lista.Tarefas.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner
builder.Services.AddControllers();

// Configurar a inje��o do DbContext
builder.Services.AddDbContext<TarefasContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TarefasConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // N�mero m�ximo de tentativas
            maxRetryDelay: TimeSpan.FromSeconds(10), // Intervalo entre as tentativas
            errorNumbersToAdd: null // Opcional: erros adicionais que voc� deseja tratar
        )
    )
);

builder.Services.AddScoped<ITarefasRepository, TarefasRepository>();
builder.Services.AddScoped<ITarefasService, TarefasService>();


// Adicionar o Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona suporte para controllers com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Verifica se o banco de dados j� existe, e se n�o, cria-o
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TarefasContext>();
    context.Database.EnsureCreated(); // Garante que o banco de dados ser� criado se n�o existir
}

// Configurar o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
