using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Espacio
    {
        public long Id { get; set; }
        public long Descripcion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}
