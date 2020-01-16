using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_CURSO", Schema = "ESPOL")]
    public class CdaCurso
    {
        [Key]
        [Column("IDCURSO")]
        public virtual int intIdCurso { get; set; }
        
        [Column("IDPROGRAMA")]
        public virtual int intIdPrograma { get; set; }

        [Column("IDPERIODO")]
        public virtual int intIdPeriodo { get; set; }
        
        [Column("FECHAINICIO")]
        public virtual Nullable<DateTime> dtFechaInicio { get; set; }

        [Column("FECHAFIN")]
        public virtual Nullable<DateTime> dtFechaFin { get; set; }

        [Column("PARALELO")]
        public virtual Int16 intParalelo { get; set; }

        [Column("NUMREGISTRADOS")]
        public virtual Nullable<Int16> intNumRegistrados { get; set; }

        [Column("CAPACIDAD")]
        public virtual int intCapacidad { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("TIPOCURSO")]
        public virtual string strTipoCurso { get; set; }

        [Column("IDPROFESOR")]
        public virtual int intIdProfesor { get; set; }

        [Column("IDMETODOEVAL")]
        public virtual int intIdMetodoEval { get; set; }

        [Column("IDMATERIA")]
        public virtual int intIdMateria { get; set; }

        [Column("IDUNIDADCARGA")]
        public virtual int intIdUnidadCarga { get; set; }

        [Column("CODPARALELOACAD")]
        public virtual string strCodParaleloAcad { get; set; }

        [Column("IDUNIDAD")]
        public virtual int intIdUnidad { get; set; }

        [Column("CUPOMINIMO")]
        public virtual int intCupoMinimo { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("CUPOPLANIFICADO")]
        public virtual int intCupoPlanificado { get; set; }

        [Column("NUMHORASTEORICAS")]
        public virtual int intNumHorasTeoricas { get; set; }

        [Column("NUMHORASPRACTICAS")]
        public virtual int intNumHorasPracticas { get; set; }

        [Column("TIENEGRUPO")]
        public virtual string strTieneGrupo{ get; set; }

        [Column("MODALIDAD")]
        public virtual string strModalidad { get; set; }

        [Column("IDBLOQUE")]
        public virtual int intIdBloque { get; set; }

        [Column("VERSION")]
        public virtual int intVersion { get; set; }

        [Column("PARALELOADM")]
        public virtual string strParaleloAdm { get; set; }

        [Column("IDCURSOMIGRADO")]
        public virtual int intIdCursoMigrado { get; set; }

        [Column("CUPO_DISPONIBLE")]
        public virtual int intCupo_Disponible { get; set; }

        [Column("NUMHORASPRACTTOTAL")]
        public virtual int intNumHorasPractTotal { get; set; }

        [Column("NUMHORASTEORICTOTAL")]
        public virtual int intNumHorasTeoricTotal { get; set; }

        [Column("CUPODISPPREREG")]
        public virtual int intCupoDispPreReg { get; set; }

        [Column("VALOR_PREREGISTRO")]
        public virtual decimal decValor_Preregistro { get; set; }

        [Column("PARALELOCERRADO")]
        public virtual string strParaleloCerrado { get; set; }

        [Column("PORCPROFESOR")]
        public virtual decimal decPorcProfesor { get; set; }

        [Column("IDPROFESOR1")]
        public virtual int intIdProfesor1 { get; set; }

        [Column("PORCPROFESOR1")]
        public virtual decimal decPorcProfesor1 { get; set; }
        
        [Column("IDPROFESOR2")]
        public virtual int intIdProfesor2 { get; set; }

        [Column("PORCPROFESOR2")]
        public virtual decimal decPorcProfesor2 { get; set; }

        [Column("IDPROFESOR3")]
        public virtual int intIdProfesor3 { get; set; }

        [Column("PORCPROFESOR3")]
        public virtual decimal decPorcProfesor3 { get; set; }

        [Column("SUPERVISOR")]
        public virtual int intSupervisor { get; set; }

        [Column("APROBADO")]
        public virtual string strAprobado { get; set; }

        [Column("HORASSUPERVISOR")]
        public virtual int intHorasSupervisor { get; set; }

        [Column("APROBADO1")]
        public virtual string strAprobado1 { get; set; }

        [Column("APROBADO2")]
        public virtual string strAprobado2 { get; set; }

        [Column("APROBADO3")]
        public virtual string strAprobado3 { get; set; }

        [Column("APROBADOSU")]
        public virtual string strAprobadoSu { get; set; }

        [Column("CUPONOVATO")]
        public virtual int intCupoNovato { get; set; }

        [Column("ESDICTADOINGLES")]
        public virtual string strEsDictadoIngles { get; set; }
        
        [Column("ESQUINCENA")]
        public virtual string strEsQuincena { get; set; }

        [Column("NUMQUINCENA")]
        public virtual int intNumQuincena { get; set; }

        [Column("ESAUTOMATICO")]
        public virtual string strEsAutomatico { get; set; }

        [Column("IDCURSOPADRE")]
        public virtual int intIdCursoPadre { get; set; }

        [Column("ESRELACIONADOGRUPOS")]
        public virtual string strEsRelacionadoGrupos { get; set; }

        [Column("IDPROFESOR4")]
        public virtual int intIdProfesor4 { get; set; }

        [Column("PORCPROFESOR4")]
        public virtual decimal decPorcProfesor4 { get; set; }

        [Column("APROBADO4")]
        public virtual string strAprobado4 { get; set; }

        [Column("IDPROFESOR5")]
        public virtual int intIdProfesor5 { get; set; }

        [Column("PORCPROFESOR5")]
        public virtual decimal decPorcProfesor5 { get; set; }

        [Column("APROBADO5")]
        public virtual string strAprobado6 { get; set; }

        [Column("TIENEMOOC")]
        public virtual string strTieneMooc { get; set; }
    }
}
