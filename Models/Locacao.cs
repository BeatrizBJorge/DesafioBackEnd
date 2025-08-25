using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackEnd.Models
{
    public class Locacao
    {
        public int Id { get; set; }

        public int MotoId { get; set; }
        public Moto? Moto { get; set; }

        public int EntregadorId { get; set; }
        public Entregador? Entregador { get; set; }
        
        public DateTime DataInicio { get; set; } = DateTime.UtcNow;
        public DateTime DataFim { get; set; }

    }
}