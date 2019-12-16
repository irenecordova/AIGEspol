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
    public class Lista_PersonalizadaController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetListasPersonalizadas()
        {
            var retorno = ConexionBase.EjecutarSP<Lista_Personalizada>(Constants.NombreSPListaPersonalizadaList, Constants.CursorListaPersonalizada);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetListaPersonalizada(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Lista_Personalizada>(Constants.NombreSPListaPersonalizadaItemId, id, Constants.CursorListaPersonalizada);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}