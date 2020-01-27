using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiHorarios.Filter;
using HorarioContext;
using IBM.EntityFrameworkCore;
using IBM.EntityFrameworkCore.Storage.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiHorarios
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CdaDbContextDB2SAAC>(options => options.UseDb2(Configuration.GetConnectionString("ConnectionStringNameSAAC"), p => p.SetServerInfo(IBMDBServerType.LUW, IBMDBServerVersion.LUW_11_01_1010)));
            services.AddDbContext<CdaDbContextDB2SAF3>(options => options.UseDb2(Configuration.GetConnectionString("ConnectionStringNameSAF3"), p => p.SetServerInfo(IBMDBServerType.LUW, IBMDBServerVersion.LUW_11_01_1010)));
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
