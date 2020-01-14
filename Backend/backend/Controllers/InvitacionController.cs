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
        private readonly ContextAIG context;

        public InvitacionController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Invitacion> GetEspacios()
        {
            return context.TBL_Invitacion.ToList();
        }

        [HttpGet("{id}")]
        public Invitacion GetEspacio(int id)
        {

            return context.TBL_Invitacion.Where(x => x.idInvitacion == id).FirstOrDefault(); ;
        }


    }
}