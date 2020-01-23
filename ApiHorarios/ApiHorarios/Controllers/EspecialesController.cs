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
        private readonly CdaDbContextDB2SAAC context;

        public EspecialesController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }
        
        //[HttpGet("periodoActual")]
        public CdaPeriodoAcademico periodoActual()
        {
            return context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
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
                from horario in context.TBL_HORARIO
                where horario.strExamen == examen //&& horario.dtFecha.Value.Date.ToString() == fechaEvaluacion
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
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
            return query;
        }

        // Cantidad de estudiantes/Cantidad registrados en periodo
        public int cantRegistrados(DateTime? fecha)
        {
            if (fecha == null) return 0;

            var periodo = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();

            var query =
                from persona in context.TBL_PERSONA
                join historia in context.HISTORIA_ANIO on persona.strCodEstudiante equals historia.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
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

        // Cantidad de bloques usados/Cantidad de bloques totales
        public int cantBloquesTotales()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V").Count();
        }

        public int cantBloquesUsadosFecha(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(context).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha <= periodoActual.FechaFinEval1) examen = "1";
            if (fecha >= periodoActual.FechaIniEval2 && fecha <= periodoActual.FechaFinEval2) examen = "2";
            if (fecha >= periodoActual.FechaIniEval3 && fecha <= periodoActual.FechaFinEval3) examen = "M";
            var query =
                from lugar in context.TBL_LUGAR_ESPOL
                join curso in context.TBL_CURSO on lugar.intIdLugarEspol equals curso.intIdCurso
                join places in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals places.intIdLugarEspol
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico 
                //&& lugar.strEstado == "V"
                && curso.strEstado == "A"
                //&& horario.strExamen == examen
                && horario.chTipo == tipoSemana.tipo
                //&& horario.dtHoraInicio <= fecha.TimeOfDay
                //&& horario.dtHoraFin > fecha.TimeOfDay
                //&& horario.dtHoraInicio.Hour <= fecha.Hour
                //&& horario.dtHoraFin.Minute > fecha.Minute
                //&& horario.dtHoraFin.Hour > fecha.Minute
                group places by places.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key
                };
            return query.ToList().Count();
        }

        // Prom. de personas por bloque
        public double promedioPersonasPorBloque(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(context).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            var query =
                from lugar in context.TBL_LUGAR_ESPOL
                join curso in context.TBL_CURSO on lugar.intIdLugarEspol equals curso.intIdCurso
                join places in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals places.intIdLugarEspol
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && lugar.strEstado == "V"
                && curso.strEstado == "A" 
                && horario.chTipo == tipoSemana.tipo
                //&& horario.dtHoraInicio.Minute <= fecha.Minute
                //&& horario.dtHoraInicio.Hour <= fecha.Hour
                //&& horario.dtHoraFin.Minute > fecha.Minute
                //&& horario.dtHoraFin.Hour > fecha.Minute
                group curso by curso.intIdBloque into grupo
                select new
                {
                    lugar = grupo.Key,
                    suma = grupo.Sum(x => x.intNumRegistrados)
                };
            if (query.Average(x => x.suma) == null) return 0;
            else return query.Average(x => x.suma).Value;
        }

        // Cantidad de lugares usados (Aulas, labs, canchas)
        public int cantLugaresUsadosFecha(DateTime fecha)
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            var query =
                from lugar in context.TBL_LUGAR_ESPOL
                join curso in context.TBL_CURSO on lugar.intIdLugarEspol equals curso.intIdCurso
                where lugar.strTipo == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && lugar.strEstado == "V"
                && curso.strEstado == "A"
                group lugar by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key
                };

            return query.ToList().Count();
        }

        // Promedio personas por lugar (Aulas, labs, canchas)
        public double promedioPersonasPorLugar(DateTime fecha)
        {
            var tipoSemana = new PeriodoAcademicoController(context).getTipoSemanaEnPeriodo(new PeriodoAcademicoController.DataFecha { fecha = fecha });
            var periodoActual = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
            if (periodoActual == null) return 0;
            var query =
                from lugar in context.TBL_LUGAR_ESPOL
                join curso in context.TBL_CURSO on lugar.intIdLugarEspol equals curso.intIdCurso
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && lugar.strEstado == "V"
                && curso.strEstado == "A" && horario.chTipo == tipoSemana.tipo
                //&& horario.dtHoraInicio.Minute <= fecha.Minute
                //&& horario.dtHoraInicio.Hour <= fecha.Hour
                //&& horario.dtHoraFin.Minute > fecha.Minute
                //&& horario.dtHoraFin.Hour > fecha.Minute
                group curso by lugar.intIdLugarEspol into grupo
                select new
                {
                    lugar = grupo.Key,
                    suma = grupo.Sum(x => x.intNumRegistrados)
                };

            if (query.Average(x => x.suma) == null) return 0;
            else return query.Average(x => x.suma).Value;
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

        public class NombreApellido
        {
            public string nombres { get; set; }
            public string apellidos { get; set; }
        }
        [HttpPost("personasPorNombreYApellido")]
        public IQueryable personasPorNombreYApellido([FromBody] NombreApellido data)
        {
            var query = context.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");

            if (data.nombres != "")
            {
                query = query.Where(persona => (persona.strNombres != null && persona.strNombres.Contains(data.nombres.ToUpper())));
            }

            if (data.apellidos != "")
            {
                query = query.Where(persona => (persona.strApellidos != null && persona.strApellidos.Contains(data.apellidos.ToUpper())));
            }

            return query;
        }

        public class IdPrograma
        {
            public int idPrograma { get; set; }
        }
        [HttpPost("estudiantesPorCarrera")]
        public IQueryable estudiantesPorCarrera([FromBody] IdPrograma data)
        {
            var query =
                from carrera in context.CARRERA_ESTUDIANTE
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                join estudiante in context.TBL_PERSONA on carrera.strCodEstudiante.Trim() equals estudiante.strCodEstudiante.Trim()
                where estudiante.strCodEstudiante != null && programa.intIdPrograma == data.idPrograma
                select new
                {
                    idPersona = estudiante.intIdPersona,
                    matricula = estudiante.strCodEstudiante,
                    nombres = estudiante.strNombres,
                    apellidos = estudiante.strApellidos,
                    email = estudiante.strEmail
                };

            return query;
        }

        public class IdFacultad
        {
            public int idFacultad { get; set; }
        }
        [HttpPost("estudiantesPorFacultad")]
        public IQueryable estudiantesPorFacultad([FromForm] IdFacultad data)
        {
            var query =
                from persona in context.TBL_PERSONA
                join carrera in context.CARRERA_ESTUDIANTE on persona.strCodEstudiante.Trim() equals carrera.strCodEstudiante.Trim()
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                where persona.strCodEstudiante != null && programa.intIdUnidadEjecuta == data.idFacultad
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

        public class IdMateria
        {
            public int idMateria { get; set; }
        }
        [HttpPost("estudiantesPorMateria")]
        public IQueryable estudiantesPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join historia in context.HISTORIA_ANIO on materia.strCodigoMateria equals historia.strCodMateria
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where historia.strAnio == periodoActual.strAnio && historia.strTermino == periodoActual.strTermino && materia.intIdMateria == data.idMateria
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

        public class IdCurso
        {
            public int idCurso { get; set; }
        }
        [HttpPost("estudiantesPorCurso")]
        public IActionResult estudiantesPorCurso([FromBody] IdCurso data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join historia in context.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.strEstado == "A" && curso.intIdCurso == data.idCurso
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        public IQueryable sacarHorarioEstudiante(int idPersona)
        {
            var periodoActual = this.periodoActual();

            var query =
                from historia in context.HISTORIA_ANIO
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in context.TBL_HORARIO on historia.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
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
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            return query;
        }

        public IQueryable sacarHorarioProfesor(int idPersona)
        {
            var periodoActual = this.periodoActual();

            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
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
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            return query;
        }

        public bool esProfesor(int idPersona)
        {
            Console.WriteLine("Entro");
            var periodoActual = this.periodoActual();
            var query = context.TBL_CURSO.Where(curso => curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && 
            (curso.intIdProfesor == idPersona || curso.intIdProfesor1 == idPersona || curso.intIdProfesor2 == idPersona || 
            curso.intIdProfesor3 == idPersona || curso.intIdProfesor4 == idPersona || curso.intIdProfesor5 == idPersona));
            return query.ToList().Count() > 0;
        }

        public class IdPersona
        {
            public int idPersona { get; set; }
        }
        [HttpPost("horarioEstudiante")]
        public IQueryable horarioEstudiante([FromBody] IdPersona data)
        {
            return sacarHorarioEstudiante(data.idPersona);
        }

        [HttpPost("horarioProfesor")]
        public IQueryable horarioProfesor([FromBody] IdPersona data)
        {
            return sacarHorarioProfesor(data.idPersona);
        }

        public class IdsPersonas
        {
            public List<int> idsPersonas { get; set; }
        }
        [HttpPost("horariosPersonas")]
        public List<IQueryable> horariosPersonas([FromBody] IdsPersonas data)
        {
            var periodoActual = this.periodoActual();

            List<IQueryable> lista = new List<IQueryable>();

            foreach(int idPersona in data.idsPersonas)
            {
                if (esProfesor(idPersona))
                {
                    lista.Add(sacarHorarioProfesor(idPersona));
                }
                else
                {
                    lista.Add(sacarHorarioEstudiante(idPersona));
                }
                
            };
            return lista;
        }


        [HttpPost("materiasPorProfesor")]
        public IQueryable materiasPorProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();

            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where curso.strEstado == "A" && persona.intIdPersona == data.idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };

            return query;
        }

        [HttpPost("esProfesor")]
        public Dictionary<string, bool> wsEsProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.intIdProfesor == data.idPersona
                select new
                {
                    idCurso = curso.intIdCurso
                };

            Dictionary<string, bool> resultado = new Dictionary<string, bool>();
            resultado.Add("resultado",query.ToArray().Length != 0);
            return resultado;
        }

        [HttpPost("profesoresPorFacultad")]
        public IQueryable profesoresPorFacultad([FromBody] IdFacultad data)
        {
            //Se va a tomar en cuenta que enseñen materias que pertenezcan a la facultad.
            //Si enseña al menos una de la facultad, se lo tomará en cuenta.
            var periodoActual = this.periodoActual();
            var query =
                from persona in context.TBL_PERSONA
                join curso in context.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && materia.intIdUnidad == data.idFacultad
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

        [HttpPost("profesoresPorMateria")]
        public IQueryable profesoresPorMateria([FromBody] IdMateria data )
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == data.idMateria
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return query;
        }

        [HttpPost("directivoFacultad")]
        public IQueryable directivoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
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

        [HttpPost("subdecanoFacultad")]
        public IQueryable subdecanoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
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

        /*
        [HttpPost("coordinadoresMateriasFacultad")]
        public IActionResult coordinadoresMateriasFacultad ([FromForm] int idFacultad)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
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
        [HttpPost("coordinadorCarrera")]
        public IQueryable coordinadorMateria([FromBody] IdPrograma data)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
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
        }

        [HttpPost("coordinadoresFacultad")]
        public IQueryable coordinadoresFacultad([FromBody] IdFacultad data)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join unidad in context.TBL_UNIDAD on programa.intIdUnidadEjecuta equals unidad.intIdUnidad
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
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

        [HttpPost("LugarPadre")]
        public object idLugarPadre([FromForm] int idLugar)
        {
            CdaLugar lugar = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return new { idPadre = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre).FirstOrDefault().intIdLugarEspol };
            }

            return new { idPadre = -1 };
        }

        public IQueryable sacarCursosEstudiante(int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join historia in context.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        public IQueryable sacarCursosProfesor(int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        [HttpPost("cursosEstudiante")]
        public IQueryable cursosEstudiante([FromBody] IdPersona data)
        {       
            return sacarCursosEstudiante(data.idPersona);
        }

        [HttpPost("cursosProfesor")]
        public IQueryable cursosProfesor([FromBody] IdPersona data)
        {
            return sacarCursosProfesor(data.idPersona);
        }

        [HttpPost("cursosRelacionados")]
        public IQueryable cursosRelacionados([FromBody] IdPersona data)
        {
            if (esProfesor(data.idPersona)) return sacarCursosProfesor(data.idPersona);
            else return sacarCursosEstudiante(data.idPersona);
        }
    }
}