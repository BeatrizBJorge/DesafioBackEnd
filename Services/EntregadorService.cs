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
    public class EntregadorService
    {
        private readonly AppDbContext _context;

        public EntregadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entregador>> GetAllAsync()
        {
            return await _context.Entregadores.ToListAsync();
        }

        public async Task<Entregador?> GetByIdAsync(int id)
        {
            return await _context.Entregadores.FindAsync(id);
        }

        public async Task<Entregador> CreateAsync(Entregador entregador)
        {
            _context.Entregadores.Add(entregador);
            await _context.SaveChangesAsync();
            return entregador;
        }

        public async Task<bool> UpdateAsync(int id, Entregador entregador)
        {
            var existing = await _context.Entregadores.FindAsync(id);
            if (existing == null)
                return false;

            existing.Nome = entregador.Nome;
            existing.Cnpj = entregador.Cnpj;
            existing.DataNascimento = entregador.DataNascimento;
            existing.CnhNumero = entregador.CnhNumero;
            existing.CnhCategoria = entregador.CnhCategoria;
            existing.CnhDataValidade = entregador.CnhDataValidade;
            existing.ImagemCnhPath = entregador.ImagemCnhPath;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null)
                return false;

            _context.Entregadores.Remove(entregador);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}