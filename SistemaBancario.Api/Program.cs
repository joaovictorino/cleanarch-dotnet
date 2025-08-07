using Microsoft.EntityFrameworkCore;
using SistemaBancario.Infraestrutura;
using SistemaBancario.Infraestrutura.Dados;
using SistemaBancario.Infraestrutura.Repositorios;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Aplicacao.Servicos;

var construtor = WebApplication.CreateBuilder(args);

// Configurar Entity Framework com MySQL
construtor.Services.AddDbContext<ContextoBancario>(opcoes =>
{
    var stringConexao = construtor.Configuration.GetConnectionString("DefaultConnection");
    opcoes.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
});

// Configurar repositórios
construtor.Services.AddScoped<IRepositorioConta, RepositorioConta>();
construtor.Services.AddScoped<IRepositorioTransacao, RepositorioTransacao>();
construtor.Services.AddScoped<IUnidadeTrabalho, UnidadeTrabalho>();

// Configurar serviços
construtor.Services.AddScoped<ServicoBancario>();

// Configurar controllers
construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

var app = construtor.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Criar banco se não existir
using (var escopo = app.Services.CreateScope())
{
    var contexto = escopo.ServiceProvider.GetRequiredService<ContextoBancario>();
    contexto.Database.EnsureCreated();
}

app.Run();
