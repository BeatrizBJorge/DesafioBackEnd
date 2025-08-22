using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;

namespace DesafioBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocacoesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/locacoes
        [HttpPost]
        public async Task<ActionResult<Locacao>> CreateLocacao([FromBody] Locacao locacao)
        {
            // Busca do entregador
            var entregador = await _context.Entregadores.FindAsync(locacao.EntregadorId);
            if (entregador == null)
                return BadRequest("Entregador não encontrado.");

            // Entregador precisa ter CNH tipo A para alugar a moto
            if (!entregador.TipoCnh.Contains("A"))
                return BadRequest("Entregador não possui CNH tipo A.");

            // Busca a moto
            var moto = await _context.Motos.FindAsync(locacao.MotoId);
            if (moto == null)
                return BadRequest("Moto não encontrada.");

            // Define DataInicio
            locacao.DataInicio = DateTime.Today.AddDays(1);

            // Define DataFimPrevisto com base no plano
            locacao.DataFimPrevisto = locacao.DataInicio.AddDays(locacao.DiasPlano);

            // Define valor da diária
            locacao.ValorDiaria = locacao.DiasPlano switch
            {
                7 => 30m,
                15 => 28m,
                30 => 22m,
                45 => 20m,
                50 => 18m,
                _ => throw new ArgumentException("Plano inválido.")
            };

            locacao.ValorTotal = locacao.ValorDiaria * locacao.DiasPlano;

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocacaoById), new { id = locacao.Id }, locacao);
        }

        // GET: api/locacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes
                .Include(l => l.Entregador)
                .Include(l => l.Moto)
                .ToListAsync();
        }

        // GET: api/locacoes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetLocacaoById(int id)
        {
            var locacao = await _context.Locacoes
                .Include(l => l.Entregador)
                .Include(l => l.Moto)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (locacao == null)
                return NotFound();

            return locacao;
        }

        // PUT: api/locacoes/{id}/devolver
        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> DevolverMoto(int id, [FromBody] DateTime dataDevolucao)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return NotFound();

            locacao.DataFimReal = dataDevolucao;

            decimal valorTotal = 0;
            int diasPlano = locacao.DiasPlano;
            decimal valorDiaria = locacao.ValorDiaria;

            if (dataDevolucao < locacao.DataFimPrevisto)
            {
                // Percentual da Multa por devolver antes do prazo
                int diasNaoUsados = (locacao.DataFimPrevisto - dataDevolucao).Days;
                decimal percentualMulta = diasPlano switch
                {
                    7 => 0.20m,
                    15 => 0.40m,
                    _ => 0m
                };

                valorTotal = valorDiaria * (diasPlano - diasNaoUsados) + valorDiaria * diasNaoUsados * percentualMulta;
            }
            else if (dataDevolucao > locacao.DataFimPrevisto)
            {
                // Valor adicional a ser cobrado pelos dias extras
                int diasExtras = (dataDevolucao - locacao.DataFimPrevisto).Days;
                valorTotal = valorDiaria * diasPlano + diasExtras * 50m;
            }
            else
            {
                // Entrega na data correta
                valorTotal = valorDiaria * diasPlano;
            }

            locacao.ValorTotal = valorTotal;
            await _context.SaveChangesAsync();

            return Ok(locacao);
        }

        // DELETE: api/locacoes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return NotFound();

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}