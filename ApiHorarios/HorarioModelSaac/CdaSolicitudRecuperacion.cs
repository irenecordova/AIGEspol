using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_SOLICITUD_REC", Schema = "ESPOL")]
    public class CdaSolicitudRecuperacion
    {
        [Key]
        [Column("IDSOLICITUD")]
        public virtual int intIdSolicitud { get; set; }

        [Column("IDAPROBADOR")]
        public virtual Nullable<int> intIdAprobador { get; set; }

        [Column("IDSOLICITANTE")]
        public virtual Nullable<int> intIdSolicitante { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("FECHA")]
        public virtual Nullable<DateTime> dtFecha { get; set; }

        [Column("HORA")]
        public virtual Nullable<TimeSpan> dtHora { get; set; }

        [Column("FECHAAPROB")]
        public virtual Nullable<DateTime> dtFechaAprobacion { get; set; }

        [Column("HORAAPROB")]
        public virtual Nullable<TimeSpan> dtHoraAprobacion { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("MOTIVOSOLICITANTE")]
        public virtual string strMotivoSolicitante { get; set; }

        [Column("MOTIVOAPROBADOR")]
        public virtual string strMotivoAprobador { get; set; }

        [Column("IDLUGARESPOL")]
        public virtual Nullable<int> intIdLugarEspol { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }
    }
}
