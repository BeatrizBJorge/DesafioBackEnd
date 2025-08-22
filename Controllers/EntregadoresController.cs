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

        public EntregadoresController(EntregadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Entregador>>> Get() =>
            await _service.ListarEntregadoresAsync();

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Entregador>> GetById(int id)
        {
            var entregador = await _service.BuscarPorIdAsync(id);
            return entregador == null ? NotFound() : Ok(entregador);
        }

        [HttpPost]
        public async Task<ActionResult<Entregador>> Post(Entregador entregador)
        {
            var novo = await _service.CriarEntregadorAsync(entregador);
            return CreatedAtAction(nameof(GetById), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Entregador entregador)
        {
            var atualizado = await _service.AtualizarEntregadorAsync(id, entregador);
            return atualizado ? NoContent() : NotFound();
        }

        // Upload da CNH
        [HttpPut("{id}/cnh")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCnh(int id, [FromForm] IFormFile file)
        {
            try
            {
                var ok = await _service.UploadCnhAsync(id, file);
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _service.RemoverEntregadorAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}