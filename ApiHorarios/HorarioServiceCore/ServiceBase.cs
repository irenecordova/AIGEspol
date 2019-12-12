using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HorarioServiceCore
{
    public abstract class ServiceBase
    {
        public HttpRequest Request { get; set; }
        public DbContext Context { get; set; }
        public HeadersDto Header { get; set; }
        public ServiceBase(HttpRequest request)
        {
            Request = request;
        }
        public ServiceBase(HeadersDto headers)
        {

        }
        public ServiceBase(HttpRequest request, DbContext context)
        {
            Request = request;
            Context = context;
        }
    }
}
