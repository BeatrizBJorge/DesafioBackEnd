using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackEnd.Models
{
    public class Entregador
    {
        public int Id { get; set; }
        public string? Identificador { get; set; }
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? NumeroCnh { get; set; }
        public string? TipoCnh { get; set; }
        public string? ImagemCnhPath { get; set; }
    }
}