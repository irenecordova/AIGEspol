using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Tools;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]

    public class EspacioController : ControllerBase
    {

        private readonly ContextAIG context;

        public EspacioController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Espacio> GetEspacios()
        {
            return context.TBL_Espacio.ToList();
        }

        [HttpGet("{id}")]
        public Espacio GetEspacio(int id)
        {

            return context.TBL_Espacio.Where(x => x.id == id).FirstOrDefault(); ;
        }

        [HttpGet("zonas")]
        public IEnumerable<int> GetZonas()
        {
            return context.TBL_Espacio.Where(e => e.zona != null).Select(e => e.zona.Value).Distinct().OrderBy(e => e);
        }

        [HttpGet("bloques/zona/{numZona}")]
        public IEnumerable<Espacio> GetBloquesAcademicosPorZona(int numZona)
        {
            return context.TBL_Espacio.Where(e => e.zona == numZona);
        }
    }
}