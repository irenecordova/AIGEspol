using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Tools
{
    public static class Constants
    {
        public static readonly string UrlWebServices = "";
        public static readonly string ApiConnectionString = "User Id=admin;Password=admin;Data Source=" +
            "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            "(HOST=localhost)(PORT=1521))(CONNECT_DATA=" +
            "(SERVICE_NAME=xe)))";
        public static readonly string CursorTipoFiltro = "TIPOFILTROCURSOR";
        public static readonly string NombreSPTipoFiltroList = "TIPO_FILTRO_LIST";
    }
}
