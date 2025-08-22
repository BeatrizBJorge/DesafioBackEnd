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
        public async Task<ActionResult<List<Locacao>>> Get() =>
            await _service.ListarLocacoesAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetById(int id)
        {
            var locacao = await _service.BuscarPorIdAsync(id);
            return locacao == null ? NotFound() : Ok(locacao);
        }

        [HttpPost]
        public async Task<ActionResult<Locacao>> Post(Locacao locacao)
        {
            try
            {
                var nova = await _service.CriarLocacaoAsync(locacao);
                return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Locacao locacao)
        {
            var atualizada = await _service.AtualizarLocacaoAsync(id, locacao);
            return atualizada ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removida = await _service.RemoverLocacaoAsync(id);
            return removida ? NoContent() : NotFound();
        }
    }
}