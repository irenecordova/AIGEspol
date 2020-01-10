using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Tools;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class Tipo_FiltroController : ControllerBase
    {

        private readonly ContextAIG context;

        public Tipo_FiltroController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Tipo_Filtro> GetTipoFiltros()
        {
            return context.TBL_Tipo_Filtro.ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<Tipo_Filtro> GetTipoFiltro(long id)
        {
            return context.TBL_Tipo_Filtro.Where(x => x.id == id).ToList();
        }

        [HttpPost]
        public IActionResult InsertarTipoFiltro([FromForm] string data)
        {
            Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Tipo_Filtro tipoFiltro = new Tipo_Filtro
            {
                criterio = dicc["criterio"]
            };
            context.Add(tipoFiltro);
            context.SaveChanges();
            Dictionary<string, long> resultado = new Dictionary<string, long>();
            resultado.Add("idInsertado", tipoFiltro.id);
            return Ok(resultado);
        }
    }
}