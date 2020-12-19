using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.Services;
using api_sis_evenda.Estoque.ViewModels;
using api_sis_evenda.Vendas.Infrastructure.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_sis_evenda.Vendas.Services
{
    public class ProdutoVendasService : IProdutoVendasService
    {
        IProdutoVendasRepository _produtoRepository;
        IMessagePublisher _messagePublisher;

        public ProdutoVendasService(IProdutoVendasRepository produtoRepository, IMessagePublisher messagePublisher)
        {
            _produtoRepository = produtoRepository;
            _messagePublisher = messagePublisher;
        }
        public async Task<Produto> AddAsync(Produto produto)
        {
            _produtoRepository.Add(produto);
            await _produtoRepository.SaveChangesAsync();

            return produto;
        }
        public async Task<bool> UpdateAsync(Produto produtoVendido)
        {
            Produto produto = await GetProdutoAsyncById(produtoVendido.Id);
            produto.Quantidade -= produtoVendido.Quantidade;
            _produtoRepository.Update(produto);
            bool produtoFoiAtualizado = await _produtoRepository.SaveChangesAsync();

            if (produtoFoiAtualizado)
            {
                _messagePublisher.setQueueClient("ServiceBus:Vendas:Productor:TopicoProdutoVendido:");
                await _messagePublisher.Publish(produtoVendido);
            }

            return produtoFoiAtualizado;
        }
        public async Task<bool> UpdateProdutoAtualizadoAsync(Produto produtoAtualizado)
        {
            _produtoRepository.Update(produtoAtualizado);
            return await _produtoRepository.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetAllProdutosAsync()
        {
            return await _produtoRepository.GetAllProdutosAsync();
        }

        public async Task<Produto> GetProdutoAsyncById(int id)
        {
            return await _produtoRepository.GetProdutoAsyncById(id);
        }
        public async Task<bool> ProdutoExists(int id)
        {
            return await _produtoRepository.ProdutoExists(id);
        }

        public Produto mapperViewProdutoToProduto(ProdutoVendasViewModel produtoViewModel)
        {
            Produto produto = new Produto();
            produto.Id = produtoViewModel.Id;
            produto.Nome = produtoViewModel.Nome;
            produto.Codigo = produtoViewModel.Codigo;
            produto.Preco = produtoViewModel.Preco;
            produto.Quantidade = produtoViewModel.Quantidade;

            return produto;
        }

        public async Task<List<string>> validarCamposAsync(ProdutoVendasViewModel produtoViewModel)
        {
            Produto produto = await GetProdutoAsyncById(produtoViewModel.Id);
            List<string> erros = new List<string>();
            if (produto.Codigo != produtoViewModel.Codigo)
            {
                erros.Add("O código do produto não confere com o informado!");
            }
            if (produto.Nome != produtoViewModel.Nome)
            {
                erros.Add("O nome do produto não confere com o informado!");
            }
            if (produto.Preco != produtoViewModel.Preco)
            {
                erros.Add("O preço do produto não confere com o informado!");
            }
            if (produto.Quantidade < produtoViewModel.Quantidade)
            {
                erros.Add("Não temos a quantidade solicitada em estoque!");
            }
            return erros;
        }

    }
}
