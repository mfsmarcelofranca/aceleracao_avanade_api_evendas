using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_sis_evenda.estoque.Models;
using Swashbuckle.AspNetCore.Annotations;
using api_sis_evenda.Estoque.Services;
using Microsoft.AspNetCore.Http;
using api_sis_evenda.Estoque.ViewModels;
using System.Linq;
using api_sis_evenda.Estoque.ViewModels.Filters;

namespace api_sis_evenda.estoque.Controllers
{
    [Route("api/v1/evendas/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        /// <summary>
        /// Serviço para consultar o cadastro de produtos
        /// </summary>
        /// <returns>Retorna status 200 OK em caso de sucesso.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetProdutos()
        {
           return await _produtoService.GetAllProdutosAsync();
        }

        /// <summary>
        /// Serviço para consultar o cadastro de produtos pelo campo Id da view model
        /// </summary>
        /// <param name="id">campo id da view model</param>
        /// <returns>Retorna status 200 OK em caso de sucesso.</returns>
        /// <response code="404">Produto não encontrado!</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _produtoService.GetProdutoAsyncById(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        /// <summary>
        /// Serviço para atualizar o cadastro de produtos pelo campo Id da view model
        /// </summary>
        /// <param name="produtoViewModel"> View model do method PUT</param>
        /// <param name="id">campo id da view model</param>
        /// <returns>Retorna status 200 OK em caso de sucesso.</returns>
        [SwaggerResponse(statusCode: 200, description: "Cadastro atualizado com sucesso!", Type = typeof(Produto))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios!", Type = typeof(ValidaCamposViewModel))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno!", Type = typeof(ErroGenericoViewModel))]
        [ValidarModelState]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoVendasViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) { return BadRequest(); }

            List<string> listErros = await _produtoService.validarCamposAsync(produtoViewModel, id);
            if (listErros.Any()) { return BadRequest(listErros); }

            try
            {
               await _produtoService.UpdateAsync(_produtoService.mapperViewProdutoToProduto(produtoViewModel));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _produtoService.ProdutoExists(id))
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


        /// <summary>
        /// Serviço para realizar o cadastro de produtos 
        /// </summary>
        /// <param name="produtoViewModel">View model do method POST</param>
        /// <returns>Retorna status 201 created em caso de sucesso.</returns>
        [SwaggerResponse(statusCode: 201, description: "Cadastro realizado com sucesso!", Type = typeof(Produto))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios!", Type = typeof(ValidaCamposViewModel))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno!", Type = typeof(ErroGenericoViewModel))]
        [ValidarModelState]
        [HttpPost]       
        public async Task<ActionResult<Produto>> PostProduto(ProdutoVendasViewModel produtoViewModel)
        {
           List<string> listErros = await _produtoService.validarCamposAsync(produtoViewModel);
           if (listErros.Any())
           {
               return BadRequest(listErros);
           }            

           Produto produtoCriado = await _produtoService.AddAsync(_produtoService.mapperViewProdutoToProduto(produtoViewModel));

           return CreatedAtAction("GetProduto", new { id = produtoCriado.Id }, produtoCriado);
        }
    }
}
