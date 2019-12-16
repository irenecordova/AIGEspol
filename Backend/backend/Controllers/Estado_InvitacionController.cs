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
    public class Estado_InvitacionController : ControllerBase
    {
        [Route("api/[controller]")]
        [Produces("application/json")]
        [ApiController]
        public class InvitacionController : ControllerBase
        {
            [HttpGet]
            public ActionResult GetEstadosInvitacion()
            {
                var retorno = ConexionBase.EjecutarSP<Estado_Invitacion>(Constants.NombreSPEstadoInvitacionList, Constants.CursorEstadoInvitacion);
                if (retorno.Count == 0)
                {
                    return NotFound();
                }

                return Ok(retorno);
            }

            [HttpGet("{id}")]
            public ActionResult GetEstadoInvitacion(long id)
            {
                var retorno = ConexionBase.EjecutarSP<Estado_Invitacion>(Constants.NombreSPEstadoInvitacionItemId, id, Constants.CursorEstadoInvitacion);
                if (retorno.Count == 0)
                {
                    return NotFound();
                }

                return Ok(retorno[0]);
            }
        }
    }
}