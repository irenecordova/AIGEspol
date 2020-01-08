using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    public class CdaPersona
    {
        [Key]
        [Column("IDPERSONA")]
        public virtual int intIdPersona { get; set; }
        
        [Column("IDLUGARNACIMIENTO")]
        public virtual Nullable<int> intIdLugarNacimiento { get; set; }

        [Column("IDLUGARDOMICILIO")]
        public virtual Nullable<int> intIdLugarDomicilio { get; set; }

        [Column("IDNACIONALIDAD")]
        public virtual Nullable<Int16> intIdNacionalidad { get; set; }
        
        [Column("TIPOIDENTIFICACION")]
        public virtual string strTipoIdentificacion { get; set; }

        [Column("NUMEROIDENTIFICACION")]
        public virtual string strNumeroIdentificacion { get; set; }

        [Column("APELLIDOS")]
        public virtual string strApellidos { get; set; }

        [Column("NOMBRES")]
        public virtual string strNombres { get; set; }

        [Column("EMAIL")]
        public virtual string strEmail { get; set; }

        [Column("ESTADOCIVIL")]
        public virtual string strEstadoCivil { get; set; }

        [Column("SEXO")]
        public virtual string strSexo { get; set; }
        
        [Column("FECHANACIMIENTO")]
        public virtual Nullable<DateTime> dtFechaNacimiento { get; set; }

        [Column("NUMEROPASAPORTE")]
        public virtual string strNumeroPasaporte { get; set; }

        [Column("NUMEROLIBRETAMILITAR")]
        public virtual string strNumeroLibretaMilitar { get; set; }

        [Column("FOTO")]
        public virtual string strFoto { get; set; }

        [Column("FECHADEFUNCION")]
        public virtual Nullable<DateTime> dtFechaDefuncion { get; set; }

        [Column("TIPOSANGRE")]
        public virtual string strTipoSangre { get; set; }

        [Column("ESTADOPERSONA")]
        public virtual string strEstadoPersona { get; set; }

        [Column("NUMEROLIBRETAIESS")]
        public virtual string strNumeroLibretaIess { get; set; }

        [Column("CODESTUDIANTE")]
        public virtual string strCodEstudiante { get; set; }

        [Column("ESTADOEMPLEADO")]
        public virtual string strEstadoEmpleado { get; set; }

        [Column("ARCHIVOCV")]
        public virtual string strArchivoCV { get; set; }

        [Column("IDIOMANATAL")]
        public virtual Nullable<Int16> intIdIdiomaNatal { get; set; }

        [Column("CONTACTOEMERGENCIA")]
        public virtual string strContactoEmergencia { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimo_Cambio{ get; set; }

        [Column("EMAILALTERNO")]
        public virtual string strEmailAlterno { get; set; }

        [Column("RUC")]
        public virtual string strRuc { get; set; }

        [Column("CODBANCO")]
        public virtual string strCodBanco { get; set; }

        [Column("NROCTA")]
        public virtual string strNroCta { get; set; }

        [Column("TIPOCTA")]
        public virtual string strTipoCta { get; set; }

        [Column("IDCLIENTEFACT")]
        public virtual string strIdClienteFact { get; set; }

        [Column("CLIENTEFACT")]
        public virtual string strClienteFact { get; set; }

        [Column("TIPOIDCLIENTEFACT")]
        public virtual string strTipoIdClienteFact { get; set; }

        [Column("DIRECCLIENTEFACT")]
        public virtual string strDirecClienteFact { get; set; }

        [Column("TELEFONOFACT")]
        public virtual string strTelefonoFact { get; set; }

        [Column("NROAUTCONTRIB")]
        public virtual string strNroAutContrib { get; set; }

        [Column("NROAUTIMPREN")]
        public virtual string strNroAutImpren { get; set; }

        [Column("FECVALIDDCTO")]
        public virtual Nullable<DateTime> dtFecValidDcto { get; set; }

        [Column("DIASSEGURIESS")]
        public virtual Nullable<Int16> intDiasSegurIess { get; set; }

        [Column("COMENTARIO")]
        public virtual string strComentario { get; set; }

        [Column("VERSION")]
        public virtual Nullable<Int64> intVersion { get; set; }

        [Column("NOMBRECONTACTO")]
        public virtual string strNombreContacto { get; set; }

        [Column("TELEFONOCONTACTO")]
        public virtual string strTelefonoContacto { get; set; }

        [Column("DIRECCIONCONTACTO")]
        public virtual string strDireccionContacto { get; set; }

        [Column("ETNIA")]
        public virtual string strEtnia { get; set; }

        [Column("TIENEDISCAPACIDAD")]
        public virtual string strTieneDiscapacidad { get; set; }

        [Column("NUMCARNETCONADIS")]
        public virtual string strNumCarnetConadis { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("TIPODISCAPACIDAD")]
        public virtual string strTipoDiscapacidad { get; set; }
        
        [Column("NUMEROHIJOS")]
        public virtual Nullable<Int16> intNumeroHijos { get; set; }

        [Column("CUENTATW")]
        public virtual string strCuentaTw { get; set; }

        [Column("CUENTAFB")]
        public virtual string strCuentaFb { get; set; }

        [Column("PARENTESCOCONTACTO")]
        public virtual string strParentesco { get; set; }

        [Column("PORCENTAJE_DISC")]
        public virtual Nullable<decimal> decPorcentaje_Disc{ get; set; }

        [Column("FONOCELULARCONTACTO")]
        public virtual string strFonoCelularContacto { get; set; }

        [Column("GENEROAUTOIDENTIFICACION")]
        public virtual string strGenerAutoIdentificacion { get; set; }

        [Column("PREFIJONOMBRE")]
        public virtual string strPrefijoNombre { get; set; }

        [Column("FECHAACTUALIZACION")]
        public virtual Nullable<DateTime> dtFechaActualizacion { get; set; }
        
        [Column("APELLIDO1")]
        public virtual string strApellido1 { get; set; }

        [Column("APELLIDO2")]
        public virtual string strApellido2 { get; set; }

        [Column("PAGWEB")]
        public virtual string strPagWeb { get; set; }

        [Column("GENERO")]
        public virtual string strGenero { get; set; }
        
        [Column("CUENTALI")]
        public virtual string strCuentaLi { get; set; }
        
    }
}
