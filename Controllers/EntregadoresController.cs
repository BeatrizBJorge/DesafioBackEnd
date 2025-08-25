using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using DesafioBackEnd.DTOs; 
using DesafioBackEnd.Services;

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregadoresController : ControllerBase
    {
        private readonly EntregadorService _service;
        private readonly IWebHostEnvironment _env;

        public EntregadoresController(EntregadorService service, IWebHostEnvironment _env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregador>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entregador>> GetById(int id)
        {
            var entregador = await _service.GetByIdAsync(id);
            if (entregador == null) return NotFound();
            return Ok(entregador);
        }

        [HttpPost]
        public async Task<ActionResult<Entregador>> Create(Entregador entregador)
        {
            var novo = await _service.CreateAsync(entregador);
            return CreatedAtAction(nameof(GetById), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}/cnh")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCnh(int id, [FromForm] UploadCnhDto dto)
        {
            var entregador = await _service.GetByIdAsync(id);
            if (entregador == null)
                return NotFound();

            var file = dto.File;
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".png" && ext != ".bmp")
                return BadRequest("Somente arquivos PNG ou BMP são permitidos.");

            var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var filePath = Path.Combine(uploadsDir, $"{Guid.NewGuid()}{ext}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            entregador.ImagemCnhPath = filePath;
            await _service.UpdateAsync(id, entregador);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}