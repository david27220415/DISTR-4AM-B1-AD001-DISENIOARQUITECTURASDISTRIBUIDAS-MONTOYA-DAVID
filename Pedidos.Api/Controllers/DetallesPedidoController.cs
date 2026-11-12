
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly PedidosContext _context;

        public DetallesPedidoController(PedidosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> Get()
        {
            return await _context.DetallesPedido.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> Get(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle == null) return NotFound();
            return detalle;
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>> Post(DetallePedido detalle)
        {
            if (detalle.IdProducto <= 0)
                return BadRequest("IdProducto debe ser positivo.");

            if (!await _context.Pedidos.AnyAsync(
                p => p.IdPedido == detalle.IdPedido))
                return BadRequest("El pedido no existe.");

            detalle.IdDetalle = 0;
            _context.DetallesPedido.Add(detalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = detalle.IdDetalle }, detalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DetallePedido detalle)
        {
            if (id != detalle.IdDetalle) return BadRequest();

            var actual = await _context.DetallesPedido.FindAsync(id);
            if (actual == null) return NotFound();

            if (detalle.IdProducto <= 0 ||
                !await _context.Pedidos.AnyAsync(
                    p => p.IdPedido == detalle.IdPedido))
                return BadRequest("Pedido o producto inválidos.");

            actual.IdPedido = detalle.IdPedido;
            actual.IdProducto = detalle.IdProducto;
            actual.Cantidad = detalle.Cantidad;
            actual.PrecioUnitario = detalle.PrecioUnitario;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle == null) return NotFound();

            _context.DetallesPedido.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
