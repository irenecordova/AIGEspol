using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Services;
using backend.Models.Envios;
using backend.Models.Retornos;
using Newtonsoft.Json;

namespace backend.Models.Retornos
{
    public class RetornoInvitacionDetallada
    {
        public int idInvitacion { get; set; }
        public int idPersona { get; set; }
        public string estado { get; set; }
        public string cancelada { get; set; }
        public ReunionDentroInvitacion reunion { get; set; }

        public class ReunionDentroInvitacion
        {
            public int id { get; set; }
            public int idCreador { get; set; }
            public string cancelada { get; set; }
            public string asunto { get; set; }
            public string descripcion { get; set; }
            public int idLugar { get; set; }
            public Nullable<DateTime> fechaInicio { get; set; }
            public Nullable<DateTime> fechaFin { get; set; }
            public Nullable<int> idPeriodo { get; set; }
            public string nombreLugar { get; set; }

            public ReunionDentroInvitacion(Reunion reunion, ContextAIG context)
            {
                ConexionEspol conexionEspol = new ConexionEspol();
                this.id = reunion.id;
                this.idCreador = reunion.idCreador;
                this.cancelada = reunion.cancelada;
                this.asunto = reunion.asunto;
                this.descripcion = reunion.descripcion;
                this.idLugar = reunion.idLugar;
                this.fechaInicio = reunion.fechaInicio;
                this.fechaFin = reunion.fechaFin;
                this.idPeriodo = reunion.idPeriodo;
                this.nombreLugar = JsonConvert.DeserializeObject<DatosLugar>(conexionEspol.Lugar(idLugar).Result).strDescripcion;
            }
        }

        public RetornoInvitacionDetallada(Invitacion invitacion, ContextAIG context)
        {
            this.idInvitacion = invitacion.idInvitacion;
            this.idPersona = invitacion.idPersona;
            this.estado = invitacion.estado;
            this.cancelada = invitacion.cancelada;
            this.reunion = new ReunionDentroInvitacion(context.TBL_Reunion.Where(x => x.id == invitacion.idReunion).FirstOrDefault(), context);
        }

    }
}
