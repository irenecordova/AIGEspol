using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Tools
{
    public static class Constants
    {
        public static readonly string UrlWebServices = "https://localhost:44336/api/";
        public static readonly string wsTipoSemana = "periodoAcademico/tipoSemana";
        public static readonly string wsDatosMapa = "datosMapa";
        public static readonly string wsEstadisticas = "EstadisticasMapa";
        public static readonly string wsPeriodoActual = "periodoActual";
        public static readonly string wsPersonaNombreApellido = "personasPorNombreYApellido";
        public static readonly string wsIdPorUsuario = "persona/idPersona";

        public static readonly string wsEstudiantesPorCarrera = "estudiantesPorCarrera";
        public static readonly string wsEstudiantesPorFacultad = "estudiantesPorFacultad";
        public static readonly string wsEstudiantesPorMateria = "estudiantesPorMateria";
        public static readonly string wsEstudiantesPorCurso = "estudiantesPorCurso";

        public static readonly string wsProfesoresPorFacultad = "profesoresPorFacultad";
        public static readonly string wsProfesoresPorMateria = "profesoresPorMateria";

        public static readonly string wsDecanoFacultad = "directivoFacultad";
        public static readonly string wsSubdecanoFacultad = "subdecanoFacultad";

        public static readonly string wsCursosEstudiante = "cursosEstudiante";
        public static readonly string wsCursosProfesor = "cursosProfesor";
        public static readonly string wsCursosRelacionados = "cursosRelacionados";

        public static readonly string wsMateriasPorProfesor = "materiasPorProfesor";
        public static readonly string wsMateriasPorFacultad = "materiasPorFacultad";
        public static readonly string wsHorarioEstudiante = "horarioEstudiante";
        public static readonly string wsHorarioProfesor = "horarioProfesor";
        public static readonly string wsEsProfesor = "esProfesor";
        
        public static readonly string wsHorariosPersonas = "horariosPersonas";
        public static readonly string wsFacultades = "unidad/facultades";
        public static readonly string wsCarreras = "carreras";
        public static readonly string wsCarrerasPorFacultad = "carreras/unidad/";

        public static readonly string ApiConnectionString = @"User Id=admin;Password=admin;Data Source=" +
            @"(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            @"(HOST=localhost)(PORT=1521))(CONNECT_DATA=" +
            @"(SERVICE_NAME=xe)))";

        public static readonly string CursorBloque = "BLOQUECURSOR";
        public static readonly string NombreSPBloqueList = "BLOQUE_LIST";
        public static readonly string NombreSPBloqueItemId = "BLOQUE_ITEM_ID";

        public static readonly string CursorEspacio = "ESPACIOCURSOR";
        public static readonly string NombreSPEspacioList = "ESPACIO_LIST";
        public static readonly string NombreSPEspacioItemId = "ESPACIO_ITEM_ID";

        public static readonly string CursorInvitacion = "INVITACIONCURSOR";
        public static readonly string NombreSPInvitacionList = "INVITACION_LIST";
        public static readonly string NombreSPInvitacionItemId = "INVITACION_ITEM_ID";

        public static readonly string CursorEstadoInvitacion = "ESTADOINVITACIONCURSOR";
        public static readonly string NombreSPEstadoInvitacionList = "ESTADO_INVITACION_LIST";
        public static readonly string NombreSPEstadoInvitacionItemId = "ESTADO_INVITACION_ITEM_ID";

        public static readonly string CursorListaPersonalizada = "LISTAPERSONALIZADACURSOR";
        public static readonly string NombreSPListaPersonalizadaList = "LISTA_PERSONALIZADA_LIST";
        public static readonly string NombreSPListaPersonalizadaItemId = "LISTAPERSONALIZADA_ITEM_ID";

        public static readonly string CursorNombreBloque = "NOMBREBLOQUECURSOR";
        public static readonly string NombreSPNombreBloqueList = "NOMBRE_BLOQUE_LIST";
        public static readonly string NombreSPNombreBloqueItemId = "NOMBRE_BLOQUE_ITEM_ID";

        public static readonly string CursorNombreEspacio = "NOMBREESPACIOCURSOR";
        public static readonly string NombreSPNombreEspacioList = "NOMBRE_ESPACIO_LIST";
        public static readonly string NombreSPNombreEspacioItemId = "NOMBRE_ESPACIO_ITEM_ID";

        public static readonly string CursorReunion = "REUNIONCURSOR";
        public static readonly string NombreSPReunionList = "REUNION_LIST";
        public static readonly string NombreSPReunionItemId = "REUNION_ITEM_ID";

        public static readonly string CursorTipoEspacio = "TIPOESPACIOCURSOR";
        public static readonly string NombreSPTipoEspacioList = "TIPO_ESPACIO_LIST";
        public static readonly string NombreSPTipoEspacioItemId = "TIPO_ESPACIO_ITEM_ID";

        public static readonly string CursorTipoFiltro = "TIPOFILTROCURSOR";
        public static readonly string NombreSPTipoFiltroList = "TIPO_FILTRO_LIST";
        public static readonly string NombreSPTipoFiltroItemId = "TIPO_FILTRO_ITEM_ID";

        public static readonly string NombreRetornoId = "RETURN_ID";
        public static string datosMapaPrueba()
        {
            string text = File.ReadAllText("DatosMapaPrueba.txt");
            return text;
        }
    }
}
