using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHorarios.DataRepresentationsIN
{
    public class IdPersona
    {
        public int idPersona { get; set; }
    }

    public class InDatosHorarios
    {
        public DateTime fecha { get; set; }
        public List<int> idsPersonas { get; set; }
    }

    public class IdsPersonas
    {
        public List<int> idsPersonas { get; set; }
    }

    public class IdLugar
    {
        public int idLugar { get; set; }
    }

    public class IdCurso
    {
        public int idCurso { get; set; }
    }

    public class IdFacultad
    {
        public int idFacultad { get; set; }
    }

    public class IdMateria
    {
        public int idMateria { get; set; }
    }

    public class IdPrograma
    {
        public int idPrograma { get; set; }
    }

    public class NombreApellido
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
    }

    public class NombreCompleto
    {
        public string nombre { get; set; }
    }

    public class InDataFecha
    {
        public DateTime fecha { get; set; }
    }
    public class TipoSemana
    {
        public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
    }
}
