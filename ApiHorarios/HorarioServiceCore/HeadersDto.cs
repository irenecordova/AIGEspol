using System;

namespace HorarioServiceCore
{
    public class HeadersDto
    {
        public string X_SAAC_netid { get; set; }
        public string X_SAAC_AppIp { get; set; }
        public string X_SAAC_AppName { get; set; }
        public string X_SAAC_API_AppToken { get; set; }
        public string X_SAAC_API_AppKey { get; set; }
        public string User_Agent { get; set; }
        public string Host { get; set; }
    }
}
