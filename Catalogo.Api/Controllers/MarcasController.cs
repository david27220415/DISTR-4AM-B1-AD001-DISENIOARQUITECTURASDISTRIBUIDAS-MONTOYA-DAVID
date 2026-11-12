
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Data;
using Catalogo.Api.Models;

namespace Catalogo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly CatalogoContext _context;

        public MarcasController(CatalogoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> Get()
        {
            return await _context.Marcas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> Get(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca == null)
                return NotFound();

            return marca;
        }

        [HttpPost]
        public async Task<ActionResult<Marca>> Post(Marca marca)
        {
            marca.IdMarca = 0;

            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = marca.IdMarca }, marca);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Marca marca)
        {
            if (id != marca.IdMarca)
                return BadRequest();

            if (!await _context.Marcas.AnyAsync(m => m.IdMarca == id))
                return NotFound();

            _context.Entry(marca).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca == null)
                return NotFound();

            if (await _context.Productos.AnyAsync(p => p.IdMarca == id))
                return Conflict("La marca tiene productos asociados.");

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
