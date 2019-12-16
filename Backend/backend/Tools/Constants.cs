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

        public static readonly string CursorBloque = "BLOQUECURSOR";
        public static readonly string NombreSPBloqueList = "BLOQUE_LIST";
        public static readonly string NombreSPBloqueItemId = "BLOQUE_ITEM_ID";

        public static readonly string CursorEspacio = "ESPACIOCURSOR";
        public static readonly string NombreSPEspacioList = "ESPACIO_LIST";
        public static readonly string NombreSPEspacioItemId = "ESPACIO_ITEM_ID";

        public static readonly string CursorInvitacion = "INVITACIONCURSOR";
        public static readonly string NombreSPInvitacionList = "INVITACION_LIST";
        public static readonly string NombreSPInvitacionItemId = "INVITACION_ITEM_ID";

        public static readonly string CursorEstadoInvitacion = "ESTADOINVITACIONCURSOR";
        public static readonly string NombreSPEstadoInvitacionList = "ESTADO_INVITACION_LIST";
        public static readonly string NombreSPEstadoInvitacionItemId = "ESTADO_INVITACION_ITEM_ID";

        public static readonly string CursorListaPersonalizada = "LISTAPERSONALIZADACURSOR";
        public static readonly string NombreSPListaPersonalizadaList = "LISTA_PERSONALIZADA_LIST";
        public static readonly string NombreSPListaPersonalizadaItemId = "LISTAPERSONALIZADA_ITEM_ID";

        public static readonly string CursorNombreBloque = "NOMBREBLOQUECURSOR";
        public static readonly string NombreSPNombreBloqueList = "NOMBRE_BLOQUE_LIST";
        public static readonly string NombreSPNombreBloqueItemId = "NOMBRE_BLOQUE_ITEM_ID";

        public static readonly string CursorNombreEspacio = "NOMBREESPACIOCURSOR";
        public static readonly string NombreSPNombreEspacioList = "NOMBRE_ESPACIO_LIST";
        public static readonly string NombreSPNombreEspacioItemId = "NOMBRE_ESPACIO_ITEM_ID";

        public static readonly string CursorReunion = "REUNIONCURSOR";
        public static readonly string NombreSPReunionList = "REUNION_LIST";
        public static readonly string NombreSPReunionItemId = "REUNION_ITEM_ID";

        public static readonly string CursorTipoEspacio = "TIPOESPACIOCURSOR";
        public static readonly string NombreSPTipoEspacioList = "TIPO_ESPACIO_LIST";
        public static readonly string NombreSPTipoEspacioItemId = "TIPO_ESPACIO_ITEM_ID";

        public static readonly string CursorTipoFiltro = "TIPOFILTROCURSOR";
        public static readonly string NombreSPTipoFiltroList = "TIPO_FILTRO_LIST";
        public static readonly string NombreSPTipoFiltroItemId = "TIPO_FILTRO_ITEM_ID";
    }
}
