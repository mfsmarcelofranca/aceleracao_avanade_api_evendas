using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_sis_evenda.estoque.Models;
using api_sis_evenda.Estoque.ViewModels;
using api_sis_evenda.Estoque.ViewModels.Filters;
using api_sis_evenda.vendas.Models;
using api_sis_evenda.Vendas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api_sis_evenda.vendas.Controllers
{
    [Route("api/v1/evendas/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly ContextVendas _context;
        private readonly IProdutoVendasService _produtoVendasService;

        public VendasController(ContextVendas context, IProdutoVendasService produtoVendasService)
        {
            _context = context;
            _produtoVendasService = produtoVendasService;
        }

        /// <summary>
        /// Serviço para consultar o cadastro de produtos do módulo de vendas
        /// </summary>
        /// <returns>Retorna status 200 OK em caso de sucesso.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        /// <summary>
        /// Serviço para realizar a venda de produtos do módulo de vendas
        /// </summary>
        /// <param name="produtoViewModel"> View model do method PUT</param>
        /// <param name="id">campo id da view model</param>
        /// <returns>Retorna status 200 OK em caso de sucesso.</returns>
        [SwaggerResponse(statusCode: 200, description: "Venda realizada com sucesso!", Type = typeof(Produto))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios!", Type = typeof(ValidaCamposViewModel))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno!", Type = typeof(ErroGenericoViewModel))]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoVendasViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) { return BadRequest(); }

            List<string> listErros = await _produtoVendasService.validarCamposAsync(produtoViewModel);
            if (listErros.Any()) { return BadRequest(listErros); }

            try
            {
                await _produtoVendasService.UpdateAsync(_produtoVendasService.mapperViewProdutoToProduto(produtoViewModel));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _produtoVendasService.ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro de acesso ao banco de dados!");
                }
            }

            return Ok(produtoViewModel);
        }    
    }
}
