
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodegasController : ControllerBase
    {
        private readonly InventarioContext _context;

        public BodegasController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bodega>>> Get()
        {
            return await _context.Bodegas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bodega>> Get(int id)
        {
            var bodega = await _context.Bodegas.FindAsync(id);
            if (bodega == null) return NotFound();
            return bodega;
        }

        [HttpPost]
        public async Task<ActionResult<Bodega>> Post(Bodega bodega)
        {
            bodega.IdBodega = 0;
            _context.Bodegas.Add(bodega);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = bodega.IdBodega }, bodega);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Bodega bodega)
        {
            if (id != bodega.IdBodega) return BadRequest();

            var existente = await _context.Bodegas.FindAsync(id);
            if (existente == null) return NotFound();

            existente.Nombre = bodega.Nombre;
            existente.Ciudad = bodega.Ciudad;
            existente.Direccion = bodega.Direccion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bodega = await _context.Bodegas.FindAsync(id);
            if (bodega == null) return NotFound();

            bool enUso =
                await _context.Existencias.AnyAsync(x => x.IdBodega == id) ||
                await _context.MovimientosInventario.AnyAsync(x => x.IdBodega == id) ||
                await _context.ReservasStock.AnyAsync(x => x.IdBodega == id);

            if (enUso)
                return Conflict("La bodega tiene registros asociados.");

            _context.Bodegas.Remove(bodega);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
