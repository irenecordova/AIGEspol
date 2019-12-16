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
    public class ReunionController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetReuniones()
        {
            var retorno = ConexionBase.EjecutarSP<Reunion>(Constants.NombreSPReunionList, Constants.CursorReunion);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetReunion(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Reunion>(Constants.NombreSPReunionItemId, id, Constants.CursorReunion);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}