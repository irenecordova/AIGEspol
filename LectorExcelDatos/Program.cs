using System;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace LectorExcelDatos
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Hello World!");
            using(var stream = File.Open("../../../../../DatosEspacios.xlsx",FileMode.Open,FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            //Console.WriteLine(reader.GetValue(0));
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet();
                    Console.WriteLine(result);

                    // The result of each spreadsheet is in result.Tables
                }
            }
        }
    }
}
