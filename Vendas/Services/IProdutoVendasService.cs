using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_sis_evenda.Vendas.Services
{
    public interface IProdutoVendasService
    {
        Task<Produto> AddAsync(Produto produto);

        Task<bool> UpdateAsync(Produto produto);

        Task<bool> UpdateProdutoAtualizadoAsync(Produto produtoAtualizado);

        Task<List<Produto>> GetAllProdutosAsync();

        Task<Produto> GetProdutoAsyncById(int id);

        Task<bool> ProdutoExists(int id);

        Task<List<string>> validarCamposAsync(ProdutoVendasViewModel produtoViewModel);

        public Produto mapperViewProdutoToProduto(ProdutoVendasViewModel produtoViewModel);
    }
}
