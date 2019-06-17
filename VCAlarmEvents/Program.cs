using System;

namespace VCAlarmEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            VCDelaysAlarm.RitardiSoapClient vcd = new VCDelaysAlarm.RitardiSoapClient();
            Console.WriteLine("Hello World!");
        }
    }
}
