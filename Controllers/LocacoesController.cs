using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using DesafioBackEnd.Services;

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocacoesController : ControllerBase
    {
        private readonly LocacaoService _service;

        public LocacoesController(LocacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetById(int id)
        {
            var locacao = await _service.GetByIdAsync(id);
            if (locacao == null) return NotFound();
            return Ok(locacao);
        }

        [HttpPost]
        public async Task<ActionResult<Locacao>> Create(Locacao locacao)
        {
            var nova = await _service.CreateAsync(locacao);
            return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(int id, [FromBody] DateTime dataFim)
        {
            var ok = await _service.FinalizarLocacaoAsync(id, dataFim);
            if (!ok) return NotFound();
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