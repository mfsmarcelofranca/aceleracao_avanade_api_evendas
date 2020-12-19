using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_sis_evenda.Estoque.Infrastructure.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly Context _context;

        public ProdutoRepository(Context context)
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
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> GetProdutoAsyncById(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }       

        public async Task<bool> ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        public async Task<bool> ProdutoExistsByCodigo(string codigo, int? id = null)
        {
            return _context.Produtos.Any(e => (e.Codigo == codigo && e.Id != id));
        }
        public async Task<bool> ProdutoExistsByNome(string nome, int? id = null)
        {
            var teste = _context.Produtos.Any(e => (e.Nome == nome && e.Id != id));
            return teste;
        }
    }
}
