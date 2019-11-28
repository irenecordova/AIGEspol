using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Nombre_Bloque
    {
        public long Id { get; set; }
        public Bloque bloque { get; set; }
        public string nombre { get; set; }
    }
}
