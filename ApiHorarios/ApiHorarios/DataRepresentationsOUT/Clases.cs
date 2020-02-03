using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHorarios.DataRepresentationsOUT
{
    public class RetornoEstadisticas
    {
        public int numRegistrados { get; set; }
        public int cantBloquesUsados { get; set; }
        public int cantBloquesTotales { get; set; }
        public int cantLugares { get; set; }
        public int cantLugaresUsados { get; set; }
        public double promPersonasPorLugar { get; set; }
        public double promPersonasPorBloque { get; set; }
        public int totalPersonasMomento { get; set; }
        public IQueryable top3Bloques { get; set; }
    }
}
