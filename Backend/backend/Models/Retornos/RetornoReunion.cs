using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Services;
using Newtonsoft.Json;
using backend.Models.Envios;

namespace backend.Models.Retornos
{
    public class RetornoReunion
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
        public List<Invitacion> invitaciones { get; set; }
        public string nombreLugar { get; set; }

        public RetornoReunion(Reunion reunion, ContextAIG context)
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
            this.invitaciones = context.TBL_Invitacion.Where(x => x.idReunion == reunion.id).ToList();
            this.nombreLugar = JsonConvert.DeserializeObject<DatosLugar>(conexionEspol.Lugar(idLugar).Result).strDescripcion;
        }
    }
}
