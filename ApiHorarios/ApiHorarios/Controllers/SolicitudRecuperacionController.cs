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
        }
        [HttpPost("fecha")]
        public IEnumerable<CdaSolicitudRecuperacion> solicitudesPorFecha([FromBody] InDataFecha data)
        {
            return context.TBL_SOLICITUD_REC.Where(x => x.strEstado == "A" && x.dtFecha!=null && x.dtFecha.Value.Day == data.fecha.Day
            && x.dtFecha.Value.Month == data.fecha.Month && x.dtFecha.Value.Year == data.fecha.Year).ToList();
        }
    }
}