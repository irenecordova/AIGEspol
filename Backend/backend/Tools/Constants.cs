﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Tools
{
    // Clase que define todas las constantes necesarias para el funcionamiento del módulo.
    public static class Constants
    {

        // String de conexión a la base de datos local
        public static readonly string ApiConnectionString = @"User Id=admin;Password=admin;Data Source=" +
            @"(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            @"(HOST=localhost)(PORT=1521))(CONNECT_DATA=" +
            @"(SERVICE_NAME=xe)))";

        // Constantes para tener definidas las urls a las que debe realizar solicitudes el módulo
        // para obtener los datos de los Web Services
        public static readonly string UrlWebServices = "http://192.168.253.6:8083/api/";

        public static readonly string wsTipoSemana = "periodoAcademico/tipoSemana";
        public static readonly string wsDatosMapa = "datosMapa";
        public static readonly string wsEstadisticas = "EstadisticasMapa";
        public static readonly string wsPeriodoActual = "periodoAcademico/actual";
        public static string wsPersona(int idPersona)
        {
            return "persona/" + idPersona.ToString();
        }
        public static readonly string wsPersonaNombreApellido = "persona/porNombreSeparado";
        public static readonly string wsPersonaNombreCompleto = "persona/porNombre";
        public static readonly string wsIdPorUsuario = "persona/idPersona";
        public static readonly string wsNombresPersonas = "persona/nombresPersonas";
        public static readonly string wsCorreosPersonas = "persona/emails";

        public static readonly string wsEstudiantesPorCarrera = "persona/estudiantes/carrera";
        public static readonly string wsEstudiantesPorFacultad = "persona/estudiantes/facultad";
        public static readonly string wsEstudiantesPorMateria = "persona/estudiantes/materia";
        public static readonly string wsEstudiantesPorCurso = "persona/estudiantes/curso";

        public static readonly string wsProfesoresPorFacultad = "persona/profesores/facultad";
        public static readonly string wsProfesoresPorMateria = "persona/profesores/materia";

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

        public static readonly string wsAulasDisponibles = "lugar/disponibles";
        public static readonly string wsAulasDisponiblesRango = "lugar/disponibles/rango";

        public static string wsLugar(int idLugar) {
            return "lugar/" + idLugar.ToString();
        } 
        public static readonly string wsLugarPadre = "lugar/padre";
        public static readonly string wsAulasPorBloque = "lugar/aulas/bloque";

<<<<<<< HEAD
        //public static readonly string UrlWebServices = "https://localhost:44336/api/";
=======
        public static readonly string ApiConnectionString = @"User Id=SYSTEM;Password=admin;Data Source=" +
            @"(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            @"(HOST=localhost)(PORT=1521))(CONNECT_DATA=" +
            @"(SERVICE_NAME=xe)))";

>>>>>>> 4efdb84bd03d4e3062cda346862d0f51d54158fb

    }
}
