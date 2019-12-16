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
        [HttpGet]
        public ActionResult GetNombresEspacios()
        {
            var retorno = ConexionBase.EjecutarSP<Nombre_Espacio>(Constants.NombreSPNombreEspacioList, Constants.CursorNombreEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetNombreEspacio(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Nombre_Espacio>(Constants.NombreSPNombreEspacioItemId, id, Constants.CursorNombreEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}