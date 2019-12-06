using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIGEspol_Frontend.Services
{
    public class TokenValidator
    {

        public Boolean ValidateSessionToken(string ticket)
        {
            /*
            WebRequest peticion;
            peticion = WebRequest.Create("https://auth.espol.edu.ec/serviceValidate?ticket=" + ticket + "&service=http://localhost:44319/Login");
            
            Stream objStream;
            objStream = peticion.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
            }
            Console.ReadLine();
            */
            return true;
        }
    }
}
