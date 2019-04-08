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
            double a = 125.11;
            Console.WriteLine(a.ToTurkishMoneyString());
            double b = 15123999.22;
            Console.WriteLine(b.ToTurkishMoneyString());
            double c = 3;
            Console.WriteLine(c.ToTurkishMoneyString());
            Console.Read();
 
            
        }
    }
}
