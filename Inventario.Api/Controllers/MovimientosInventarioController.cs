
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosInventarioController : ControllerBase
    {
        private readonly InventarioContext _context;

        public MovimientosInventarioController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoInventario>>> Get()
        {
            return await _context.MovimientosInventario.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoInventario>> Get(int id)
        {
            var movimiento = await _context.MovimientosInventario.FindAsync(id);
            if (movimiento == null) return NotFound();
            return movimiento;
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoInventario>> Post(
            MovimientoInventario movimiento)
        {
            if (movimiento.IdProducto <= 0)
                return BadRequest("IdProducto debe ser positivo.");

            if (!await _context.Bodegas.AnyAsync(
                b => b.IdBodega == movimiento.IdBodega))
                return BadRequest("La bodega no existe.");

            movimiento.IdMovimiento = 0;
            _context.MovimientosInventario.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = movimiento.IdMovimiento }, movimiento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id, MovimientoInventario movimiento)
        {
            if (id != movimiento.IdMovimiento)
                return BadRequest();

            var actual = await _context.MovimientosInventario.FindAsync(id);
            if (actual == null) return NotFound();

            if (movimiento.IdProducto <= 0 ||
                !await _context.Bodegas.AnyAsync(
                    b => b.IdBodega == movimiento.IdBodega))
                return BadRequest("Producto o bodega inválidos.");

            actual.IdProducto = movimiento.IdProducto;
            actual.IdBodega = movimiento.IdBodega;
            actual.Tipo = movimiento.Tipo;
            actual.Cantidad = movimiento.Cantidad;
            actual.Fecha = movimiento.Fecha;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movimiento = await _context.MovimientosInventario.FindAsync(id);
            if (movimiento == null) return NotFound();

            _context.MovimientosInventario.Remove(movimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
