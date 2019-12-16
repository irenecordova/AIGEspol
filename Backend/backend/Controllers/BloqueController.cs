﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Tools;

namespace backend.Controllers
{
    [Route("api/[Controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BloqueController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetBloques()
        {
            var retorno = ConexionBase.EjecutarSP<Bloque>(Constants.NombreSPBloqueList, Constants.CursorBloque);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetBloque(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Bloque>(Constants.NombreSPBloqueItemId, id, Constants.CursorBloque);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}