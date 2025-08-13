using Microsoft.AspNetCore.Http;
using SistemaBancario.Api.Erros;

namespace SistemaBancario.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string mensagem)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var erro = new Erro(mensagem);
            return context.Response.WriteAsJsonAsync(erro);
        }
    }
}
