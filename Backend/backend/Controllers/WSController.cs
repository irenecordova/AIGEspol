using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class WSController : ControllerBase
    {
        [HttpGet("datosMapa/{dia}")]
        public IActionResult datosMapa(int dia)
        {
            ConexionEspol conexionEspol = new ConexionEspol(new System.Net.Http.HttpClient());
            string resultado = conexionEspol.datosMapa(dia).Result;
            var dict = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            //Console.WriteLine(dict.ToString());
            return Ok(dict);
        }
    }
}