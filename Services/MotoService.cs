using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
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

        public async Task<List<Moto>> ListarMotosAsync()
        {
            return await _context.Motos.ToListAsync();
        }

        public async Task<Moto?> BuscarPorIdAsync(int id)
        {
            return await _context.Motos.FindAsync(id);
        }

        public async Task<Moto> CriarMotoAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task<bool> AtualizarMotoAsync(int id, Moto motoAtualizada)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return false;

            moto.Modelo = motoAtualizada.Modelo;
            moto.Placa = motoAtualizada.Placa;
            moto.Ano = motoAtualizada.Ano;
            moto.Disponivel = motoAtualizada.Disponivel;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverMotoAsync(int id)
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