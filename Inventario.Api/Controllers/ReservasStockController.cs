
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasStockController : ControllerBase
    {
        private readonly InventarioContext _context;

        public ReservasStockController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaStock>>> Get()
        {
            return await _context.ReservasStock.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaStock>> Get(int id)
        {
            var reserva = await _context.ReservasStock.FindAsync(id);
            if (reserva == null) return NotFound();
            return reserva;
        }

        [HttpPost]
        public async Task<ActionResult<ReservaStock>> Post(ReservaStock reserva)
        {
            if (reserva.IdProducto <= 0)
                return BadRequest("IdProducto debe ser positivo.");

            if (!await _context.Bodegas.AnyAsync(
                b => b.IdBodega == reserva.IdBodega))
                return BadRequest("La bodega no existe.");

            reserva.IdReserva = 0;
            _context.ReservasStock.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = reserva.IdReserva }, reserva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ReservaStock reserva)
        {
            if (id != reserva.IdReserva)
                return BadRequest();

            var actual = await _context.ReservasStock.FindAsync(id);
            if (actual == null) return NotFound();

            if (reserva.IdProducto <= 0 ||
                !await _context.Bodegas.AnyAsync(
                    b => b.IdBodega == reserva.IdBodega))
                return BadRequest("Producto o bodega inválidos.");

            actual.IdProducto = reserva.IdProducto;
            actual.IdBodega = reserva.IdBodega;
            actual.Cantidad = reserva.Cantidad;
            actual.Estado = reserva.Estado;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _context.ReservasStock.FindAsync(id);
            if (reserva == null) return NotFound();

            _context.ReservasStock.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
