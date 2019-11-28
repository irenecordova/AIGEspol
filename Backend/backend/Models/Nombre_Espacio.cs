using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Nombre_Espacio
    {
        public long Id { get; set; }
        public Espacio espacio { get; set; }
        public string nombre { get; set; }
    }
}
