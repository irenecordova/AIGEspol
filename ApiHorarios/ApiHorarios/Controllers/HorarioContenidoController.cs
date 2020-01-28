using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioContenidoController : ControllerBase
    {

        private readonly CdaDbContextDB2SAAC context;

        public HorarioContenidoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaHorarioContenido> Get()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            return context.TBL_HORARIO_CONTENIDO.Where(x => x.dtFecha <= periodoActual.dtFechaFin 
            && x.dtFecha >= periodoActual.dtFechaInicio 
            && x.strEstadoRecuperacion == "AP"
            && x.intIdLugarEspol != null).ToList();
        }

        [HttpGet("{id}")]
        public CdaHorarioContenido GetById(int id)
        {
            return context.TBL_HORARIO_CONTENIDO.Where(x => x.intIdHorarioContenido == id).FirstOrDefault();
        }


    }
}