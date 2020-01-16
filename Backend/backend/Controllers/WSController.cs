using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models.Envios;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class WSController : ControllerBase
    {
        [HttpGet("datosMapa/{dia}")]
        public IActionResult datosMapa(int dia)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.datosMapa(dia).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            //Console.WriteLine(dict.ToString());
            return Ok(datosQuery);
        }

        [HttpPost("cursosRelacionados")]
        public IActionResult cursosRelacionados([FromForm] int idPersona)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = "";
            if (conexionEspol.esProfesor(idPersona))
            {
                resultado = conexionEspol.cursosProfesor(idPersona).Result;
            }
            else
            {
                resultado = conexionEspol.cursosEstudiante(idPersona).Result;
            }
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("personasPorNombreYApellido")]
        public IActionResult personasPorNombreYApellido([FromForm] string nombres, [FromForm] string apellidos)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.personaPorNombreYApellido(nombres, apellidos).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorCarrera")]
        public IActionResult estudiantesPorCarrera([FromForm] int idCarrera )
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCarrera(idCarrera).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorFacultad")]
        public IActionResult estudiantesPorFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorFacultad(idFacultad).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorCurso")]
        public IActionResult estudiantesPorCurso([FromForm] int idCurso)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCurso(idCurso).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("profesoresPorFacultad")]
        public IActionResult profesoresPorFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.profesoresPorFacultad(idFacultad).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("dirigentesFacultad")]
        public IActionResult dirigentesFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado1 = conexionEspol.decanoFacultad(idFacultad).Result;
            var datosQuery1 = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado1);
            string resultado2 = conexionEspol.subdecanoFacultad(idFacultad).Result;
            var datosQuery2 = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado2);

            return Ok(datosQuery1.Concat(datosQuery2));
        }

        [HttpPost("horarioDisponibilidad")]
        public IActionResult horarioDisponibilidad([FromBody] IdsPersonas data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            if (data.ids != null)
            {

            }
            //return Ok(conexionEspol.prueba(data));
            return Ok();
        }

        //La clase Prueba sale de carpeta Models.Envios
        //Ahí están todos los 'jsons' que pueden recibir mis funciones
        [HttpPost("prueba1")]
        public Object cantidadElementos([FromBody] Prueba argumento)
        {
            return new { 
                suma = argumento.num1 + argumento.num2,
                resta = argumento.num1 - argumento.num2,
                multiplicacion = argumento.num1 * argumento.num2,
                division = argumento.num1 / argumento.num2,
            };
        }

    }
}