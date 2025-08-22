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

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : Controller
    {
        private readonly AppDbContext _context;

        public MotosController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/motos
        [HttpPost]
        public async Task<ActionResult<Moto>> CreateMoto(Moto moto)
        {
            // Verificação da placa
            var exists = await _context.Motos.AnyAsync(m => m.Placa == moto.Placa);
            if (exists)
            {
                return BadRequest("Placa já cadastrada.");
            }
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMotoById), new { id = moto.Id }, moto);
        }

        // GET: api/motos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos([FromQuery] string? placa)
        {
            var query = _context.Motos.AsQueryable();

            if (!string.IsNullOrEmpty(placa))
            {
                query = query.Where(m => m.Placa.Contains(placa));
            }

            return await query.ToListAsync();
        }

        // GET: api/motos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMotoById(int id)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }

        // PUT: api/motos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaca(int id, [FromBody] string novaPlaca)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            // Verificação de nova placa
            var exists = await _context.Motos.AnyAsync(m => m.Placa == novaPlaca && m.Id != id);
            if (exists)
            {
                return BadRequest("Placa já cadastrada.");
            }

            moto.Placa = novaPlaca;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/motos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}