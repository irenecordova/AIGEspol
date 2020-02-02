using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Envios
{
    public class RespuestasWS
    {
    }

    public class ClasePersona
    {
        public int intIdPersona { get; set; }
        public Nullable<int> intIdLugarNacimiento { get; set; }
        public Nullable<int> intIdLugarDomicilio { get; set; }
        public Nullable<Int16> intIdNacionalidad { get; set; }
        public string strTipoIdentificacion { get; set; }
        public string strNumeroIdentificacion { get; set; }
        public string strApellidos { get; set; }
        public string strNombres { get; set; }
        public string strEmail { get; set; }
        public string strEstadoCivil { get; set; }
        public string strSexo { get; set; }
        public Nullable<DateTime> dtFechaNacimiento { get; set; }
        public string strNumeroPasaporte { get; set; }
        public string strNumeroLibretaMilitar { get; set; }
        public string strFoto { get; set; }
        public Nullable<DateTime> dtFechaDefuncion { get; set; }
        public string strTipoSangre { get; set; }
        public string strEstadoPersona { get; set; }
        public string strNumeroLibretaIess { get; set; }
        public string strCodEstudiante { get; set; }
        public string strEstadoEmpleado { get; set; }
        public string strArchivoCV { get; set; }
        public Nullable<Int16> intIdIdiomaNatal { get; set; }
        public string strContactoEmergencia { get; set; }
        public Nullable<DateTime> dtUltimo_Cambio { get; set; }
        public string strEmailAlterno { get; set; }
        public string strRuc { get; set; }
        public string strCodBanco { get; set; }
        public string strNroCta { get; set; }
        public string strTipoCta { get; set; }
        public string strIdClienteFact { get; set; }
        public string strClienteFact { get; set; }
        public string strTipoIdClienteFact { get; set; }
        public string strDirecClienteFact { get; set; }
        public string strTelefonoFact { get; set; }
        public string strNroAutContrib { get; set; }
        public string strNroAutImpren { get; set; }
        public Nullable<DateTime> dtFecValidDcto { get; set; }
        public Nullable<Int16> intDiasSegurIess { get; set; }
        public string strComentario { get; set; }
        public Nullable<Int64> intVersion { get; set; }
        public string strNombreContacto { get; set; }
        public string strTelefonoContacto { get; set; }
        public string strDireccionContacto { get; set; }
        public string strEtnia { get; set; }
        public string strTieneDiscapacidad { get; set; }
        public string strNumCarnetConadis { get; set; }
        public string strEstado { get; set; }
        public string strTipoDiscapacidad { get; set; }
        public Nullable<Int16> intNumeroHijos { get; set; }
        public string strCuentaTw { get; set; }
        public string strCuentaFb { get; set; }
        public string strParentesco { get; set; }
        public Nullable<decimal> decPorcentaje_Disc { get; set; }
        public string strFonoCelularContacto { get; set; }
        public string strGenerAutoIdentificacion { get; set; }
        public string strPrefijoNombre { get; set; }
        public Nullable<DateTime> dtFechaActualizacion { get; set; }
        public string strApellido1 { get; set; }
        public string strApellido2 { get; set; }
        public string strPagWeb { get; set; }
        public string strGenero { get; set; }
        public string strCuentaLi { get; set; }
    }

    public class DatosPersonaWS
    {
        public int idPersona { get; set; }
        public string matricula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
    }

    public class DatosMapaWS
    {
        public Nullable<int> idHorario { get; set; }
        public Nullable<DateTime> fecha { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public string tipoHorario { get; set; }
        public int numRegistrados { get; set; }
        public string tipoCurso { get; set; }
        public int idLugar { get; set; }
        public string descripcionLugar { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string tipoLugar { get; set; }

    }

    public class TipoSemana
    {
        public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
    }

    public class HorarioPersona
    {
        public int idPersona { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Nullable<int> idCurso { get; set; }
        public string nombreMateria { get; set; }
        public string nombreCompletoMateria { get; set; }
        public Nullable<DateTime> cursoFechaInicio { get; set; }
        public Nullable<DateTime> cursoFechaFin { get; set; }
        public int horarioDia { get; set; }
        public Nullable<DateTime> horarioFecha { get; set; }
        public Nullable<DateTime> horarioHoraInicio { get; set; }
        public Nullable<DateTime> horarioHoraFin { get; set; }
        public string horarioTipo { get; set; }
    }

    public class IdPadre
    {
        public int idPadre { get; set; }
    }

    public class DatosLugar
    {
        public Int16 intIdLugarEspol { get; set; }
        public Nullable<Int16> intIdLugarPadre { get; set; }
        public string strTipo { get; set; }
        public string strDescripcion { get; set; }
        public Nullable<int> intCapacidad { get; set; }
        public Nullable<DateTime> dtUltimo_Cambio { get; set; }
        public Nullable<int> intCapacidadOyente { get; set; }
        public string strEstado { get; set; }
        public Nullable<Int64> intVersion { get; set; }
        public string strCodLocalidad { get; set; }
        public string strLatitud { get; set; }
        public string strLongitud { get; set; }
        public string strObservacion { get; set; }
    }

    public class WsNombrePersona
    {
        public int idPersona { get; set; }
        public string nombreCompleto { get; set; }
    }
}
