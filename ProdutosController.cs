using Microsoft.AspNetCore.Mvc;
using NikeStoreApi.Data;
using NikeStoreApi.Models;

namespace NikeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Produtos.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Create(Produto produto)
        {
            if (produto.Preco <= 0)
                return BadRequest("O preço do produto deve ser maior que zero.");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Produto produtoAtualizado)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            if (produtoAtualizado.Preco <= 0)
                return BadRequest("O preço do produto deve ser maior que zero.");

            produto.Nome = produtoAtualizado.Nome;
            produto.Modelo = produtoAtualizado.Modelo;
            produto.Preco = produtoAtualizado.Preco;
            produto.QuantidadeEstoque = produtoAtualizado.QuantidadeEstoque;

            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return NoContent();
        }
    }
}