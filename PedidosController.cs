using Microsoft.AspNetCore.Mvc;
using NikeStoreApi.Data;
using NikeStoreApi.DTOs;
using NikeStoreApi.Models;

namespace NikeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Pedidos.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pedido = _context.Pedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            return Ok(pedido);
        }

        [HttpGet("detalhados")]
        public IActionResult GetDetalhados()
        {
            var pedidosDetalhados = _context.Pedidos
                .Select(p => new PedidoDetalhadoDto
                {
                    Id = p.Id,
                    ProdutoId = p.ProdutoId,
                    ClienteId = p.ClienteId,
                    DataPedido = p.DataPedido,
                    QuantidadeItens = p.QuantidadeItens,
                    NomeProduto = _context.Produtos
                        .Where(prod => prod.Id == p.ProdutoId)
                        .Select(prod => prod.Nome)
                        .FirstOrDefault() ?? "Produto não encontrado",
                    NomeCliente = _context.Clientes
                        .Where(cli => cli.Id == p.ClienteId)
                        .Select(cli => cli.NomeCompleto)
                        .FirstOrDefault() ?? "Cliente não encontrado"
                })
                .ToList();

            return Ok(pedidosDetalhados);
        }

        [HttpPost]
        public IActionResult Create(Pedido pedido)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == pedido.ProdutoId);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == pedido.ClienteId);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            if (pedido.QuantidadeItens <= 0)
                return BadRequest("A quantidade de itens deve ser maior que zero.");

            if (produto.QuantidadeEstoque < pedido.QuantidadeItens)
                return Conflict("Estoque insuficiente para este modelo.");

            produto.QuantidadeEstoque -= pedido.QuantidadeItens;
            pedido.DataPedido = DateTime.Now;

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            if (produto.Nome.Contains("Air Jordan", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Alerta de Hype: Um Air Jordan acaba de ser vendido!");
            }

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }
    }
}