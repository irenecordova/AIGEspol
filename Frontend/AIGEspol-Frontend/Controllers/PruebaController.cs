using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIGEspol_Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebaController : ControllerBase
    {

        public async Task<string> prueba1(int x, int y)
        {
            var conexion = new HttpClient();
            string url = "https://localhost:44303/";
            conexion.BaseAddress = new Uri(url);
            conexion.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage respuesta = await conexion.PostAsJsonAsync("api/prueba1", new { num1 = x, num2 = y });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        //Cómo crear reunion
        public async Task<string> prueba2(int idCreador, string asunto, string descripcion, int idLugar, DateTime fechaInicio, DateTime fechaFin, List<int> idPersonas)
        {
            var conexion = new HttpClient();
            string url = "https://localhost:44303/";
            conexion.BaseAddress = new Uri(url);
            conexion.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage respuesta = await conexion.PostAsJsonAsync("api/reunion", new {
                idCreador = idCreador,
                asunto = asunto,
                descripcion = descripcion,
                idLugar = idLugar,
                fechaInicio = fechaInicio,
                fechaFin = fechaFin,
                idPersonas = idPersonas,
            });
            string result = respuesta.Content.ReadAsStringAsync().Result;
            return result;
        }

        [HttpGet("{num}")]
        public IActionResult probando(int num)
        {
            if (num == 1)
            {
                return Ok(prueba1(3,5).Result);
            } else if (num == 2)
            {
                var lista = new List<int>();
                lista.Add(2);
                lista.Add(6);
                lista.Add(4);
                return Ok(prueba2(
                        3333,
                        "asunto vago",
                        "descripcion vaga",
                        50,
                        DateTime.Now,
                        DateTime.Now,
                        lista
                    ).Result);
            }
            return Ok();
        }

        //Esto era otra forma que estaba intentando, pero no pude hacerlo funcar
        /*
            var contenido = "{\"argumento\":[";
            foreach(int i in argPrueba)
            {
                contenido += i.ToString() + ","; 
            }
            contenido += "]\"}";

            HttpContent c = new StringContent(contenido, Encoding.UTF8, "application/json");
            */
    }
}