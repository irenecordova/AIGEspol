using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIGEspol_Frontend.Models
{
    public class Lista
    {
        public string name { get; set; }
        public int idOwner { get; set; }
        public List<int> idPersons { get; set; }
    }
}
