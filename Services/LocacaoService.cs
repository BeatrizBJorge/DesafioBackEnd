using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using DesafioBackEnd.Controllers;
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

        public async Task<List<Locacao>> GetAllAsync()
        {
            return await _context.Locacoes
                .Include(l => l.Moto)
                .Include(l => l.Entregador)
                .ToListAsync();
        }

        public async Task<Locacao?> GetByIdAsync(int id)
        {
            return await _context.Locacoes
                .Include(l => l.Moto)
                .Include(l => l.Entregador)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Locacao> CreateAsync(Locacao locacao)
        {
            _context.Locacoes.Add(locacao);

            var moto = await _context.Motos.FindAsync(locacao.MotoId);
            if (moto != null)
            {
                moto.Disponivel = false;
            }

            await _context.SaveChangesAsync();
            return locacao;
        }

        public async Task<bool> FinalizarLocacaoAsync(int id, DateTime dataFim)
        {
            var locacao = await _context.Locacoes
                .Include(l => l.Moto)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (locacao == null)
                return false;

            locacao.DataFim = dataFim;

            if (locacao.Moto != null)
            {
                locacao.Moto.Disponivel = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
                return false;

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}