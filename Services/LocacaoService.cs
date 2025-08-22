using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Services
{
    public class LocacaoService
    {
        private readonly AppDbContext _context;

        public LocacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Locacao>> ListarLocacoesAsync()
        {
            return await _context.Locacoes
                .Include(l => l.Moto)
                .Include(l => l.Entregador)
                .ToListAsync();
        }

        public async Task<Locacao?> BuscarPorIdAsync(int id)
        {
            return await _context.Locacoes
                .Include(l => l.Moto)
                .Include(l => l.Entregador)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Locacao> CriarLocacaoAsync(Locacao locacao)
        {
            
            var moto = await _context.Motos.FindAsync(locacao.MotoId);
            if (moto == null)
                throw new InvalidOperationException("Moto não encontrada.");

            
            bool motoAlugada = await _context.Locacoes.AnyAsync(l => l.MotoId == locacao.MotoId);
            if (motoAlugada)
                throw new InvalidOperationException("Moto já está alugada.");

            
            var entregador = await _context.Entregadores.FindAsync(locacao.EntregadorId);
            if (entregador == null)
                throw new InvalidOperationException("Entregador não encontrado.");

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return locacao;
        }

        public async Task<bool> AtualizarLocacaoAsync(int id, Locacao locacao)
        {
            var existente = await _context.Locacoes.FindAsync(id);
            if (existente == null)
                return false;

            existente.DataInicio = locacao.DataInicio;
            existente.DataFim = locacao.DataFim;
            existente.MotoId = locacao.MotoId;
            existente.EntregadorId = locacao.EntregadorId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverLocacaoAsync(int id)
        {
            var existente = await _context.Locacoes.FindAsync(id);
            if (existente == null)
                return false;

            _context.Locacoes.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}