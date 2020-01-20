using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Services;

namespace backend.Models.Retornos
{
    public class RetornoInvitacionDetallada
    {
        public int idInvitacion { get; set; }
        public int idPersona { get; set; }
        public string estado { get; set; }
        public string cancelada { get; set; }
        public Reunion reunion { get; set; }

        public RetornoInvitacionDetallada(Invitacion invitacion, ContextAIG context)
        {
            this.idInvitacion = invitacion.idInvitacion;
            this.idPersona = invitacion.idPersona;
            this.estado = invitacion.estado;
            this.cancelada = invitacion.cancelada;
            this.reunion = context.TBL_Reunion.Where(x => x.id == invitacion.idReunion).FirstOrDefault();
        }

    }
}
