using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Tools;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Newtonsoft.Json;

namespace backend.Services
{
    public static class ConexionBase
    {

        public static OracleConnection NewConnection()
        {
            return new OracleConnection(Constants.ApiConnectionString);
        }

        public static string Ejecutar(string sql)
        {
            try
            {
                OracleConnection conn = NewConnection();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                OracleCommand command = new OracleCommand(sql,conn);
                OracleDataReader reader = command.ExecuteReader();

                var retorno = "";
                while (reader.Read())
                {
                    retorno = reader.GetString(0);
                    Console.WriteLine(reader.GetData(0));
                }

                reader.Dispose();
                command.Dispose();
                conn.Close();
                return retorno;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
                return "";
            }
        }

        public static List<T> EjecutarSP<T>(string nombre, string nombreCursor)
        {
            List<T> result = new List<T>();

            try
            {
                OracleConnection conn = NewConnection();
                Console.WriteLine("punto 1");
                
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    Console.WriteLine("punto 2");
                    conn.Open();
                }

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("punto 3");
                    OracleCommand command = new OracleCommand(nombre, conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(nombreCursor, OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(JsonConvert.DeserializeObject<T>(reader.GetValue(0).ToString()));
                    }
                }
                Console.WriteLine("punto 5");
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
            }

            return result;
        } 

    }
}
