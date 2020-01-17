using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIGEspol_Frontend.Models
{
    public class Lista
    {
        public int idCreador { get; set; }
        public string nombre { get; set; }
        public List<int> idPersonas { get; set; }
        public List<string> nombrePersonas { get; set; }
    }
}
