using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string constr = "User Id=admin;Password=admin;Data Source=oracle";
                OracleConnection con = new OracleConnection(constr);
                con.Open();
                Console.WriteLine("Connected to Oracle Database {0}", con.ServerVersion);
                con.Dispose();
                Console.WriteLine("Press RETURN to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
