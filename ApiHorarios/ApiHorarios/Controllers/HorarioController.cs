using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using ApiHorarios.DataRepresentationsIN;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public HorarioController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaHorario> Get()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select horario;
            return query.ToList();
        }

        [HttpGet("dia/{dia}")]
        public IEnumerable<CdaHorario> GetByDia(int dia)
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && horario.intDia == dia
                select horario;
            return query.ToList();
        }

        public IQueryable sacarHorarioEstudiante(int idPersona, DateTime fecha)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaFinEval2 && fecha <= periodoActual.FechaIniMejoramiento) examen = "2";
            else if (fecha >= periodoActual.FechaIniMejoramiento && fecha <= periodoActual.FechaFinMejoramiento) examen = "M";
            var query =
                from historia in context.HISTORIA_ANIO
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in context.TBL_HORARIO on historia.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
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
                    horarioHoraInicio = horario.tsHoraInicio,
                    horarioHoraFin = horario.tsHoraFin,
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
                from historia in context.HISTORIA_ANIO
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in context.TBL_HORARIO_CONTENIDO on curso.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
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
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaFinEval2 && fecha <= periodoActual.FechaIniMejoramiento) examen = "2";
            else if (fecha >= periodoActual.FechaIniMejoramiento && fecha <= periodoActual.FechaFinMejoramiento) examen = "M";
            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
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
                    horarioHoraInicio = horario.tsHoraInicio,
                    horarioHoraFin = horario.tsHoraFin,
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
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in context.TBL_HORARIO_CONTENIDO on curso.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
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

        [HttpPost("personas")]
        public List<IQueryable> horariosPersonas([FromBody] InDatosHorarios data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();

            List<IQueryable> lista = new List<IQueryable>();

            foreach (int idPersona in data.idsPersonas)
            {
                if (new PersonaController(context).esProfesor(idPersona))
                {
                    lista.Add(sacarHorarioProfesor(idPersona, data.fecha));
                }
                else
                {
                    lista.Add(sacarHorarioEstudiante(idPersona, data.fecha));
                }

            };
            return lista;
        }
    }
}