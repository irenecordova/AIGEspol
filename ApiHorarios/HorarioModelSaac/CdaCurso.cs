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
        public virtual Nullable<int> intIdPrograma { get; set; }

        [Column("IDPERIODO")]
        public virtual Nullable<int> intIdPeriodo { get; set; }
        
        [Column("FECHAINICIO")]
        public virtual Nullable<DateTime> dtFechaInicio { get; set; }

        [Column("FECHAFIN")]
        public virtual Nullable<DateTime> dtFechaFin { get; set; }

        [Column("PARALELO")]
        public virtual Nullable<Int16> intParalelo { get; set; }

        [Column("NUMREGISTRADOS")]
        public virtual Nullable<Int16> intNumRegistrados { get; set; }

        [Column("CAPACIDAD")]
        public virtual Nullable<Int16> intCapacidad { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("TIPOCURSO")]
        public virtual string strTipoCurso { get; set; }

        [Column("IDPROFESOR")]
        public virtual Nullable<int> intIdProfesor { get; set; }

        [Column("IDMETODOEVAL")]
        public virtual Nullable<int> intIdMetodoEval { get; set; }

        [Column("IDMATERIA")]
        public virtual Nullable<int> intIdMateria { get; set; }

        [Column("IDUNIDADCARGA")]
        public virtual Nullable<Int16> intIdUnidadCarga { get; set; }

        [Column("CODPARALELOACAD")]
        public virtual string strCodParaleloAcad { get; set; }

        [Column("IDUNIDAD")]
        public virtual Nullable<Int16> intIdUnidad { get; set; }

        [Column("CUPOMINIMO")]
        public virtual Nullable<int> intCupoMinimo { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("CUPOPLANIFICADO")]
        public virtual Nullable<Int16> intCupoPlanificado { get; set; }

        [Column("NUMHORASTEORICAS")]
        public virtual Nullable<Int16> intNumHorasTeoricas { get; set; }

        [Column("NUMHORASPRACTICAS")]
        public virtual Nullable<Int16> intNumHorasPracticas { get; set; }

        [Column("TIENEGRUPO")]
        public virtual string strTieneGrupo{ get; set; }

        [Column("MODALIDAD")]
        public virtual string strModalidad { get; set; }

        [Column("IDBLOQUE")]
        public virtual Nullable<int> intIdBloque { get; set; }

        [Column("VERSION")]
        public virtual Nullable<Int64> intVersion { get; set; }

        [Column("PARALELOADM")]
        public virtual string strParaleloAdm { get; set; }

        [Column("IDCURSOMIGRADO")]
        public virtual Nullable<int> intIdCursoMigrado { get; set; }

        [Column("CUPO_DISPONIBLE")]
        public virtual Nullable<Int16> intCupo_Disponible { get; set; }

        [Column("NUMHORASPRACTTOTAL")]
        public virtual Nullable<int> intNumHorasPractTotal { get; set; }

        [Column("NUMHORASTEORICTOTAL")]
        public virtual Nullable<int> intNumHorasTeoricTotal { get; set; }

        [Column("CUPODISPPREREG")]
        public virtual Nullable<int> intCupoDispPreReg { get; set; }

        [Column("VALOR_PREREGISTRO")]
        public virtual Nullable<decimal> decValor_Preregistro { get; set; }

        [Column("PARALELOCERRADO")]
        public virtual string strParaleloCerrado { get; set; }

        [Column("PORCPROFESOR")]
        public virtual Nullable<decimal> decPorcProfesor { get; set; }

        [Column("IDPROFESOR1")]
        public virtual Nullable<int> intIdProfesor1 { get; set; }

        [Column("PORCPROFESOR1")]
        public virtual Nullable<decimal> decPorcProfesor1 { get; set; }
        
        [Column("IDPROFESOR2")]
        public virtual Nullable<int> intIdProfesor2 { get; set; }

        [Column("PORCPROFESOR2")]
        public virtual Nullable<decimal> decPorcProfesor2 { get; set; }

        [Column("IDPROFESOR3")]
        public virtual Nullable<int> intIdProfesor3 { get; set; }

        [Column("PORCPROFESOR3")]
        public virtual Nullable<decimal> decPorcProfesor3 { get; set; }

        [Column("SUPERVISOR")]
        public virtual Nullable<int> intSupervisor { get; set; }

        [Column("APROBADO")]
        public virtual string strAprobado { get; set; }

        [Column("HORASSUPERVISOR")]
        public virtual Nullable<int> intHorasSupervisor { get; set; }

        [Column("APROBADO1")]
        public virtual string strAprobado1 { get; set; }

        [Column("APROBADO2")]
        public virtual string strAprobado2 { get; set; }

        [Column("APROBADO3")]
        public virtual string strAprobado3 { get; set; }

        [Column("APROBADOSU")]
        public virtual string strAprobadoSu { get; set; }

        [Column("CUPONOVATO")]
        public virtual Nullable<int> intCupoNovato { get; set; }

        [Column("ESDICTADOINGLES")]
        public virtual string strEsDictadoIngles { get; set; }
        
        [Column("ESQUINCENA")]
        public virtual string strEsQuincena { get; set; }

        [Column("NUMQUINCENA")]
        public virtual Nullable<int> intNumQuincena { get; set; }

        [Column("ESAUTOMATICO")]
        public virtual string strEsAutomatico { get; set; }

        [Column("IDCURSOPADRE")]
        public virtual Nullable<int> intIdCursoPadre { get; set; }

        [Column("ESRELACIONADOGRUPOS")]
        public virtual string strEsRelacionadoGrupos { get; set; }

        [Column("IDPROFESOR4")]
        public virtual Nullable<int> intIdProfesor4 { get; set; }

        [Column("PORCPROFESOR4")]
        public virtual Nullable<decimal> decPorcProfesor4 { get; set; }

        [Column("APROBADO4")]
        public virtual string strAprobado4 { get; set; }

        [Column("IDPROFESOR5")]
        public virtual Nullable<int> intIdProfesor5 { get; set; }

        [Column("PORCPROFESOR5")]
        public virtual Nullable<decimal> decPorcProfesor5 { get; set; }

        [Column("APROBADO5")]
        public virtual string strAprobado6 { get; set; }

        [Column("TIENEMOOC")]
        public virtual string strTieneMooc { get; set; }

        public int cantidadProfesores()
        {
            var cantidadProfesores = 0;
            if (this.intIdProfesor != null) cantidadProfesores++;
            if (this.intIdProfesor1 != null) cantidadProfesores++;
            if (this.intIdProfesor2 != null) cantidadProfesores++;
            if (this.intIdProfesor3 != null) cantidadProfesores++;
            if (this.intIdProfesor4 != null) cantidadProfesores++;
            if (this.intIdProfesor5 != null) cantidadProfesores++;
            return cantidadProfesores;
        }
    }
}
