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
        [HttpGet]
        public ActionResult GetEspacios()
        {
            var retorno = ConexionBase.EjecutarSP<Espacio>(Constants.NombreSPEspacioList, Constants.CursorEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetEspacio(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Espacio>(Constants.NombreSPEspacioItemId, id, Constants.CursorEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}