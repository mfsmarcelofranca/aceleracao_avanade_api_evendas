using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_sis_evenda.Estoque.Services
{
    public interface IProdutoService
    {
        Task<Produto> AddAsync(Produto produto);

        Task<bool> UpdateAsync(Produto produto);

        Task<bool> UpdateProdutoVendidoAsync(Produto produtoVendido);

        Task<List<Produto>> GetAllProdutosAsync();

        Task<Produto> GetProdutoAsyncById(int id);

        Task<bool> ProdutoExists(int id);

        Task<bool> ProdutoExistsByCodigo(string codigo, int? id = null);

        Task<bool> ProdutoExistsByNome(string nome, int? id = null);

        Task<List<string>> validarCamposAsync(ProdutoVendasViewModel produtoViewModel, int? id = null);

        public Produto mapperViewProdutoToProduto(ProdutoVendasViewModel produtoViewModel);
    }
}
