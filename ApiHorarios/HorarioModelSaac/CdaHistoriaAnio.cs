using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("HISTORIA_ANIO", Schema = "Espol")]
    public class CdaHistoriaAnio
    {
        [Column("ANIO")]
        public virtual string strAnio { get; set; }

        [Column("TERMINO")]
        public virtual string strTermino { get; set; }

        [Column("COD_MATERIA_ACAD")]
        public virtual string strCodMateria { get; set; }

        [Column("PARALELO")]
        public virtual string strParalelo { get; set; }

        [Column("COD_ESTUDIANTE")]
        public virtual string strCodEstudiante { get; set; }

        [Column("COD_DIVISION")]
        public virtual string strCodDivision { get; set; }

        [Column("COD_CARRERA")]
        public virtual string strCodCarrera { get; set; }

        [Column("COD_ESPECIALIZACION")]
        public virtual string strCodEspecializacion { get; set; }

        //[Column("ESTADO_MAT_REGISTRADA")]
        //public virtual string strEstadoMateriaRegistrada { get; set; }

        [Column("ESTADO_AUTORIZ")]
        public virtual string strEstadoAutoriz { get; set; }

        //OBSERV_REG_A...
        //ESTADO_MAT_T...
        //[Column("NOTA1")]
        //public virtual string strNota1 { get; set; }

        [Column("USER")]
        public virtual string strUser { get; set; }

        [Column("ESTADO_INCOMP")]
        public virtual string strEstadoIncomp { get; set; }

        //[Column("MATERIA_REGISTRADA")]
        //public virtual string strMateriaRegistrada { get; set; }

        [Key]
        [Column("IDCURSO")]
        public virtual int intIdCurso { get; set; }
    }
}
