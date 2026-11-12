
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly PedidosContext _context;

        public ClientesController(PedidosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post(Cliente cliente)
        {
            if (await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula))
                return BadRequest("La cédula ya está registrada.");

            cliente.IdCliente = 0;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente) return BadRequest();

            var actual = await _context.Clientes.FindAsync(id);
            if (actual == null) return NotFound();

            if (await _context.Clientes.AnyAsync(c =>
                c.Cedula == cliente.Cedula && c.IdCliente != id))
                return BadRequest("La cédula ya está registrada.");

            actual.Nombres = cliente.Nombres;
            actual.Cedula = cliente.Cedula;
            actual.Correo = cliente.Correo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            if (await _context.Pedidos.AnyAsync(p => p.IdCliente == id))
                return Conflict("El cliente tiene pedidos asociados.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
