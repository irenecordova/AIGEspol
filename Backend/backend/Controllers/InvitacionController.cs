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
    public class InvitacionController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetInvitaciones()
        {
            var retorno = ConexionBase.EjecutarSP<Invitacion>(Constants.NombreSPInvitacionList, Constants.CursorInvitacion);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult GetInvitacion(long id)
        {
            var retorno = ConexionBase.EjecutarSP<Invitacion>(Constants.NombreSPInvitacionItemId, id, Constants.CursorInvitacion);
            if (retorno.Count == 0)
            {
                return NotFound();
            }

            return Ok(retorno[0]);
        }
    }
}