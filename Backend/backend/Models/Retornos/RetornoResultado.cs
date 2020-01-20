using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Retornos
{
    public class RetornoResultado
    {
        public string mensaje { get; set; }
        public int error { get; set; } // 0 si es exitoso - 1 si tiene error
    }
}
