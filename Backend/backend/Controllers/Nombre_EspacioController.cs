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
    public class Nombre_EspacioController : ControllerBase
    {
        private readonly ContextAIG context;

        public Nombre_EspacioController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Nombre_Espacio> GetNombresEspacios()
        {

            return context.TBL_Nombre_Espacio.ToList();
        }

        [HttpGet("{id}")]
        public Nombre_Espacio GetNombreEspacio(int id)
        {
            return context.TBL_Nombre_Espacio.Where(x => x.id == id).FirstOrDefault();
        }
    }
}