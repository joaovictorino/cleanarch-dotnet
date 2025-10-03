using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SistemaBancario.Infraestrutura;
using SistemaBancario.Infraestrutura.Dados;
using SistemaBancario.Infraestrutura.Repositorios;
using SistemaBancario.Dominio.Interfaces;
using SistemaBancario.Aplicacao.Servicos;
using SistemaBancario.Infraestrutura.Mapeamentos;
using SistemaBancario.Aplicacao.Interfaces;
using SistemaBancario.Api.Middlewares;
using SistemaBancario.Dominio.Servicos;

var construtor = WebApplication.CreateBuilder(args);

// Configurar Entity Framework com MySQL
construtor.Services.AddDbContext<ContextoBancario>(opcoes =>
{
    var stringConexao = construtor.Configuration.GetConnectionString("DefaultConnection");
    opcoes.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
});

construtor.Services.AddAutoMapper(typeof(PerfilConta), typeof(PerfilTransacao));

const string nomePoliticaCors = "CorsPolicy";
var origensPermitidas = (construtor.Configuration.GetValue<string>("Cors:AllowedOrigins") ?? string.Empty)
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(origem => origem.Trim())
    .Where(origem => !string.IsNullOrWhiteSpace(origem))
    .ToArray();

if (origensPermitidas.Length == 0)
{
    origensPermitidas = new[] { "http://localhost:3000" };
}

construtor.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy(nomePoliticaCors, politica =>
    {
        politica.WithOrigins(origensPermitidas)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configurar repositórios
construtor.Services.AddScoped<IRepositorioConta, RepositorioConta>();
construtor.Services.AddScoped<IRepositorioTransacao, RepositorioTransacao>();
construtor.Services.AddScoped<IUnidadeTrabalho, UnidadeTrabalho>();
construtor.Services.AddScoped<IMapeamentoConta, MapeamentoConta>();
construtor.Services.AddScoped<IMapeamentoTransacao, MapeamentoTransacao>();

// Configurar serviços
construtor.Services.AddScoped<ITransferirValor, TransferirValor>();
construtor.Services.AddScoped<IServicoConta, ServicoConta>();
construtor.Services.AddScoped<IServicoTransferencia, ServicoTransferencia>();

// Configurar controllers
construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

var app = construtor.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors(nomePoliticaCors);
app.UseAuthorization();
app.MapControllers();

// Criar banco se não existir
using (var escopo = app.Services.CreateScope())
{
    var contexto = escopo.ServiceProvider.GetRequiredService<ContextoBancario>();
    contexto.Database.Migrate();
}

app.Run();
