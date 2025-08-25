using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackEnd.Models
{
    public class Entregador
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        
        public string? CnhNumero { get; set; }
        public string? CnhCategoria { get; set; }
        public DateTime CnhDataValidade { get; set; }
        public string? ImagemCnhPath { get; set; }
    }
}