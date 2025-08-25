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
    public class MotoService
    {
        private readonly AppDbContext _context;

        public MotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Moto>> GetAllAsync()
        {
            return await _context.Motos.ToListAsync();
        }

        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _context.Motos.FindAsync(id);
        }

        public async Task<Moto> CreateAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task<bool> UpdateAsync(int id, Moto moto)
        {
            var existing = await _context.Motos.FindAsync(id);
            if (existing == null)
                return false;

            existing.Modelo = moto.Modelo;
            existing.Placa = moto.Placa;
            existing.Ano = moto.Ano;
            existing.Disponivel = moto.Disponivel;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return false;

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}