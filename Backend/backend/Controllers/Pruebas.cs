using System;
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
    public class PruebasController : ControllerBase
    {
        private readonly ContextAIG context;

        public PruebasController(ContextAIG context)
        {
            this.context = context;
        }

    }
}
