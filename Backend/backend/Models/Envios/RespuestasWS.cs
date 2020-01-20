﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Envios
{
    public class RespuestasWS
    {
    }

    public class DatosPersonaWS
    {
        public int idPersona { get; set; }
        public string matricula { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
    }

    public class DatosMapaWS
    {
        public Nullable<int> idHorario { get; set; }
        public Nullable<DateTime> fecha { get; set; }
        public Nullable<DateTime> horaInicio { get; set; }
        public Nullable<DateTime> horaFin { get; set; }
        public string tipoHorario { get; set; }
        public int numRegistrados { get; set; }
        public string tipoCurso { get; set; }
        public int idLugar { get; set; }
        public string descripcionLugar { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string tipoLugar { get; set; }

    }

    public class TipoSemana
    {
        public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
    }
}
