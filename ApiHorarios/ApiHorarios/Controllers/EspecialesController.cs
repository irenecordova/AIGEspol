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
        /*public IActionResult datosMapa(int dia)
        {
            Dictionary<string, dynamic> dict1 = new Dictionary<string, dynamic>();
            dict1.Add("idHorario", 1);
            dict1.Add("fecha", DateTime.Today);
            dict1.Add("horaInicio", DateTime.Today);
            dict1.Add("horaFin", DateTime.Today.AddHours(2));
            dict1.Add("tipoHorario", "E");
            dict1.Add("numRegistrados", 40);
            dict1.Add("tipoCurso", "C");
            dict1.Add("idLugar", 2);
            dict1.Add("diaIngresado", dia);

            Dictionary<string, dynamic> dict2 = new Dictionary<string, dynamic>();
            dict2.Add("idHorario", 2);
            dict2.Add("fecha", DateTime.Today);
            dict2.Add("horaInicio", DateTime.Today);
            dict2.Add("horaFin", DateTime.Today.AddHours(1));
            dict2.Add("tipoHorario", "E");
            dict2.Add("numRegistrados", 40);
            dict2.Add("tipoCurso", "C");
            dict2.Add("idLugar", 2);
            dict2.Add("diaIngresado", dia++);

            Dictionary<string, dynamic> dict3 = new Dictionary<string, dynamic>();
            dict3.Add("idHorario", 3);
            dict3.Add("fecha", DateTime.Today);
            dict3.Add("horaInicio", DateTime.Today);
            dict3.Add("horaFin", DateTime.Today.AddHours(4));
            dict3.Add("tipoHorario", "E");
            dict3.Add("numRegistrados", 40);
            dict3.Add("tipoCurso", "C");
            dict3.Add("idLugar", 2);
            dict3.Add("diaIngresado", dia++);

            List<Dictionary<string, dynamic>> resultado = new List<Dictionary<string, dynamic>>();
            resultado.Add(dict1);
            resultado.Add(dict2);
            resultado.Add(dict3);

            return Ok(JsonConvert.SerializeObject(resultado));
        }*/

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
                where programa.intIdPrograma == idPrograma
                select new
                {
                    idPersona = estudiante.intIdPersona,
                    matricula = estudiante.strCodEstudiante,
                    nombres = estudiante.strNombres,
                    apellidos = estudiante.strApellidos
                };

            return Ok(JsonConvert.SerializeObject(query.ToArray()));

        }

        [HttpPost("personasPorFacultad")]
        public IActionResult personasPorFacultad([FromForm]int idFacultad)
        {
            return Ok();
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
                    persona.intIdPersona,
                    persona.strNombres,
                    persona.strApellidos,
                    historia.strTermino,
                };

            //if (query.ToArray().Length == 0) return NotFound();
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

        [HttpPost("horarioEstudiante")]
        public IActionResult horarioEstudiante([FromForm] int idPersona)
        {
            var periodoActual = this.periodoActual();

            var query =
                from historia in context.HISTORIA_ANIO
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
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
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        [HttpPost("esProfesor")]
        public IActionResult esProfesor([FromForm] int idPersona)
        {
            return Ok();
        }
    }
}