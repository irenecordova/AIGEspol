using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_HORARIO_CONTENIDO", Schema = "ESPOL")]
    public class CdaHorarioContenido
    {
        [Key]
        [Column("IDHORARIOCONTENIDO")]
        public virtual int intIdHorarioContenido { get; set; }

        [Column("IDSOLICITUDREC")]
        public virtual Nullable<int> intIdSolicitudRec { get; set; }

        [Column("IDCURSO")]
        public virtual Nullable<int> intIdCurso { get; set; }

        [Column("FECHA")]
        public virtual Nullable<DateTime> dtFecha { get; set; }

        [Column("HORAINICIO")]
        public virtual Nullable<TimeSpan> tsHoraInicio { get; set; }

        [Column("HORAFIN")]
        public virtual Nullable<TimeSpan> tsHoraFin { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("ESTADORECUPERACION")]
        public virtual string strEstadoRecuperacion { get; set; }

        [Column("IDLUGARESPOL")]
        public virtual Nullable<int> intIdLugarEspol { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("OBSERVACION")]
        public virtual string strObservacion { get; set; }

        [Column("HORAINICIOPLANIFICADO")]
        public virtual Nullable<TimeSpan> tsHoraInicioPlanificado { get; set; }

        [Column("HORAFINPLANIFICADO")]
        public virtual Nullable<TimeSpan> tsHoraFinPlanificado { get; set; }
    }
}
