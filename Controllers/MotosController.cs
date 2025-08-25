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
        public async Task<ActionResult<IEnumerable<Moto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetById(int id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<Moto>> Create(Moto moto)
        {
            var nova = await _service.CreateAsync(moto);
            return CreatedAtAction(nameof(GetById), new { id = nova.Id }, nova);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Moto moto)
        {
            var ok = await _service.UpdateAsync(id, moto);
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