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
    public class Tipo_EspacioController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetTipoEspacios()
        {
            var retorno = ConexionBase.EjecutarSP<Tipo_Espacio>(Constants.NombreSPTipoEspacioList, Constants.CursorTipoEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetTipoEspacio(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Tipo_Espacio>(Constants.NombreSPTipoEspacioItemId, id, Constants.CursorTipoEspacio);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}