using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Tools;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class Lista_PersonalizadaController : ControllerBase
    {
        private readonly ContextAIG context;

        public Lista_PersonalizadaController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Lista_Personalizada> GetListasPersonalizadas()
        {
            return context.TBL_Lista_Personalizada.ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<Lista_Personalizada> GetListaPersonalizada(long id)
        {
            return context.TBL_Lista_Personalizada.Where(x => x.id == id).ToList();
        }

        [HttpPost]
        public IActionResult InsertarListaPersonalizada([FromForm] string data)
        {
            Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Lista_Personalizada lista = new Lista_Personalizada
            {
                idPersona = dicc["idPersona"],
                nombre = dicc["nombre"]
            };
            context.Add(lista);
            context.SaveChanges();

            //Creación de anidadas


            Dictionary<string, long> resultado = new Dictionary<string, long>();
            resultado.Add("idInsertado", lista.id);
            return Ok(resultado);
        }
    }
}