using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using DesafioBackEnd.Services;

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : Controller
    {
        private readonly MotoService _service;

        public MotosController(MotoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Moto>>> Get() =>
            await _service.ListarMotosAsync();


        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetById(int id)
        {
            var moto = await _service.BuscarPorIdAsync(id);
            return moto == null ? NotFound() : Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<Moto>> Post(Moto moto)
        {
            var nova = await _service.CriarMotoAsync(moto);
            return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Moto moto)
        {
            var atualizado = await _service.AtualizarMotoAsync(id, moto);
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _service.RemoverMotoAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}