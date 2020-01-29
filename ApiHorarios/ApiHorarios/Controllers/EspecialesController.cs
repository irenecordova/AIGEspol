using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        //[HttpGet("periodoActual")]
        public CdaPeriodoAcademico periodoActual()
        {
            return contextSAAC.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
        }

        public class InDatosMapa
        {
            public DateTime fecha { get; set; }
            public int dia { get; set; }
            public string tipoSemana { get; set; }
        }
        [HttpPost("datosMapa")]
        public IQueryable datosMapa([FromBody] InDatosMapa data)
        //[HttpGet("datosMapa/{dia}", Name = "horarios_por_dia")]
        //public IQueryable datosMapa(int dia)
        {
            CdaPeriodoAcademico periodoActual = this.periodoActual();
            string examen = null;
            if (data.fecha >= periodoActual.FechaIniEval1 && data.fecha <= periodoActual.FechaFinEval1) examen = "1";
            if (data.fecha >= periodoActual.FechaIniEval2 && data.fecha <= periodoActual.FechaFinEval2) examen = "2";
            if (data.fecha >= periodoActual.FechaIniEval3 && data.fecha <= periodoActual.FechaFinEval3) examen = "M";
            //var fechaEvaluacion = "";
            //if (examen == "1" || examen == "2" || examen == "M") fechaEvaluacion = data.fecha.Date.ToString();
            var query =
                from horario in contextSAAC.TBL_HORARIO
                where horario.strExamen == examen //&& horario.dtFecha.Value.Date.ToString() == fechaEvaluacion
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
        [HttpPost("top3Bloques")]
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
                    nombre = grupo.Select(x => x.places.strDescripcion),
                    numPersonas = grupo.Sum(x => x.curso.intNumRegistrados)
                };
            return query.OrderBy(x => x.numPersonas).Take(3);
        }

        // Cantidad de bloques usados/Cantidad de bloques totales
        public int cantBloquesTotales()
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V").Count();
        }

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

        public class InDatosEstadisticas
        {
            public DateTime fecha { get; set; }
            public Nullable<int> dia { get; set; }
            public string tipoSemana { get; set; }
        }
        public class RetornoEstadisticas
        {
            public int numRegistrados { get; set; }
            public int cantBloquesUsados { get; set; }
            public int cantBloquesTotales { get; set; }
            public int cantLugaresUsados { get; set; }
            public double promPersonasPorLugar { get; set; }
            public double promPersonasPorBloque { get; set; }
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
            };
        }

        //Vale
        public class NombreApellido
        {
            public string nombres { get; set; }
            public string apellidos { get; set; }
        }
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

        public class NombreCompleto
        {
            public string nombre { get; set; }
        }
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

        [HttpGet("estadospersonas")]
        public IQueryable estadosPersonas()
        {
            var query =
                from persona in contextSAAC.TBL_PERSONA
                group persona by persona.strEstadoPersona into grupo
                select new
                {
                    estado = grupo.Key,
                    veces = grupo.Count(x => x.intIdPersona > 0)
                };
            return query;
        }

        public class IdPrograma
        {
            public int idPrograma { get; set; }
        }
        //Vale
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

        public class IdFacultad
        {
            public int idFacultad { get; set; }
        }
        //Vale
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

        public class IdMateria
        {
            public int idMateria { get; set; }
        }
        //Vale
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

        public class IdCurso
        {
            public int idCurso { get; set; }
        }
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
                    //identificacion = persona.strNumeroIdentificacion,
                    //tipoIdentificacion = persona.strTipoIdentificacion,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return query.Distinct();
        }

        public IQueryable sacarHorarioEstudiante(int idPersona, DateTime fecha)
        {
            var periodoActual = this.periodoActual();
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaFinEval2 && fecha <= periodoActual.FechaIniMejoramiento) examen = "2";
            else if (fecha >= periodoActual.FechaIniMejoramiento && fecha <= periodoActual.FechaFinMejoramiento) examen = "M";
            var query =
                from historia in contextSAAC.HISTORIA_ANIO
                join persona in contextSAAC.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in contextSAAC.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in contextSAAC.TBL_HORARIO on historia.intIdCurso equals horario.intIdCurso
                join materia in contextSAAC.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                      && persona.intIdPersona == idPersona
                      && horario.intDia != 7
                      && horario.strExamen == examen
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            int numDia = (int)fecha.DayOfWeek;
            var fechaInicioSemana = fecha.AddDays(-(numDia - 1)).Date;
            var fechaFinSemana = fecha.AddDays(8 - numDia).Date;

            if (examen != null) {
                query = query.Where(x => fechaInicioSemana <= x.horarioFecha && x.horarioFecha < fechaFinSemana);
            }

            var query2 =
                from historia in contextSAAC.HISTORIA_ANIO
                join persona in contextSAAC.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in contextSAAC.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in contextSAAC.TBL_HORARIO_CONTENIDO on curso.intIdCurso equals horario.intIdCurso
                join materia in contextSAAC.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
                where fechaInicioSemana <= horario.dtFecha && horario.dtFecha < fechaFinSemana 
                && horario.strEstadoRecuperacion == "AP" 
                && horario.intIdLugarEspol != null
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && persona.intIdPersona == idPersona
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = (Nullable<short>)horario.dtFecha.Value.DayOfWeek,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.tsHoraInicioPlanificado,
                    horarioHoraFin = horario.tsHoraFinPlanificado,
                    horarioTipo = horario.strTipo,
                };

            return query.Concat(query2);
        }

        public IQueryable sacarHorarioProfesor(int idPersona, DateTime fecha)
        {
            var periodoActual = this.periodoActual();
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaFinEval2 && fecha <= periodoActual.FechaIniMejoramiento) examen = "2";
            else if (fecha >= periodoActual.FechaIniMejoramiento && fecha <= periodoActual.FechaFinMejoramiento) examen = "M";
            var query =
                from curso in contextSAAC.TBL_CURSO
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in contextSAAC.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                join materia in contextSAAC.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                      && persona.intIdPersona == idPersona
                      && horario.intDia != 7
                      && horario.strExamen == examen
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            int numDia = (int)fecha.DayOfWeek;
            var fechaInicioSemana = fecha.AddDays(-(numDia - 1)).Date;
            var fechaFinSemana = fecha.AddDays(8 - numDia).Date;

            if (examen != null)
            {
                query = query.Where(x => fechaInicioSemana <= x.horarioFecha && x.horarioFecha < fechaFinSemana);
            }

            var query2 =
                from curso in contextSAAC.TBL_CURSO
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in contextSAAC.TBL_HORARIO_CONTENIDO on curso.intIdCurso equals horario.intIdCurso
                join materia in contextSAAC.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where fechaInicioSemana <= horario.dtFecha && horario.dtFecha < fechaFinSemana
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && persona.intIdPersona == idPersona
                && horario.strEstadoRecuperacion == "AP"
                && horario.intIdLugarEspol != null
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && persona.intIdPersona == idPersona
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = (Nullable<short>)horario.dtFecha.Value.DayOfWeek,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.tsHoraInicioPlanificado,
                    horarioHoraFin = horario.tsHoraFinPlanificado,
                    horarioTipo = horario.strTipo,
                };

            return query.Concat(query2).Distinct();
        }

        public bool esProfesor(int idPersona)
        {
            Console.WriteLine("Entro");
            var periodoActual = this.periodoActual();
            var query = contextSAAC.TBL_CURSO.Where(curso => curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico &&
            (curso.intIdProfesor == idPersona || curso.intIdProfesor1 == idPersona || curso.intIdProfesor2 == idPersona ||
            curso.intIdProfesor3 == idPersona || curso.intIdProfesor4 == idPersona || curso.intIdProfesor5 == idPersona));
            return query.ToList().Count() > 0;
        }

        public class IdPersona
        {
            public int idPersona { get; set; }
        }
        
        public class InDatosHorarios
        {
            public DateTime fecha { get; set; }
            public List<int> idsPersonas { get; set; }
        }
        
        public class IdsPersonas
        {
            public List<int> idsPersonas { get; set; }
        }
        [HttpPost("horariosPersonas")]
        public List<IQueryable> horariosPersonas([FromBody] InDatosHorarios data)
        {
            var periodoActual = this.periodoActual();

            List<IQueryable> lista = new List<IQueryable>();

            foreach (int idPersona in data.idsPersonas)
            {
                if (esProfesor(idPersona))
                {
                    lista.Add(sacarHorarioProfesor(idPersona,data.fecha));
                }
                else
                {
                    lista.Add(sacarHorarioEstudiante(idPersona, data.fecha));
                }

            };
            return lista;
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
        [HttpPost("profesoresPorMateria")]
        public IQueryable profesoresPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in contextSAAC.TBL_MATERIA
                join curso in contextSAAC.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == data.idMateria && persona.strEstadoPersona == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                //group persona by persona.intIdPersona into grupo
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

            //if (query.ToArray().Length == 0) return NotFound();
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
                    //matricula = persona.strCodEstudiante,
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
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        /*
        [HttpPost("coordinadoresMateriasFacultad")]
        public IActionResult coordinadoresMateriasFacultad ([FromForm] int idFacultad)
        {
            var query =
                from unidad in contextSAAC.TBL_UNIDAD
                join persona in contextSAAC.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdUnidad == idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return Ok(JsonConvert.SerializeObject(query.ToList()));
        }
        */

        /*
        [HttpPost("coordinadorCarrera")]
        public IQueryable coordinadorMateria([FromBody] IdPrograma data)
        {
            var query =
                from programa in contextSAAC.TBL_PROGRAMA_ACADEMICO
                join persona in contextSAAC.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
                where programa.intIdPrograma == data.idPrograma
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }*/

        /*
        [HttpPost("coordinadoresFacultad")]
        public IQueryable coordinadoresFacultad([FromBody] IdFacultad data)
        {
            var query =
                from programa in contextSAAC.TBL_PROGRAMA_ACADEMICO
                join unidad in contextSAAC.TBL_UNIDAD on programa.intIdUnidadEjecuta equals unidad.intIdUnidad
                join persona in contextSAAC.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }
        */

        public class IdLugar
        {
            public int idLugar { get; set; }
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

        //Vale
        public IQueryable sacarCursosEstudiante(int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in contextSAAC.TBL_CURSO
                join historia in contextSAAC.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in contextSAAC.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join materia in contextSAAC.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        //Vale
        public IQueryable sacarCursosProfesor(int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in contextSAAC.TBL_CURSO
                join persona in contextSAAC.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join materia in contextSAAC.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        //Vale
        [HttpPost("cursosEstudiante")]
        public IQueryable cursosEstudiante([FromBody] IdPersona data)
        {       
            return sacarCursosEstudiante(data.idPersona);
        }

        //Vale
        [HttpPost("cursosProfesor")]
        public IQueryable cursosProfesor([FromBody] IdPersona data)
        {
            return sacarCursosProfesor(data.idPersona);
        }

        [HttpPost("cursosRelacionados")]
        //Vale
        public IQueryable cursosRelacionados([FromBody] IdPersona data)
        {
            if (esProfesor(data.idPersona)) return sacarCursosProfesor(data.idPersona);
            else return sacarCursosEstudiante(data.idPersona);
        }
    }
}