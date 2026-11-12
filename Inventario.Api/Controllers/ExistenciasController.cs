
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExistenciasController : ControllerBase
    {
        private readonly InventarioContext _context;

        public ExistenciasController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Existencia>>> Get()
        {
            return await _context.Existencias.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Existencia>> Get(int id)
        {
            var existencia = await _context.Existencias.FindAsync(id);
            if (existencia == null) return NotFound();
            return existencia;
        }

        [HttpPost]
        public async Task<ActionResult<Existencia>> Post(Existencia existencia)
        {
            if (existencia.IdProducto <= 0)
                return BadRequest("IdProducto debe ser positivo.");

            if (!await _context.Bodegas.AnyAsync(
                b => b.IdBodega == existencia.IdBodega))
                return BadRequest("La bodega no existe.");

            if (await _context.Existencias.AnyAsync(e =>
                e.IdProducto == existencia.IdProducto &&
                e.IdBodega == existencia.IdBodega))
                return BadRequest("Ya existe stock para ese producto y bodega.");

            existencia.IdExistencia = 0;
            _context.Existencias.Add(existencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = existencia.IdExistencia }, existencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Existencia existencia)
        {
            if (id != existencia.IdExistencia)
                return BadRequest();

            var actual = await _context.Existencias.FindAsync(id);
            if (actual == null) return NotFound();

            if (existencia.IdProducto <= 0 ||
                !await _context.Bodegas.AnyAsync(
                    b => b.IdBodega == existencia.IdBodega))
                return BadRequest("Producto o bodega inválidos.");

            if (await _context.Existencias.AnyAsync(e =>
                e.IdExistencia != id &&
                e.IdProducto == existencia.IdProducto &&
                e.IdBodega == existencia.IdBodega))
                return BadRequest("Ya existe esa combinación.");

            actual.IdProducto = existencia.IdProducto;
            actual.IdBodega = existencia.IdBodega;
            actual.Stock = existencia.Stock;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existencia = await _context.Existencias.FindAsync(id);
            if (existencia == null) return NotFound();

            _context.Existencias.Remove(existencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
