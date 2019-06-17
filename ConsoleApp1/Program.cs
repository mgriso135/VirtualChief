using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            VCDelays.RitardiSoapClient vcd = new VCDelays.RitardiSoapClient();
            vcd.Main();
            Console.WriteLine("All alarms have been sent.");
        }
    }
}
