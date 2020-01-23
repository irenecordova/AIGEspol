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
    public class CursoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public CursoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaCurso> Get()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            return context.TBL_CURSO.ToList().Where(x => x.strEstado == "A" && x.intIdPeriodo == periodoActual.intIdPeriodoAcademico);
        }

        [HttpGet("{id}")]
        public CdaCurso Get(int id)
        {
            return context.TBL_CURSO.ToList().Where(x => x.strEstado == "A").FirstOrDefault();
        }


    }
}