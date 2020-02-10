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

        [HttpPost("datosFecha")]
        public Object datosFecha([FromBody] InDataFecha data)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            return new
            {
                tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fecha }),
                examen = periodoController.tipoExamen(data.fecha),
                dia = (int)data.fecha.DayOfWeek
            };
        }

        [HttpPost("datosMapa")]
        public IQueryable datosMapa([FromBody] InDataFecha data)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(data.fecha);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fecha });
            string examen = periodoController.tipoExamen(data.fecha);
            int dia = (int)data.fecha.DayOfWeek;
            var query =
                from horario in contextSAAC.TBL_HORARIO
                where horario.strExamen == examen
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in contextSAAC.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where horario.intDia == dia
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico 
                && horario.chTipo == tipoSemana.tipo
                && horario.tsHoraInicio <= data.fecha.TimeOfDay 
                && horario.tsHoraFin > data.fecha.TimeOfDay
                select new
                {
                    idHorario = horario.intIdHorario,
                    fecha = horario.dtFecha,
                    horaInicio = horario.tsHoraInicio,
                    horaFin = horario.tsHoraFin,
                    tipoHorario = horario.chTipo,
                    numRegistrados = curso.intNumRegistrados + 1, // Por el profesor
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
                where data.fecha == horario.dtFecha 
                && horario.strEstadoRecuperacion == "AP" 
                && horario.intIdLugarEspol != null
                && horario.tsHoraInicio <= data.fecha.TimeOfDay 
                && horario.tsHoraFin > data.fecha.TimeOfDay
                select new
                {
                    idHorario = horario.intIdHorarioContenido,
                    fecha = horario.dtFecha,
                    horaInicio = horario.tsHoraInicioPlanificado,
                    horaFin = horario.tsHoraFinPlanificado,
                    tipoHorario = horario.strTipo,
                    numRegistrados = curso.intNumRegistrados + 1, // Por el profesor
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
        public int cantRegistrados(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);

            var query =
                from persona in contextSAAC.TBL_PERSONA
                join historia in contextSAAC.HISTORIA_ANIO on persona.strCodEstudiante equals historia.strCodEstudiante
                join curso in contextSAAC.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                group persona by persona.intIdPersona into grupo
                select new {
                    grupo,
                    idPersona = grupo.Key,
                    cantidad = grupo.Count()
                };


            return query.Count();
        }

        // Cantidad de profesores con un curso este semestre
        public int cantProfesoresPeriodo(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);

            var query =
                from persona in contextSAAC.TBL_PERSONA
                join curso in contextSAAC.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.strEstado == "A" && persona.strEstadoPersona == "A"
                select new
                {
                    persona
                };
            return query.Distinct().Count();
        }

        // Cantidad de Personas por bloque
        public IQueryable CantidadDePersonasPorBloque(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);

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
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
                group new { places, curso } by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    nombre = grupo.Select(x => x.places.strDescripcion).First(),
                    numPersonas = grupo.Sum(x => x.curso.intNumRegistrados + 1) // +1 por el profesor
                };
            return query.OrderByDescending(x => x.numPersonas);
        }

        //Cantidad de bloques totales
        public int cantBloquesTotales()
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V").Count();
        }

        //Cantidad de bloques usados al momento
        public int cantBloquesUsadosFecha(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            if (periodoActual == null) return 0;
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);

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
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
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
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            if (periodoActual == null) return 0;
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);

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
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
                group curso by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados + 1) // +1 por el profesor
                };
            if (query.Average(x => x.numPersonas) == null) return 0;
            else return query.Average(x => x.numPersonas).Value;
        }

        public int cantLugaresTotales()
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strEstado == "V" && x.strTipo == "A").Count();
        }

        // Cantidad de lugares usados (Aulas, labs, canchas)
        public int cantLugaresUsadosFecha(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            if (periodoActual == null) return 0;
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);

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
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
                group curso by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados + 1) // +1 por el profesor
                };

            return query.Count(x => x.lugar > 0);
        }

        // Total personas en fecha y hora
        public int totalPersonasMomento(DateTime fecha)
        {
            var cursoController = new CursoController(contextSAAC);
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            if (periodoActual == null) return 0;
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);
            
            var query =
                from horario in contextSAAC.TBL_HORARIO
                join curso in contextSAAC.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && horario.intDia == (int)fecha.DayOfWeek
                && horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
                group curso by curso.intIdPeriodo into grupo
                select new
                {
                    idPeriodo = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados + 1) // +1 por el profesor
                };

            return query.Sum(x => x.numPersonas).Value;
        }

        // Promedio personas por lugar (Aulas, labs, canchas)
        public double promedioPersonasPorLugar(DateTime fecha)
        {
            var periodoController = new PeriodoAcademicoController(contextSAAC);
            var periodoActual = periodoController.GetPeriodoFecha(fecha);
            if (periodoActual == null) return 0;
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = fecha });
            string examen = periodoController.tipoExamen(fecha);

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
                && horario.tsHoraInicio <= fecha.TimeOfDay
                && horario.tsHoraFin > fecha.TimeOfDay
                group curso by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    numPersonas = grupo.Sum(x => x.intNumRegistrados + 1) // +1 por el profesor
                };
            if (query.Average(x => x.numPersonas) == null) return 0;
            else return query.Average(x => x.numPersonas).Value;
        }

        // Calcula las Estadísticas y las retorna
        [HttpPost("EstadisticasMapa")]
        public RetornoEstadisticas estadisticasMapa([FromBody] InDataFecha data)
        {
            return new RetornoEstadisticas
            {
                numRegistrados = cantRegistrados(data.fecha) + cantProfesoresPeriodo(data.fecha),
                cantBloquesUsados = cantBloquesUsadosFecha(data.fecha),
                cantBloquesTotales = cantBloquesTotales(),
                cantLugares = cantLugaresTotales(),
                cantLugaresUsados = cantLugaresUsadosFecha(data.fecha),
                promPersonasPorBloque = promedioPersonasPorBloque(data.fecha),
                promPersonasPorLugar = promedioPersonasPorLugar(data.fecha),
                totalPersonasMomento = totalPersonasMomento(data.fecha),
                cantPersonasPorBloque = CantidadDePersonasPorBloque(data.fecha)
            };
        }

    }
}