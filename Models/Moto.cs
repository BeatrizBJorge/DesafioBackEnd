using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackEnd.Models
{
    public class Moto
    {
        public int Id { get; set; }
        public int Ano { get; set; }
        public string? Modelo { get; set; }
        public string? Placa { get; set; }
        public bool Disponivel { get; set; } = true;
    }
}