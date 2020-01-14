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

        [HttpGet("datosMapa/{dia}", Name = "horarios_por_dia")]
        public IActionResult datosMapa(int dia)
        {
            int idPeriodoActual = this.periodoActual().intIdPeriodoAcademico;
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where horario.intDia == dia && curso.strEstado == "A" && lugar.strEstado == "A" && curso.intIdPeriodo == idPeriodoActual
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
            
            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        // Cantidad de estudiantes/Cantidad registrados en periodo
        // Top 3 bloques con más personas
        // Cantidad de bloques usados/Cantidad de bloques totales
        // Prom. de personas por bloque
        // Cantidad de lugares usados (Aulas, labs, canchas)
        // Promedio personas por lugar (Aulas, labs, canchas)

        [HttpGet("periodoActual")]
        public CdaPeriodoAcademico periodoActual()
        {
            //return context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.chEstado == "E" && x.strTipo == "G");
            return context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
        }

        [HttpPost("personasPorNombreYApellido")]
        //[HttpGet("personasPorNombreYApellido")]
        public IActionResult personasPorNombreYApellido([FromForm]string nombres = "", [FromForm]string apellidos = "")
        {
            var query = context.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");

            if (nombres != "")
            {
                query = query.Where(persona => (persona.strNombres != null && persona.strNombres.Contains(nombres.ToUpper())));
            }

            if (apellidos != "")
            {
                query = query.Where(persona => (persona.strApellidos != null && persona.strApellidos.Contains(apellidos.ToUpper())));
            }
            
            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("estudiantesPorCarrera")]
        public IActionResult estudiantesPorCarrera([FromForm]int idPrograma)
        {
            var query =
                from carrera in context.CARRERA_ESTUDIANTE
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                join estudiante in context.TBL_PERSONA on carrera.strCodEstudiante.Trim() equals estudiante.strCodEstudiante.Trim()
                where estudiante.strCodEstudiante != null && programa.intIdPrograma == idPrograma
                select new
                {
                    idPersona = estudiante.intIdPersona,
                    matricula = estudiante.strCodEstudiante,
                    nombres = estudiante.strNombres,
                    apellidos = estudiante.strApellidos,
                    email = estudiante.strEmail
                };

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("estudiantesPorFacultad")]
        public IActionResult estudiantesPorFacultad([FromForm]int idFacultad)
        {
            var query =
                from persona in context.TBL_PERSONA
                join carrera in context.CARRERA_ESTUDIANTE on persona.strCodEstudiante.Trim() equals carrera.strCodEstudiante.Trim()
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                where persona.strCodEstudiante != null && programa.intIdUnidadEjecuta == idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("estudiantesPorMateria")]
        public IActionResult estudiantesPorMateria([FromForm]int idMateria = 0)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join historia in context.HISTORIA_ANIO on materia.strCodigoMateria equals historia.strCodMateria
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where historia.strAnio == periodoActual.strAnio && historia.strTermino == periodoActual.strTermino && materia.intIdMateria == idMateria
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

        [HttpPost("horarioEstudiante")]
        public IActionResult horarioEstudiante([FromForm] int idPersona)
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

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("horarioProfesor")]
        public IActionResult horarioProfesor([FromForm] int idPersona)
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

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("materiasPorProfesor")]
        public IActionResult materiasPorProfesor([FromForm]int idProfesor)
        {
            var periodoActual = this.periodoActual();

            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where curso.strEstado == "A" && persona.intIdPersona == idProfesor && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("esProfesor")]
        public IActionResult esProfesor([FromForm] int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.intIdProfesor == idPersona
                select new
                {
                    idCurso = curso.intIdCurso
                };

            Dictionary<string, bool> resultado = new Dictionary<string, bool>();
            resultado.Add("resultado",query.ToArray().Length != 0);
            return Ok(JsonConvert.SerializeObject(resultado));
        }

        [HttpPost("profesoresPorFacultad")]
        public IActionResult profesoresPorFacultad([FromForm]int idFacultad)
        {
            //Se va a tomar en cuenta que enseñen materias que pertenezcan a la facultad.
            //Si enseña al menos una de la facultad, se lo tomará en cuenta.
            var periodoActual = this.periodoActual();
            var query =
                from persona in context.TBL_PERSONA
                join curso in context.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && materia.intIdUnidad == idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("profesoresPorMateria")]
        public IActionResult profesoresPorMateria([FromForm]int idMateria = 0)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == idMateria
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToList()));
        }

        [HttpPost("directivoFacultad")]
        public IActionResult directivoFacultad([FromForm] int idFacultad)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
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

        [HttpPost("subdecanoFacultad")]
        public IActionResult subdecanoFacultad([FromForm] int idFacultad)
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
        public IActionResult coordinadorMateria([FromForm] int idPrograma)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
                where programa.intIdPrograma == idPrograma
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

        [HttpPost("coordinadoresFacultad")]
        public IActionResult coordinadoresFacultad([FromForm] int idFacultad)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join unidad in context.TBL_UNIDAD on programa.intIdUnidadEjecuta equals unidad.intIdUnidad
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
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

        [HttpPost("LugarPadre")]
        public IActionResult lugarPadre([FromForm] int idLugar)
        {
            CdaLugar lugar = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return Ok(context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre));
            }

            return Ok(JsonConvert.SerializeObject(new List<int>()));
        }
    }
}