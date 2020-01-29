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

        [HttpPost("datosMapa")]
        public IQueryable datosMapa([FromBody] InDatosMapa data)
        {
            CdaPeriodoAcademico periodoActual = new PeriodoAcademicoController(contextSAAC).periodoActual();
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

        // Calcula las Estadísticas y las retorna
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

    }
}