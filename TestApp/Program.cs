using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string a = "112.88";
            string b = "1690484,43543";

            decimal? ad = a.ToDecimal();
            decimal? bd = b.ToDecimal();

            Console.Read();
 
            
        }
    }
}
