using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace analizadorLexico_v0._2
{
    public class Sintactico
    {
        public static void cargarTabla2()
        {
            string fileName = "GR2slrTable.txt";
            
            string[] lineas = System.IO.File.ReadAllLines(@fileName);
            foreach (string linea in lineas)
            {
                
            }
        }
        public void cargarReglas()
        {

        }
    }
}
