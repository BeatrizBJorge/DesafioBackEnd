using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Data;
using DesafioBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Services
{
    public class EntregadorService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EntregadorService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Entregador>> ListarEntregadoresAsync()
        {
            return await _context.Entregadores.ToListAsync();
        }

        public async Task<Entregador?> BuscarPorIdAsync(int id)
        {
            return await _context.Entregadores.FindAsync(id);
        }

        public async Task<Entregador> CriarEntregadorAsync(Entregador entregador)
        {
            _context.Entregadores.Add(entregador);
            await _context.SaveChangesAsync();
            return entregador;
        }

        public async Task<bool> AtualizarEntregadorAsync(int id, Entregador atualizado)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null)
                return false;

            entregador.Nome = atualizado.Nome;
            entregador.Cnh = atualizado.Cnh;
            entregador.DataValidadeCnh = atualizado.DataValidadeCnh;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UploadCnhAsync(int id, IFormFile file)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador == null) return false;

            // Validacoes para upload da cnh
            if (file == null || file.Length == 0) 
                throw new InvalidOperationException("Arquivo inválido.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".png" && ext != ".bmp")
                throw new InvalidOperationException("Arquivo somente PNG ou BMP são permitidos.");

            var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var filePath = Path.Combine(uploadsDir, $"{Guid.NewGuid()}{ext}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            entregador.ImagemCnhPath = filePath;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoverEntregadorAsync(int id)
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