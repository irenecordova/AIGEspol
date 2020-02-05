using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_HORARIO", Schema = "ESPOL")]
    public class CdaHorario
    {
        [Key]
        [Column("IDHORARIO")]
        public virtual int intIdHorario { get; set; }

        [Column("IDCURSO")]
        public virtual Nullable<int> intIdCurso { get; set; }

        [Column("EXAMEN")]
        public virtual string strExamen { get; set; }

        [Column("FECHA")]
        public virtual Nullable<DateTime> dtFecha { get; set; }

        [Column("DIA")]
        public virtual Nullable<Int16> intDia { get; set; }

        [Column("HORAINICIO")]
        public virtual Nullable<TimeSpan> tsHoraInicio { get; set; }

        [Column("HORAFIN")]
        public virtual Nullable<TimeSpan> tsHoraFin { get; set; }

        [Column("TIPO")]
        public virtual string chTipo { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual DateTime dtUltimoCambio{ get; set; }

        [Column("IDAULA")]
        public virtual Nullable<Int16> intIdAula { get; set; }

        [Column("CODHORARIOACAD")]
        public virtual string strCodHorarioAcad { get; set; }

        [Column("VERSION")]
        public virtual Nullable<Int64> intVersion { get; set; }

    }
}
