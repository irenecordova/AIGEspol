using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_PROGRAMA_ACADEMICO", Schema = "ESPOL")]
    public class CdaProgramaAcademico
    {
        [Key]
        [Column("IDPROGRAMA")]
        public virtual int intIdPrograma { get; set; }

        [Column("IDPROGRAMAPADRE")]
        public virtual Nullable<int> intIdProgramaPadre { get; set; }

        [Column("IDCOORDINADOR")]
        public virtual Nullable<int> intIdCoordinador { get; set; }

        [Column("IDDIRECTOR")]
        public virtual Nullable<int> intIdDirector { get; set; }

        [Column("IDMETODOEVAL")]
        public virtual Nullable<int> intIdMetodoEval { get; set; }

        [Column("IDUNIDADAVALA")]
        public virtual Nullable<Int16> intIdUnidadAvala { get; set; }

        [Column("IDUNIDADEJECUTA")]
        public virtual Nullable<Int16> intIdUnidadEjecuta { get; set; }

        [Column("IDLUGARESPOL")]
        public virtual Nullable<Int16> intIdLugarEspol { get; set; }
        
        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("COSTO")]
        public virtual Nullable<decimal> decCosto { get; set; }

        [Column("TIPOPROGRAMA")]
        public virtual string strTipoPrograma { get; set; }

        [Column("COMENTARIOS")]
        public virtual string strComentarios { get; set; }

        [Column("NOMBRECONESUP")]
        public virtual string strNombreConesup { get; set; }

        [Column("CATEGORIA")]
        public virtual string strCategoria { get; set; }

        [Column("NIVELESTUDIO")]
        public virtual string strNivelEstudio { get; set; }

        [Column("TITULO")]
        public virtual string strTitulo { get; set; }

        [Column("TITULOMASCULINO")]
        public virtual string strTituloMasculino { get; set; }

        [Column("TITULOFEMENINO")]
        public virtual string strTituloFemenino { get; set; }

        [Column("NUMCREDITOS")]
        public virtual Nullable<decimal> decNumCreditos { get; set; }

        [Column("NUMHORAS")]
        public virtual Nullable<Int16> intNumHoras { get; set; }

        [Column("NUMMESES")]
        public virtual Nullable<Int16> intNumMeses { get; set; }

        [Column("NUMANIOS")]
        public virtual Nullable<decimal> decNumAnios { get; set; }

        [Column("CODIGOCONESUP")]
        public virtual string strCodigoConesup { get; set; }

        [Column("FECHACODCONESUP")]
        public virtual Nullable<DateTime> dtFechaCodConesup { get; set; }

        [Column("FECHAEXPCODCONESUP")]
        public virtual Nullable<DateTime> dtFechaExpCodConesup { get; set; }

        //[Column("MAXNUMCONVALIDACION")]
        //public virtual Nullable<Int16> intMaxNumConvalidacion { get; set; }

        [Column("MAXNUMREPROBAR")]
        public virtual Nullable<Int16> intMaxNumReprobar { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimo_Cambio { get; set; }

        [Column("SITIOWEB")]
        public virtual string strSitioWeb { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("MODALIDAD")]
        public virtual string strModalidad { get; set; }

        [Column("CODDIVCARRERAESPEC")]
        public virtual string strCodDivCarreraEspec { get; set; }

        [Column("NUMNIVEL")]
        public virtual Nullable<int> intNumNivel { get; set; }

        [Column("VERSION")]
        public virtual Nullable<Int64> intVersion { get; set; }

        [Column("NOMBREINGLES")]
        public virtual string strNombreIngles { get; set; }

        [Column("TITULO_INGLES")]
        public virtual string strTitulo_Ingles { get; set; }

        [Column("ESTAVIGENTE")]
        public virtual string strEstaVigente { get; set; }

        [Column("SIGLASRRA")]
        public virtual string strSiglasRRA { get; set; }

        [Column("IDCAMPOAMPLIO")]
        public virtual Nullable<int> intIdCampoAmplio { get; set; }

        [Column("IDCAMPOESPECIFICO")]
        public virtual Nullable<int> intIdCampoEspecifico { get; set; }

        [Column("IDCAMPODETALLADO")]
        public virtual Nullable<int> intIdCampoDetallado { get; set; }

        [Column("IDCARRERAGRADO")]
        public virtual Nullable<int> intIdCarreraGrado { get; set; }

        [Column("IDTITULOGRADO")]
        public virtual Nullable<int> intIdTituloGrado { get; set; }

        [Column("IDNOMENCLATURATIT")]
        public virtual Nullable<int> intIdNomenclaturaTIT { get; set; }

        [Column("IDPERSONA")]
        public virtual Nullable<int> intIdPersona { get; set; }

        [Column("TIPOPERSONA")]
        public virtual string strTipoPersona { get; set; }

        [Column("TIPOCARRERA")]
        public virtual string strTipoCarrera { get; set; }

        [Column("ACREDITACION")]
        public virtual string strAcreditacion { get; set; }

        [Column("FECHAACREDITACIONI")]
        public virtual Nullable<DateTime> dtFechaAcreditacionI { get; set; }

        [Column("CODDIVISION")]
        public virtual string strCodDivision { get; set; }

        [Column("CODCARRERA")]
        public virtual string strCodCarrera { get; set; }

        [Column("CODESPECIALIZ")]
        public virtual string strCodEspecializ { get; set; }

        [Column("ABREVINGLES")]
        public virtual string strAbrevIngles { get; set; }

        [Column("NOMBREACREDITACION")]
        public virtual string strNombreAcreditacion { get; set; }

        [Column("RESOLUCIONCES")]
        public virtual string strResolucionCes { get; set; }

        [Column("FECHARESOLCES")]
        public virtual Nullable<DateTime> dtFechaResolCes{ get; set; }

        [Column("RUTARESOLCD")]
        public virtual string strRutaResolCD{ get; set; }

        [Column("RUTARESOLCP")]
        public virtual string strRutaResolCP { get; set; }

        [Column("RUTARESOLCE")]
        public virtual string strRutaResolCE { get; set; }

        [Column("VISION")]
        public virtual string strVision { get; set; }

        [Column("ESDECIENCIAS")]
        public virtual string strEsDeCiencias { get; set; }

        [Column("CREDITOSREGULAR")]
        public virtual Nullable<Int16> intCreditosRegular { get; set; }

        [Column("DESCRIPCIONGENERAL")]
        public virtual string strDescripcionGeneral { get; set; }

        [Column("RESOLUCIONACADEMICA")]
        public virtual string strResolucionAcademica { get; set; }

        //[Column("RESOLUCIONCONSPOLITECNICA")]
        //public virtual string strResolucionConsPoli { get; set; }

        [Column("FECHARESOLACADEMICA")]
        public virtual Nullable<DateTime> dtFechaResolucionAcademica { get; set; }

        //[Column("FECHARESOLUCIONCONSPOLITECNICA")]
        //public virtual Nullable<DateTime> dtFechaResolucionConsPoli { get; set; }

        [Column("MISION")]
        public virtual string strMision{ get; set; }

        //[Column("TIENEPRACTICAVACACIONAL")]
        //public virtual string strTienePracticaVacacional { get; set; }

        [Column("ESCOMPLETA")]
        public virtual string strEsCompleta { get; set; }

        [Column("NUMPERIODODURACION")]
        public virtual Nullable<Int16> intNumPeriodoDuracion { get; set; }

        [Column("NUMNIVELESINGLES")]
        public virtual Nullable<Int16> intNumNivelesIngles { get; set; }

        [Column("NUMNIVELES")]
        public virtual Nullable<Int16> intNumNiveles { get; set; }

        [Column("TIENEBASICA")]
        public virtual string strTieneBasica { get; set; }

        [Column("TIENEMATERIACLAVES")]
        public virtual string strTieneMateriaClaves { get; set; }

        [Column("CUENTAPAGO")]
        public virtual string strCuentaPago { get; set; }

        [Column("TIPOFLUJODEF")]
        public virtual string strTipoFlujoDef { get; set; }

        [Column("PROMEDIO")]
        public virtual Nullable<decimal> decPromedio { get; set; }

        [Column("FECHAFINREGISTRO")]
        public virtual Nullable<DateTime> dtFechaFinRegistro { get; set; }

        [Column("FECHAINICIOREGISTRO")]
        public virtual Nullable<DateTime> dtFechaInicioRegistro { get; set; }

        [Column("CONTROLDEUDAS")]
        public virtual string strControlDeudas { get; set; }

        [Column("NUMPERIODOS")]
        public virtual Nullable<Int16> intNumPeriodos { get; set; }

        [Column("TIPOPERIODO")]
        public virtual string strTipoPeriodo { get; set; }

        [Column("TIENEAFECTACION")]
        public virtual string strTieneAfectacion { get; set; }

        [Column("CARRERAAUTOFINAN")]
        public virtual string strCarreraAutoFinan { get; set; }

        [Column("BASICA")]
        public virtual string strBasica { get; set; }

        [Column("CODAFECTACIONCOBRO")]
        public virtual string strCodAfectacionCobro { get; set; }

        [Column("ESNUEVA")]
        public virtual string strEsNueva { get; set; }

        [Column("CODEQUIVALENTE")]
        public virtual string strCodEquivalente { get; set; }

        [Column("PAGOCREDITO")]
        public virtual string strPagoCredito { get; set; }

        [Column("FECHACREACION")]
        public virtual Nullable<DateTime> dtFechaCreacion { get; set; }

        [Column("CODNUMRESOLUCION")]
        public virtual string strCodNumResolucion { get; set; }

        [Column("PERFILPROFES")]
        public virtual string strPerfilProfes { get; set; }

        [Column("FECHAACREDITACIONF")]
        public virtual Nullable<DateTime> dtFechaAcreditacionF { get; set; }

        [Column("CODNIVEL")]
        public virtual string strCodNivel { get; set; }

        [Column("CONSIDERARPADRE")]
        public virtual string strConsiderarPadre { get; set; }
    }
}
