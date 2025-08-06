using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArch.Domain
{
    public interface IContaRepositorio
    {
        Task<IEnumerable<Conta>> GetAllAsync();
        Task<Conta?> GetByNumeroAsync(String numero);
        Task SalvarAsync(Conta conta);
    }
}
