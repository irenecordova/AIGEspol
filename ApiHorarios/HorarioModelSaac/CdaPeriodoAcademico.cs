using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_PERIODO_ACADEMICO", Schema = "ESPOL")]
    public class CdaPeriodoAcademico
    {

        [Key]
        [Column("IDPERIODO")]
        public virtual int intIdPeriodoAcademico { get; set; }

        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("FECHAINICIO")]
        public virtual Nullable<DateTime> dtFechaInicio { get; set; }

        [Column("FECHAFIN")]
        public virtual Nullable<DateTime> dtFechaFin { get; set; }

        [Column("FECHAINIEVAL1")]
        public virtual Nullable<DateTime> FechaIniEval1 { get; set; }

        [Column("FECHAINIEVAL2")]
        public virtual Nullable<DateTime> FechaIniEval2 { get; set; }

        [Column("FECHAINIEVAL3")]
        public virtual Nullable<DateTime> FechaIniEval3 { get; set; }
        [Column("FECHAINIEVAL4")]
        public virtual Nullable<DateTime> FechaIniEval4 { get; set; }
        [Column("FECHAINIEVAL5")]
        public virtual Nullable<DateTime> FechaIniEval5 { get; set; }
        [Column("FECHAFINEVAL1")]
        public virtual Nullable<DateTime> FechaFinEval1 { get; set; }
        [Column("FECHAFINEVAL2")]
        public virtual Nullable<DateTime> FechaFinEval2 { get; set; }

        [Column("FECHAFINEVAL3")]
        public virtual Nullable<DateTime> FechaFinEval3 { get; set; }

        [Column("FECHAFINEVAL4")]
        public virtual Nullable<DateTime> FechaFinEval4 { get; set; }

        [Column("FECHAFINEVAL5")]
        public virtual Nullable<DateTime> FechaFinEval5 { get; set; }

        [Column("FECHAINIMEJO")]
        public virtual Nullable<DateTime> FechaIniMejoramiento { get; set; }

        [Column("FECHAFINMEJO")]
        public virtual Nullable<DateTime> FechaFinMejoramiento { get; set; }

        [Column("TERMINO")]
        public virtual string strTermino { get; set; }

        [Column("ESTADO")]
        public virtual string chEstado { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("TIPOPERIODO")]
        public virtual string chTipoPeriodo { get; set; }

        [Column("ANIO")]
        public virtual string strAnio { get; set; }

        [Column("SIGLAS")]
        public virtual string strSiglas { get; set; }

    }
}
