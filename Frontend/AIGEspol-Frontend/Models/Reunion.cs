using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIGEspol_Frontend.Models
{
    public class Reunion
    {
        public int idCreador { get; set; }
        public string asunto { get; set; }
        public string descripcion { get; set; }
        public int idLugar { get; set; }
        public int fechaInicio { get; set; }
        public int fechaFin { get; set; }
        public List<int> idPersonas { get; set; }
    }
}
