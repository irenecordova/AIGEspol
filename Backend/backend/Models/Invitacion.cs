using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Invitacion
    {
        public long Id { get; set; }
        public long ReunionId { get; set; }
        public string UsernameInvitado { get; set; }
        public string EstadoInvitacion { get; set; }
        public bool eliminado { get; set; }
    }
}
