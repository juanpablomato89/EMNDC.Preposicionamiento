using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Dto;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly PreposicionamientoDbContext _context;

        public ProductosController(PreposicionamientoDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos?pageIndex=0&pageSize=10&search=&provinciaId=&municipioId=&organismoId=
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductoDto>>> GetProductos(
            int pageIndex = 0,
            int pageSize = 10,
            string? search = null,
            int? provinciaId = null,
            int? municipioId = null,
            int? organismoId = null,
            string? orderBy = "Descripcion",
            bool ascending = true)
        {
            var isAdmin = User.IsAdmin();
            var userOrganismoId = User.GetUserOrganismoIdFromToken();

            if (!isAdmin && userOrganismoId == null)
                return Forbid();

            var query = _context.Productos
                .Include(p => p.Organismo)
                .Include(p => p.Stocks)
                    .ThenInclude(s => s.Almacen)
                    .ThenInclude(a => a.Address)
                    .ThenInclude(a => a.Municipio)
                    .ThenInclude(m => m.Provincia)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(p => p.OrganismoId == userOrganismoId);
            else if (organismoId.HasValue)
                query = query.Where(p => p.OrganismoId == organismoId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Descripcion.Contains(search));

            if (provinciaId.HasValue)
                query = query.Where(p => p.Stocks.Any(s => s.Almacen.Address != null && s.Almacen.Address.Municipio.ProvinciaId == provinciaId));

            if (municipioId.HasValue)
                query = query.Where(p => p.Stocks.Any(s => s.Almacen.Address != null && s.Almacen.Address.MunicipioId == municipioId));

            query = orderBy?.ToLower() switch
            {
                "descripcion" => ascending ? query.OrderBy(p => p.Descripcion) : query.OrderByDescending(p => p.Descripcion),
                "unidadmedida" => ascending ? query.OrderBy(p => p.UnidadMedida) : query.OrderByDescending(p => p.UnidadMedida),
                "organismo" => ascending ? query.OrderBy(p => p.Organismo.Descripcion) : query.OrderByDescending(p => p.Organismo.Descripcion),
                "creado" => ascending ? query.OrderBy(p => p.Creado) : query.OrderByDescending(p => p.Creado),
                _ => ascending ? query.OrderBy(p => p.Descripcion) : query.OrderByDescending(p => p.Descripcion)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Descripcion = p.Descripcion,
                    UnidadMedida = p.UnidadMedida,
                    OrganismoId = p.OrganismoId,
                    Organismo = p.Organismo != null ? p.Organismo.Descripcion : null,
                    Creado = p.Creado,
                    Modificado = p.Modificado,
                    FechaIngreso = p.FechaIngreso
                })
                .ToListAsync();

            return Ok(new
            {
                items,
                totalCount,
                pageIndex,
                pageSize
            });
        }

        // GET: api/Productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(Guid id)
        {
            var producto = await _context.Productos
                .Include(p => p.Organismo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return NotFound();

            if (!User.IsAdmin())
            {
                var userOrganismoId = User.GetUserOrganismoIdFromToken();
                if (userOrganismoId == null || producto.OrganismoId != userOrganismoId)
                    return Forbid();
            }

            return Ok(new ProductoDto
            {
                Id = producto.Id,
                Descripcion = producto.Descripcion,
                UnidadMedida = producto.UnidadMedida,
                OrganismoId = producto.OrganismoId,
                Organismo = producto.Organismo?.Descripcion,
                Creado = producto.Creado,
                Modificado = producto.Modificado,
                FechaIngreso = producto.FechaIngreso
            });
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> PostProducto(ProductoCreateUpdateDto dto)
        {
            var isAdmin = User.IsAdmin();
            var userOrganismoId = User.GetUserOrganismoIdFromToken();

            int? organismoIdFinal = isAdmin ? dto.OrganismoId : userOrganismoId;
            if (organismoIdFinal == null)
                return BadRequest("El usuario no pertenece a ningún organismo.");

            var producto = new Producto
            {
                Id = Guid.NewGuid(),
                Descripcion = dto.Descripcion,
                UnidadMedida = dto.UnidadMedida,
                OrganismoId = organismoIdFinal,
                FechaIngreso = dto.FechaIngreso,
                Creado = DateTime.UtcNow,
                Modificado = DateTime.UtcNow
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, new ProductoDto
            {
                Id = producto.Id,
                Descripcion = producto.Descripcion,
                UnidadMedida = producto.UnidadMedida,
                OrganismoId = producto.OrganismoId,
                Creado = producto.Creado,
                Modificado = producto.Modificado,
                FechaIngreso = producto.FechaIngreso
            });
        }

        // PUT: api/Productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(Guid id, ProductoCreateUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            var isAdmin = User.IsAdmin();
            var userOrganismoId = User.GetUserOrganismoIdFromToken();

            if (!isAdmin)
            {
                if (userOrganismoId == null || producto.OrganismoId != userOrganismoId)
                    return Forbid();
            }

            producto.Descripcion = dto.Descripcion;
            producto.UnidadMedida = dto.UnidadMedida;
            producto.FechaIngreso = dto.FechaIngreso;
            producto.Modificado = DateTime.UtcNow;

            if (isAdmin && dto.OrganismoId.HasValue)
                producto.OrganismoId = dto.OrganismoId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Productos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(Guid id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            if (!User.IsAdmin())
            {
                var userOrganismoId = User.GetUserOrganismoIdFromToken();
                if (userOrganismoId == null || producto.OrganismoId != userOrganismoId)
                    return Forbid();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Productos/organismos
        [HttpGet("organismos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrganismoDto>>> GetOrganismos()
        {
            var organismos = await _context.Organismos
                .OrderBy(o => o.Descripcion)
                .Select(o => new OrganismoDto { Id = o.Id, Descripcion = o.Descripcion })
                .ToListAsync();
            return Ok(organismos);
        }
    }
}
