using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Tools
{
    public static class Constants
    {
        //public static readonly string UrlWebServices = "http://192.168.253.6:8083/api/";
        public static readonly string UrlWebServices = "https://localhost:44336/api/";
        public static readonly string wsTipoSemana = "periodoAcademico/tipoSemana";
        public static readonly string wsDatosMapa = "datosMapa";
        public static readonly string wsEstadisticas = "EstadisticasMapa";
        public static readonly string wsPeriodoActual = "periodoAcademico/actual";
        public static readonly string wsPersonaNombreApellido = "persona/porNombreSeparado";
        public static readonly string wsPersonaNombreCompleto = "persona/porNombre";
        public static readonly string wsIdPorUsuario = "persona/idPersona";

        public static readonly string wsEstudiantesPorCarrera = "estudiantes/carrera";
        public static readonly string wsEstudiantesPorFacultad = "estudiantes/facultad";
        public static readonly string wsEstudiantesPorMateria = "estudiantes/materia";
        public static readonly string wsEstudiantesPorCurso = "estudiantes/curso";

        public static readonly string wsProfesoresPorFacultad = "profesores/facultad";
        public static readonly string wsProfesoresPorMateria = "profesores/materia";

        public static readonly string wsDecanoFacultad = "persona/directivo/facultad";
        public static readonly string wsSubdecanoFacultad = "persona/subdecano/facultad";
        public static readonly string wsDecanosSubdecanosTodos = "persona/directivos";
        public static readonly string wsProfesoresTodos = "persona/profesores";

        public static readonly string wsCursosEstudiante = "curso/estudiante";
        public static readonly string wsCursosProfesor = "curso/profesor";
        public static readonly string wsCursosRelacionados = "curso/relacionados";

        public static readonly string wsMateriasPorProfesor = "materia/profesor";
        public static readonly string wsMateriasPorFacultad = "materia/facultad";
        
        public static readonly string wsHorariosPersonas = "horario/personas";
        public static readonly string wsFacultades = "unidad/facultades";
        public static readonly string wsCarreras = "carreras";
        public static readonly string wsCarrerasPorFacultad = "carreras/unidad/";

        public static readonly string wsLugarPadre = "lugar/padre";
        public static readonly string wsAulasPorBloque = "lugar/aulas/bloque";

        public static readonly string ApiConnectionString = @"User Id=admin;Password=admin;Data Source=" +
            @"(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            @"(HOST=localhost)(PORT=1521))(CONNECT_DATA=" +
            @"(SERVICE_NAME=xe)))";

        public static string datosMapaPrueba()
        {
            string text = File.ReadAllText("DatosMapaPrueba.txt");
            return text;
        }
    }
}
