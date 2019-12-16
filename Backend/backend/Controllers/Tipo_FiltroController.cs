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
    [Route("api/[Controller]")]
    [Produces("application/json")]
    [ApiController]
    public class Tipo_FiltroController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetTipoFiltros()
        {
            var retorno = ConexionBase.EjecutarSP<Tipo_Filtro>(Constants.NombreSPTipoFiltroList, Constants.CursorTipoFiltro);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetTipoFiltro(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Tipo_Filtro>(Constants.NombreSPTipoFiltroItemId, id, Constants.CursorTipoFiltro);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}