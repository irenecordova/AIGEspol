using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Reunion
    {
        public long Id { get; set; }
        public string UsernameOrganizador { get; set; }
        public string Eliminado { get; set; }
    }
}
