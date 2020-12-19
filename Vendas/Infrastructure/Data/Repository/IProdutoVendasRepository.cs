using api_sis_evenda.estoque.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_sis_evenda.Vendas.Infrastructure.Data.Repository
{
    public interface IProdutoVendasRepository
    {
        void Add(Produto produto);

        void Update(Produto produto);

        Task<bool> SaveChangesAsync();

        Task<List<Produto>> GetAllProdutosAsync();

        Task<Produto> GetProdutoAsyncById(int id);

        Task<bool> ProdutoExists(int id);
    }
}