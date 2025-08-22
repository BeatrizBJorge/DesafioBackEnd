using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using DesafioBackEnd.DTOs; 

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregadoresController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EntregadoresController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // POST: api/entregadores
        [HttpPost]
        public async Task<ActionResult<Entregador>> CreateEntregador(Entregador entregador)
        {
            // Verificação de CNPJ único
            if (await _context.Entregadores.AnyAsync(e => e.Cnpj == entregador.Cnpj))
                return BadRequest("CNPJ já cadastrado.");

            // Verificação de CNH única
            if (await _context.Entregadores.AnyAsync(e => e.NumeroCnh == entregador.NumeroCnh))
                return BadRequest("Número da CNH já cadastrado.");

            // Validação do Tipo CNH
            var tiposValidos = new[] { "A", "B", "A+B" };
            if (!tiposValidos.Contains(entregador.TipoCnh))
                return BadRequest("Tipo de CNH inválido. Deve ser A, B ou A+B.");

            _context.Entregadores.Add(entregador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntregadorById), new { id = entregador.Id }, entregador);
        }

        // GET: api/entregadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregador>>> GetEntregadores([FromQuery] string? nome)
        {
            var query = _context.Entregadores.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(e => e.Nome.Contains(nome));

            return await query.ToListAsync();
        }

        // GET: api/entregadores/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Entregador>> GetEntregadorById(int id)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null)
                return NotFound();

            return entregador;
        }

        // PUT: api/entregadores/{id}/cnh
        [HttpPut("{id}/cnh")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCnh(int id, [FromForm] UploadCnhDto dto)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null)
                return NotFound();

            var file = dto.File;
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".png" && ext != ".bmp")
                return BadRequest("Somente arquivos PNG ou BMP são permitidos.");

            // Salva arquivo no disco e na pasta uploads
            var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var filePath = Path.Combine(uploadsDir, $"{Guid.NewGuid()}{ext}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            entregador.ImagemCnhPath = filePath;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/entregadores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntregador(int id)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null)
                return NotFound();

            _context.Entregadores.Remove(entregador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}