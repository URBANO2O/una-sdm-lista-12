using Microsoft.AspNetCore.Mvc;
using NikeStoreApi.Data;
using NikeStoreApi.Models;

namespace NikeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Clientes.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Cliente clienteAtualizado)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            cliente.NomeCompleto = clienteAtualizado.NomeCompleto;
            cliente.Email = clienteAtualizado.Email;
            cliente.CPF = clienteAtualizado.CPF;

            _context.SaveChanges();

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return NoContent();
        }
    }
}