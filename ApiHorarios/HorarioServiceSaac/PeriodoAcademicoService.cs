using HorarioContext;
using HorarioModelSaac;
using HorarioServiceCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace HorarioServiceSaac
{
    public class PeriodoAcademicoService : ServiceBase
    {
        public PeriodoAcademicoService(HttpRequest request) : base(request)
        {

        }

        public CdaPeriodoAcademico GetPeriodoActivo(CdaDbContextDB2SAAC context)
        {
            
            return context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.chEstado == "E" && x.strTipo == "G");
            
        }

    }
}
