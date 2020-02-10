using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_CPL_CAMBIOS_CURSO", Schema = "ESPOL")]
    public class CdaCambiosCurso
    {
        [Key]
        [Column("IDCPLCAMBIOSCURSO")]
        public virtual int intIdCplCambiosCurso { get; set; }

        [Column("IDPROFESOR")]
        public virtual Nullable<int> intIdProfesor { get; set; }

        [Column("FECHAINICIO")]
        public virtual Nullable<DateTime> dtFechaInicio { get; set; }

        [Column("FECHAFIN")]
        public virtual Nullable<DateTime> dtFechaFin { get; set; }

        [Column("PORCENTAJE")]
        public virtual Nullable<decimal> dcPorcentaje { get; set; }

        [Column("TIPOPROFESOR")]
        public virtual string strTipoProfesor { get; set; }

        [Column("HORASSUPERVISOR")]
        public virtual Nullable<int> intHorasSupervisor { get; set; }

        [Column("IDCPLSOLICITUDES")]
        public virtual Nullable<int> intIdCplSolicitudes { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> tsUltimo_Cambio { get; set; }

        [Column("ACCION")]
        public virtual string strAccion { get; set; }

        [Column("HORASCARGALABORAL")]
        public virtual Nullable<int> intHorasCargaLaboral { get; set; }

        [Column("IDCURSO")]
        public virtual Nullable<int> intIdCurso { get; set; }

        [Column("PROCESADO")]
        public virtual Nullable<int> intProcesado { get; set; }

        [Column("HORASCARGAANTERIOR")]
        public virtual Nullable<decimal> dcHorasCargaAnterior { get; set; }

        [Column("APROBADO")]
        public virtual string strAprobado { get; set; }

    }
}
