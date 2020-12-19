using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.Repository;
using api_sis_evenda.Estoque.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_sis_evenda.Estoque.Services
{
    public class ProdutoService : IProdutoService
    {
        IProdutoRepository _produtoRepository;
        IMessagePublisher _messagePublisher;

        public ProdutoService(IProdutoRepository produtoRepository, IMessagePublisher messagePublisher)
        {
            _produtoRepository = produtoRepository;
            _messagePublisher = messagePublisher;
        }
        public async Task<Produto> AddAsync(Produto produto)
        {
            _produtoRepository.Add(produto);
            if (await _produtoRepository.SaveChangesAsync())
            {
                _messagePublisher.setQueueClient("ServiceBus:Estoque:Productor:TopicoProdutoCriado:");
                await _messagePublisher.Publish(produto);
            }          

            return produto;
        }
        public async Task<bool> UpdateAsync(Produto produto)
        {
            _produtoRepository.Update(produto);
            bool produtoFoiAtualizado = await _produtoRepository.SaveChangesAsync();

            if (produtoFoiAtualizado)
            {
                _messagePublisher.setQueueClient("ServiceBus:Estoque:Productor:TopicoProdutoAtualizado:");
                await _messagePublisher.Publish(produto);
            }

            return produtoFoiAtualizado;
        }
        public async Task<bool> UpdateProdutoVendidoAsync(Produto produtoVendido)
        {
            Produto produto = await GetProdutoAsyncById(produtoVendido.Id);
            produto.Quantidade -= produtoVendido.Quantidade;

            return await UpdateAsync(produto);
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
        public async Task<bool> ProdutoExistsByCodigo(string codigo, int? id = null)
        {
            return await _produtoRepository.ProdutoExistsByCodigo(codigo, id);
        }
        public async Task<bool> ProdutoExistsByNome(string nome, int? id = null)
        {
            return await _produtoRepository.ProdutoExistsByNome(nome, id);
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

        public async Task<List<string>> validarCamposAsync(ProdutoVendasViewModel produtoViewModel, int? id = null)
        {
            List<string> erros = new List<string>();
            if (await ProdutoExistsByNome(produtoViewModel.Nome, id))
            {
                erros.Add("Este Nome já foi cadastrato!");
            }
            if (await ProdutoExistsByCodigo(produtoViewModel.Codigo, id))
            {
                erros.Add("Este Codigo já foi cadastrato!");
            }
            return erros;
        }

    }
}
