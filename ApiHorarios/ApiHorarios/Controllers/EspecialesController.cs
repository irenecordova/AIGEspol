using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ApiHorarios.DataRepresentationsIN;
using ApiHorarios.DataRepresentationsOUT;

namespace ApiHorarios.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class EspecialesController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC contextSAAC;
        private readonly CdaDbContextDB2SAF3 contextSAF3;

        public EspecialesController(CdaDbContextDB2SAAC context, CdaDbContextDB2SAF3 context2)
        {
            this.contextSAAC = context;
            this.contextSAF3 = context2;
        }

        public CdaPeriodoAcademico periodoActual()
        {
            return contextSAAC.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
        }

        [HttpPost("datosMapa")]
        public IQueryable datosMapa([FromBody] InDatosMapa data)
        {
            CdaPeriodoAcademico periodoActual = this.periodoActual();
            string examen = null;
            if (data.fecha >= periodoActual.FechaIniEval1 && data.fecha <= periodoActual.FechaFinEval1) examen = "1";
            if (data.fecha >= periodoActual.FechaIniEval2 && data.fecha <= periodoActual.FechaFinEval2) examen = "2";
            if (data.fecha >= periodoActual.FechaIniEval3 && data.fecha <= periodoActual.FechaFinEval3) examen = "M";
            var query =
                from horario in contextSAAC.TBL_HORARIO
                where horario.strExamen == examen
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in contextSAAC.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where horario.intDia == data.dia && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && horario.chTipo == data.tipoSemana
                select new
                {
                    idHorario = horario.intIdHorario,
                    fecha = horario.dtFecha,
                    horaInicio = horario.dtHoraInicio,
                    horaFin = horario.dtHoraFin,
                    tipoHorario = horario.chTipo,
                    numRegistrados = curso.intNumRegistrados,
                    tipoCurso = curso.strTipoCurso,
                    idLugar = lugar.intIdLugarEspol,
                    descripcionLugar = lugar.strDescripcion,
                    latitud = lugar.strLatitud,
                    longitud = lugar.strLongitud,
                    tipoLugar = lugar.strTipo
                };

            var query2 =
                from horario in contextSAAC.TBL_HORARIO_CONTENIDO
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in contextSAAC.TBL_LUGAR_ESPOL on horario.intIdLugarEspol equals lugar.intIdLugarEspol
                where data.fecha == horario.dtFecha && horario.strEstadoRecuperacion == "AP" && horario.intIdLugarEspol != null
                select new
                {
                    idHorario = horario.intIdHorarioContenido,
                    fecha = horario.dtFecha,
                    horaInicio = horario.tsHoraInicioPlanificado,
                    horaFin = horario.tsHoraFinPlanificado,
                    tipoHorario = horario.strTipo,
                    numRegistrados = curso.intNumRegistrados,
                    tipoCurso = curso.strTipoCurso,
                    idLugar = lugar.intIdLugarEspol,
                    descripcionLugar = lugar.strDescripcion,
                    latitud = lugar.strLatitud,
                    longitud = lugar.strLongitud,
                    tipoLugar = lugar.strTipo
                };
            return query.Concat(query2);
        }

        // Cantidad de estudiantes/Cantidad registrados en periodo
        public int cantRegistrados(DateTime? fecha)
        {
            if (fecha == null) return 0;

            var periodo = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();

            var query =
                from persona in contextSAAC.TBL_PERSONA
                join historia in contextSAAC.HISTORIA_ANIO on persona.strCodEstudiante equals historia.strCodEstudiante
                join curso in contextSAAC.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodo.intIdPeriodoAcademico
                group persona by persona.intIdPersona into grupo
                select new {
                    grupo,
                    idPersona = grupo.Key,
                    cantidad = grupo.Count()
                };
            return query.Count();
        }

        // Top 3 bloques con más personas
        public IQueryable top3Bloques(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(contextSAAC).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";

            var query =
                from lugar in contextSAAC.TBL_LUGAR_ESPOL
                join places in contextSAAC.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals places.intIdLugarEspol
                join horario in contextSAAC.TBL_HORARIO on lugar.intIdLugarEspol equals horario.intIdAula
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && lugar.strTipo == "A"
                //&& lugar.strEstado == "V" //Descomentar en caso de que se necesite solo tener en cuenta los lugares vigentes
                && places.strTipo == "E"
                && curso.strEstado == "A"
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.dtHoraInicio <= fecha.TimeOfDay
                && horario.dtHoraFin > fecha.TimeOfDay
                group new { places, curso } by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    nombre = grupo.Select(x => x.places.strDescripcion).First(),
                    numPersonas = grupo.Sum(x => x.curso.intNumRegistrados)
                };
            return query.OrderByDescending(x => x.numPersonas).Take(3);
        }

        //Cantidad de bloques totales
        public int cantBloquesTotales()
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V").Count();
        }

        //Cantidad de bloques usados al momento
        public int cantBloquesUsadosFecha(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(contextSAAC).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";

            var query =
                from lugar in contextSAAC.TBL_LUGAR_ESPOL
                join places in contextSAAC.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals places.intIdLugarEspol
                join horario in contextSAAC.TBL_HORARIO on lugar.intIdLugarEspol equals horario.intIdAula
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && lugar.strTipo == "A"
                && places.strTipo == "E"
                && curso.strEstado == "A"
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.dtHoraInicio <= fecha.TimeOfDay
                && horario.dtHoraFin > fecha.TimeOfDay
                group curso by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados)
                };
            return query.Count(x => x.lugar > 0);
        }

        // Prom. de personas por bloque
        public double promedioPersonasPorBloque(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(contextSAAC).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";

            var query =
                from lugar in contextSAAC.TBL_LUGAR_ESPOL
                join places in contextSAAC.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals places.intIdLugarEspol
                join horario in contextSAAC.TBL_HORARIO on lugar.intIdLugarEspol equals horario.intIdAula
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && lugar.strTipo == "A"
                && places.strTipo == "E"
                && curso.strEstado == "A"
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.dtHoraInicio <= fecha.TimeOfDay
                && horario.dtHoraFin > fecha.TimeOfDay
                group curso by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados)
                };
            if (query.Average(x => x.numPersonas) == null) return 0;
            else return query.Average(x => x.numPersonas).Value;
        }

        // Cantidad de lugares usados (Aulas, labs, canchas)
        public int cantLugaresUsadosFecha(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(contextSAAC).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";

            var query =
                from lugar in contextSAAC.TBL_LUGAR_ESPOL
                join horario in contextSAAC.TBL_HORARIO on lugar.intIdLugarEspol equals horario.intIdAula
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && lugar.strTipo == "A"
                && curso.strEstado == "A"
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.dtHoraInicio <= fecha.TimeOfDay
                && horario.dtHoraFin > fecha.TimeOfDay
                group curso by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados)
                };

            return query.Count(x => x.lugar > 0);
        }

        // Promedio personas por lugar (Aulas, labs, canchas)
        public double promedioPersonasPorLugar(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(contextSAAC).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = contextSAAC.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";

            var query =
                from lugar in contextSAAC.TBL_LUGAR_ESPOL
                join horario in contextSAAC.TBL_HORARIO on lugar.intIdLugarEspol equals horario.intIdAula
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && lugar.strTipo == "A"
                && curso.strEstado == "A"
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.dtHoraInicio <= fecha.TimeOfDay
                && horario.dtHoraFin > fecha.TimeOfDay
                group curso by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados)
                };
            if (query.Average(x => x.numPersonas) == null) return 0;
            else return query.Average(x => x.numPersonas).Value;
        }

        //Vale
        [HttpPost("EstadisticasMapa")]
        public RetornoEstadisticas estadisticasMapa([FromBody] InDatosEstadisticas data)
        {
            return new RetornoEstadisticas
            {
                numRegistrados = cantRegistrados(data.fecha),
                cantBloquesUsados = cantBloquesUsadosFecha(data.fecha),
                cantBloquesTotales = cantBloquesTotales(),
                cantLugaresUsados = cantLugaresUsadosFecha(data.fecha),
                promPersonasPorBloque = promedioPersonasPorBloque(data.fecha),
                promPersonasPorLugar = promedioPersonasPorLugar(data.fecha),
                top3Bloques = top3Bloques(data.fecha),
            };
        }

        //Obtiene los datos de las personas cuyos nombres coinciden con el criterio de búsqueda. Recibe nombres y apellidos por separado.
        [HttpPost("personasPorNombreYApellido")]
        public IQueryable personasPorNombreYApellido([FromBody] NombreApellido data)
        {
            var query = contextSAAC.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");

            if (data.nombres != "")
            {
                query = query.Where(persona => (persona.strNombres != null && persona.strNombres.Contains(data.nombres.ToUpper()) && persona.strEstadoPersona == "A"));
            }

            if (data.apellidos != "")
            {
                query = query.Where(persona => (persona.strApellidos != null && persona.strApellidos.Contains(data.apellidos.ToUpper()) && persona.strEstadoPersona == "A"));
            }

            return query;
        }

        //Obtiene los datos de las personas cuyos nombres coinciden con el criterio de búsqueda. Solo recibe nombre y apellido en un string, ya sea completo o no.
        [HttpPost("personasPorNombre")]
        public IActionResult personasPorNombreCompleto([FromBody] NombreCompleto data)
        {
            if (data.nombre != "" && data.nombre != null)
            {
                var query = contextSAAC.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");
                if (data.nombre.Contains(" "))
                {
                    var separado = data.nombre.Split(" ");
                    foreach(var palabra in separado)
                    {
                        query = query.Where(persona => persona.strNombres != null && persona.strApellidos != null &&
                        (persona.strNombres.Trim().Contains(palabra.ToUpper()) || persona.strApellidos.Trim().Contains(palabra.ToUpper())));
                    }
                }
                else
                {
                    query = query.Where(persona => persona.strNombres != null && persona.strApellidos != null &&
                    (persona.strNombres.Trim().Contains(data.nombre.ToUpper()) || persona.strApellidos.Trim().Contains(data.nombre.ToUpper())));
                }
                return Ok(query);
            }

            return Ok(new List<CdaPersona>());
        }

        //Obtiene los datos de las personas que se encuentran cursando una carrera.
        [HttpPost("estudiantesPorCarrera")]
        public IQueryable estudiantesPorCarrera([FromBody] IdPrograma data)
        {
            var query =
                from carrera in contextSAAC.CARRERA_ESTUDIANTE
                join programa in contextSAAC.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera equals programa.strCodCarrera
                join persona in contextSAAC.TBL_PERSONA on carrera.strCodEstudiante equals persona.strCodEstudiante
                where persona.strCodEstudiante != null && programa.intIdPrograma == data.idPrograma
                && carrera.strEstadoCarrEstud == "A"
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    //identificacion = persona.strNumeroIdentificacion,
                    //tipoIdentificacion = persona.strTipoIdentificacion,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct();
        }

        //Obtiene los estudiantes cuyas carreras pertenecen a cierta facultad.
        [HttpPost("estudiantesPorFacultad")]
        public IQueryable estudiantesPorFacultad([FromBody] IdFacultad data)
        {
            var query =
                from persona in contextSAAC.TBL_PERSONA
                join carrera in contextSAAC.CARRERA_ESTUDIANTE on persona.strCodEstudiante equals carrera.strCodEstudiante
                join programa in contextSAAC.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera equals programa.strCodCarrera
                where programa.intIdUnidadEjecuta == data.idFacultad
                && carrera.strEstadoCarrEstud == "A"
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    //identificacion = persona.strNumeroIdentificacion,
                    //tipoIdentificacion = persona.strTipoIdentificacion,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct();
        }

        //Obtiene todos los estudiantes que se encuentran cursando una materia específica.
        [HttpPost("estudiantesPorMateria")]
        public IQueryable estudiantesPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in contextSAAC.TBL_MATERIA
                join historia in contextSAAC.HISTORIA_ANIO on materia.strCodigoMateria equals historia.strCodMateria
                join persona in contextSAAC.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where historia.strAnio == periodoActual.strAnio && historia.strTermino == periodoActual.strTermino && materia.intIdMateria == data.idMateria
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    //identificacion = persona.strNumeroIdentificacion,
                    //tipoIdentificacion = persona.strTipoIdentificacion,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct();
        }

        //Vale
        [HttpPost("estudiantesPorCurso")]
        public IQueryable estudiantesPorCurso([FromBody] IdCurso data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in contextSAAC.TBL_CURSO
                join historia in contextSAAC.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in contextSAAC.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.strEstado == "A" && curso.intIdCurso == data.idCurso && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return query.Distinct();
        }

        [HttpPost("materiasPorProfesor")]
        public IQueryable materiasPorProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();

            var query =
                from materia in contextSAAC.TBL_MATERIA
                join curso in contextSAAC.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where curso.strEstado == "A" && persona.intIdPersona == data.idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };

            return query.Distinct();
        }

        //Vale
        [HttpPost("materiasPorFacultad")]
        public IQueryable materiasPorFacultad([FromBody] IdFacultad data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in contextSAAC.TBL_MATERIA
                join unidad in contextSAAC.TBL_UNIDAD on materia.intIdUnidad equals unidad.intIdUnidad
                join curso in contextSAAC.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                where unidad.intIdUnidad == data.idFacultad && curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };
            return query.Distinct();
        }

        [HttpPost("esProfesor")]
        public Dictionary<string, bool> wsEsProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in contextSAAC.TBL_CURSO
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.intIdProfesor == data.idPersona
                select new
                {
                    idCurso = curso.intIdCurso
                };

            Dictionary<string, bool> resultado = new Dictionary<string, bool>();
            resultado.Add("resultado", query.ToArray().Length != 0);
            return resultado;
        }

        //Vale
        [HttpPost("profesoresPorFacultad")]
        public IQueryable profesoresPorFacultad([FromBody] IdFacultad data)
        {
            //Se va a tomar en cuenta que enseñen materias que pertenezcan a la facultad.
            //Si enseña al menos una de la facultad, se lo tomará en cuenta.
            var periodoActual = this.periodoActual();
            var query =
                from persona in contextSAAC.TBL_PERSONA
                join curso in contextSAAC.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                join materia in contextSAAC.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && materia.intIdUnidad == data.idFacultad
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct();
        }

        //Vale
        [HttpPost("profesoresPorMateria")]
        public IQueryable profesoresPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in contextSAAC.TBL_MATERIA
                join curso in contextSAAC.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == data.idMateria && persona.strEstadoPersona == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct();
        }

        //Vale
        [HttpPost("directivoFacultad")]
        public IQueryable directivoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in contextSAAC.TBL_UNIDAD
                join persona in contextSAAC.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        //Vale
        [HttpPost("subdecanoFacultad")]
        public IQueryable subdecanoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in contextSAAC.TBL_UNIDAD
                join persona in contextSAAC.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        [HttpPost("LugarPadre")]
        public object idLugarPadre([FromBody] IdLugar data)
        {
            CdaLugar lugar = contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == data.idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return new { idPadre = contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre).FirstOrDefault().intIdLugarEspol };
            }

            return new { idPadre = -1 };
        }

    }
}