using Microsoft.AspNetCore.Mvc;
using NikeStoreApi.Data;

namespace NikeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("balanco")]
        public IActionResult GetBalanco()
        {
            var faturamentoTotal = _context.Pedidos
                .ToList()
                .Sum(p =>
                {
                    var produto = _context.Produtos.FirstOrDefault(prod => prod.Id == p.ProdutoId);
                    return produto != null ? p.QuantidadeItens * produto.Preco : 0;
                });

            var giroDeEstoque = _context.Produtos.Count(p => p.QuantidadeEstoque == 0);

            return Ok(new
            {
                FaturamentoTotal = faturamentoTotal,
                GiroDeEstoque = giroDeEstoque
            });
        }
    }
}