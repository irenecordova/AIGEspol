using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("CARRERA_ESTUDIANTE", Schema = "ESPOL")]
    public class CdaCarreraEstudiante
    {
        [Key]
        [Column("COD_ESTUDIANTE")]
        public virtual string strCodEstudiante { get; set; }

        [Key]
        [Column("COD_DIVISION")]
        public virtual string strCodDivision { get; set; }

        [Key]
        [Column("COD_CARRERA")]
        public virtual string strCodCarrera { get; set; }

        [Key]
        [Column("COD_ESPECIALIZ")]
        public virtual string strCodEspecializ{ get; set; }

        [Column("TIPO_FLUJO")]
        public virtual string strTipoFlujo { get; set; }

        [Column("MATRICULA_ANTERIOR")]
        public virtual string strMatriculaAnterior { get; set; }

        [Column("ANIO_INGRESO")]
        public virtual string strAnioIngreso{ get; set; }

        [Column("TERMINO_INGRESO")]
        public virtual string strTerminoIngreso { get; set; }

        [Column("ESTADO_CARR_ESTUD")]
        public virtual string strEstadoCarrEstud { get; set; }

        [Column("ESTADO_ACAD")]
        public virtual string strEstadoAcad{ get; set; }

        [Column("PROMEDIO")]
        public virtual Nullable<decimal> decPromedio { get; set; }

        [Column("CUENTA_PAGO")]
        public virtual string strCuentaPago { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("FECHA_CREACION")]
        public virtual Nullable<DateTime> dtFechaCreacion { get; set; }

        [Column("SITIO_CREACION")]
        public virtual string strSitioCreacion { get; set; }

        [Column("COD_LOCALIDAD")]
        public virtual string strCodLocalidad { get; set; }

        [Column("MODIF_PROM")]
        public virtual string strModifProm { get; set; }

        [Column("SECUENCIA")]
        public virtual Nullable<int> intSecuencia { get; set; }

        [Column("MODIF_HISTORIA")]
        public virtual string strModifHistoria { get; set; }

        [Column("PROMEDIO_ANT")]
        public virtual Nullable<decimal> decPromedioAnt { get; set; }

        [Column("PAGOATIEMPO")]
        public virtual string strPagoATiempo { get; set; }

        [Column("CODDIVCARRESPEC")]
        public virtual string strCodDivCarrEspec { get; set; }

        [Column("ANIO_CAMBIO")]
        public virtual string strAnioCambio { get; set; }

        [Column("TERM_CAMBIO")]
        public virtual string strTermCambio { get; set; }

        [Column("ANIO_REINGRESO")]
        public virtual string strAnioReingreso { get; set; }

        [Column("TERMINO_REINGRESO")]
        public virtual string strTerminoReingreso { get; set; }

        [Column("OBSERVACION")]
        public virtual string strObservacion { get; set; }

        [Column("NUMCREDITOTOMLO")]
        public virtual Nullable<int> intNumCreditoTomlo { get; set; }

        [Column("NUMCREDITOTOMOP")]
        public virtual Nullable<int> intNumCreditoTomop { get; set; }

        [Column("IDPROGRAMAPADRE")]
        public virtual Nullable<int> intIdProgramaPadre { get; set; }

    }
}
