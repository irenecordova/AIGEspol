using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HorarioContext;
using HorarioModelSaac;
using Newtonsoft.Json;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public UnidadController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaUnidad> unidades()
        {
            return context.TBL_UNIDAD.ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<CdaUnidad> unidad(int id)
        {
            return context.TBL_UNIDAD.Where(x => x.intIdUnidad == id).ToList();
        }

        //Las facultades tienen tipo "U"
        //Fiec tienen id 15004
        [HttpGet("facultades")]
        public IEnumerable<CdaUnidad> facultades()
        {
            return context.TBL_UNIDAD.Where(x => x.strTipo == "U").ToList();
        }

    }
}