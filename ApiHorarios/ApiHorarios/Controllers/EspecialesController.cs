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
    [Route("api/[controller]")]
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
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where horario.intDia == dia
                select new
                {
                    idHorario = horario.intIdHorario,
                    fecha = horario.dtFecha,
                    horaInicio = horario.dtHoraInicio,
                    horaFin = horario.dtHoraFin,
                    numRegistrados = curso.intNumRegistrados,
                    latitud = lugar.strLatitud,
                    longitud = lugar.strLongitud
                };
            
            if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }


    }
}