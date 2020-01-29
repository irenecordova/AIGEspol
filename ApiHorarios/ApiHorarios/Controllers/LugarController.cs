using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using ApiHorarios.DataRepresentationsIN;

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

        [HttpPost("Aulas/Bloque")]
        public IEnumerable<CdaLugar> AulasPorBloque([FromBody] IdLugar data)
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "A" && x.strEstado == "V" && data.idLugar == x.intIdLugarPadre);
        }

        [HttpPost("padre")]
        public object idLugarPadre([FromBody] IdLugar data)
        {
            CdaLugar lugar = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == data.idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return new { idPadre = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre).FirstOrDefault().intIdLugarEspol };
            }

            return new { idPadre = -1 };
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