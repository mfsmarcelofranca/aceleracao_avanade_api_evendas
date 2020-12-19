using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.Repository;
using api_sis_evenda.vendas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_sis_evenda.Vendas.Infrastructure.Data.Repository
{
    public class ProdutoVendasRepository : IProdutoVendasRepository
    {
        private readonly ContextVendas _context;

        public ProdutoVendasRepository(ContextVendas context)
        {
            _context = context;
        }

        public void Add(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Update(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
        }

        public async Task<List<Produto>> GetAllProdutosAsync()
        {
            return await _context.Produtos.Where(p => p.Quantidade > 0).ToListAsync();
        }

        public async Task<Produto> GetProdutoAsyncById(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }
        public async Task<bool> ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }     
    }
}
