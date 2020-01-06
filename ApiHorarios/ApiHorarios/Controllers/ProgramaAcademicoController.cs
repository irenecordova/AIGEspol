﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramaAcademicoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public ProgramaAcademicoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaProgramaAcademico> Get()
        {
            return context.TBL_PROGRAMA_ACADEMICO.ToList();
        }
    }
}