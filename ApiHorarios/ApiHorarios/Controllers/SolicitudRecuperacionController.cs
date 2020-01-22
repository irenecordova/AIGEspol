using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HorarioContext;
using HorarioModelSaac;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudRecuperacionController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public SolicitudRecuperacionController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaSolicitudRecuperacion> Get()
        {
            return context.TBL_SOLICITUD_REC.Where(x => x.strEstado == "A").ToList();
        }

        [HttpGet("{id}")]
        public CdaSolicitudRecuperacion GetPorId(int id)
        {
            return context.TBL_SOLICITUD_REC.Where(x => x.strEstado == "A").FirstOrDefault();
        }

        public class InDataFecha
        {
            public DateTime fecha { get; set; }
            public List<int> idsPersonas { get; set; }
        }
        [HttpPost("fecha")]
        public IEnumerable<CdaSolicitudRecuperacion> solicitudesPorFecha([FromBody] InDataFecha data)
        {
            return context.TBL_SOLICITUD_REC.Where(x => x.strEstado == "A" && x.dtFecha!=null && x.dtFecha.Value.Day == data.fecha.Day
            && x.dtFecha.Value.Month == data.fecha.Month && x.dtFecha.Value.Year == data.fecha.Year
            && x.intIdSolicitante!=null && data.idsPersonas.Contains(x.intIdSolicitante.Value)).ToList();
        }

        [HttpPost("semana")]
        public IEnumerable<CdaSolicitudRecuperacion> solicitudesPorSemana([FromBody] InDataFecha data)
        {
            int numDia = (int)data.fecha.DayOfWeek;
            var fechaInicioSemana = data.fecha.AddDays(-(numDia-1)).Date;
            var fechaFinSemana = data.fecha.AddDays(8 - numDia).Date;
            
            return context.TBL_SOLICITUD_REC.Where(x => x.strEstado == "A" && x.dtFecha != null
            && x.dtFecha.Value >= fechaInicioSemana
            && x.dtFecha.Value < fechaFinSemana && x.dtFecha.Value.Year == data.fecha.Year
            && x.intIdSolicitante != null && data.idsPersonas.Contains(x.intIdSolicitante.Value)).ToList();
        }
    }
}