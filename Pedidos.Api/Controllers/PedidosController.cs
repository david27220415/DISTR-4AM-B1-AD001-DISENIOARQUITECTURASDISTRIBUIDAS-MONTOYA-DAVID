
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PedidosContext _context;

        public PedidosController(PedidosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> Get()
        {
            return await _context.Pedidos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> Get(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();
            return pedido;
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> Post(Pedido pedido)
        {
            if (!await _context.Clientes.AnyAsync(
                c => c.IdCliente == pedido.IdCliente))
                return BadRequest("El cliente no existe.");

            pedido.IdPedido = 0;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = pedido.IdPedido }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Pedido pedido)
        {
            if (id != pedido.IdPedido) return BadRequest();

            var actual = await _context.Pedidos.FindAsync(id);
            if (actual == null) return NotFound();

            if (!await _context.Clientes.AnyAsync(
                c => c.IdCliente == pedido.IdCliente))
                return BadRequest("El cliente no existe.");

            actual.IdCliente = pedido.IdCliente;
            actual.Fecha = pedido.Fecha;
            actual.Total = pedido.Total;
            actual.Estado = pedido.Estado;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            if (await _context.DetallesPedido.AnyAsync(d => d.IdPedido == id))
                return Conflict("El pedido tiene detalles asociados.");

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
