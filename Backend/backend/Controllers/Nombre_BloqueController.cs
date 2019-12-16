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
    public class Nombre_BloqueController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetNombresBloques()
        {
            var retorno = ConexionBase.EjecutarSP<Nombre_Bloque>(Constants.NombreSPNombreBloqueList, Constants.CursorNombreBloque);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetNombreBloque(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Nombre_Bloque>(Constants.NombreSPNombreBloqueItemId, id, Constants.CursorNombreBloque);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}