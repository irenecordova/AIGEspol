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
        public int intIdSolicitud { get; set; }

        [Column("IDAPROBADOR")]
        public Nullable<int> intIdAprobador { get; set; }

        [Column("IDSOLICITANTE")]
        public Nullable<int> intIdSolicitante { get; set; }

        [Column("TIPO")]
        public string strTipo { get; set; }

        [Column("FECHA")]
        public Nullable<DateTime> dtFecha { get; set; }

        [Column("HORA")]
        public Nullable<DateTime> dtHora { get; set; }

        [Column("FECHAAPROB")]
        public Nullable<DateTime> dtFechaAprobacion { get; set; }

        [Column("HORAAPROB")]
        public Nullable<DateTime> dtHoraAprobacion { get; set; }

        [Column("ESTADO")]
        public string strEstado { get; set; }

        [Column("MOTIVOSOLICITANTE")]
        public string strMotivoSolicitante { get; set; }

        [Column("MOTIVOAPROBADOR")]
        public string strMotivoAprobador { get; set; }

        [Column("IDLUGARESPOL")]
        public Nullable<int> intIdLugarEspol { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public Nullable<DateTime> dtUltimoCambio { get; set; }
    }
}
