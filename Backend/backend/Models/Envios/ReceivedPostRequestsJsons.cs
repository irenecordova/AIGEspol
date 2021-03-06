﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Envios
{
    public class ReceivedPostRequestsJsons
    {
    }

    public class Prueba
    {
        public int num1 { get; set; }
        public int num2 { get; set; }
    }

    public class IdsPersonas
    {
        public List<int> ids { get; set; }
    }

    public class IdFacultad
    {
        public int idFacultad { get; set; }
    }

    public class IdCarrera
    {
        public int idCarrera { get; set; }
    }

    public class IdCurso
    {
        public int idCurso { get; set; }
    }

    public class IdMateria
    {
        public int idMateria { get; set; }
    }

    public class NombrePersona
    {
        public string nombre { get; set; }
    }

    public class DatosMapaInput
    {
        public DateTime Fecha { get; set; }
    }

    public class DatosHorarioDisponibilidadInput
    {
        public DateTime fecha { get; set; }
        public List<int> idsPersonas { get; set; }
    }

    public class DatosListaPersonalizada
    {
        public int idCreador { get; set; }
        public string nombre { get; set; }
        public List<int> idPersonas { get; set; }
        public List<string> nombrePersonas { get; set; }

    }

    public class DatosReunion
    {
        public int idCreador { get; set; }
        public string asunto { get; set; }
        public string descripcion { get; set; }
        public int idLugar { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public List<int> idPersonas { get; set; }
        public List<string> nombrePersonas { get; set; }
    }

    public class IdPersona
    {
        public int idPersona { get; set; }
    }

    public class IdInvitacion
    {
        public int idInvitacion { get; set; }
    }

    public class IdReunion
    {
        public int idReunion { get; set; }
    }

    public class InLugaresDisponibles
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int idBloque { get; set; }
    }

}
