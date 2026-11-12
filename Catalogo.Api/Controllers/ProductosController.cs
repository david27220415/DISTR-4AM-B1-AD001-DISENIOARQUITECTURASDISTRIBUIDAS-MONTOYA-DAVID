
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Data;
using Catalogo.Api.Models;

namespace Catalogo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly CatalogoContext _context;

        public ProductosController(CatalogoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            if (!await _context.Categorias.AnyAsync(c =>
                c.IdCategoria == producto.IdCategoria))
                return BadRequest("La categoría no existe.");

            if (!await _context.Marcas.AnyAsync(m =>
                m.IdMarca == producto.IdMarca))
                return BadRequest("La marca no existe.");

            if (await _context.Productos.AnyAsync(p =>
                p.SKU == producto.SKU))
                return BadRequest("El SKU ya existe.");

            producto.IdProducto = 0;
            producto.Categoria = null;
            producto.Marca = null;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = producto.IdProducto }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Producto producto)
        {
            if (id != producto.IdProducto)
                return BadRequest();

            var existente = await _context.Productos.FindAsync(id);

            if (existente == null)
                return NotFound();

            if (!await _context.Categorias.AnyAsync(c =>
                c.IdCategoria == producto.IdCategoria))
                return BadRequest("La categoría no existe.");

            if (!await _context.Marcas.AnyAsync(m =>
                m.IdMarca == producto.IdMarca))
                return BadRequest("La marca no existe.");

            if (await _context.Productos.AnyAsync(p =>
                p.SKU == producto.SKU && p.IdProducto != id))
                return BadRequest("El SKU ya existe.");

            existente.IdCategoria = producto.IdCategoria;
            existente.IdMarca = producto.IdMarca;
            existente.Nombre = producto.Nombre;
            existente.SKU = producto.SKU;
            existente.Precio = producto.Precio;
            existente.Estado = producto.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
