using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugarController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public LugarController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaLugar> Get()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strEstado == "V").ToList();
        }

        [HttpGet("{id}")]
        public CdaLugar GetById(int id)
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == id).FirstOrDefault();
        }

        [HttpGet("Edificios")]
        public IEnumerable<CdaLugar> facultades()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V");
        }

        [HttpGet("Aulas")]
        public IEnumerable<CdaLugar> Aulas()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "A" && x.strEstado == "V");
        }

        [HttpGet("Aulas/Bloque/{idBloque}")]
        public IEnumerable<CdaLugar> AulasPorBloque(int idBloque)
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "A" && x.strEstado == "V" && idBloque == x.intIdLugarPadre);
        }
        /*
        [HttpGet("Oficinas")]
        public IEnumerable<CdaLugar> Oficinas()
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "O" && x.strEstado == "V");
        }

        [HttpGet("Oficinas/Bloque/{idBloque}")]
        public IEnumerable<CdaLugar> OficinasPorBloque(int idBloque)
        {
            return contextSAAC.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "O" && x.strEstado == "V" && idBloque == x.intIdLugarPadre);
        }*/

    }
}