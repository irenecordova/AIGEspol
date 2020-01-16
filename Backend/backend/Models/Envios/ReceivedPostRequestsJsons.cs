using System;
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

    public class DatosPersona
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
    }

    public class DatosListaPersonalizada
    {
        public int idDueño { get; set; }
        public string nombre { get; set; }
        public List<int> idPersonas { get; set; }
        public List<string> nombresPersonas { get; set; }

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
    }
}
