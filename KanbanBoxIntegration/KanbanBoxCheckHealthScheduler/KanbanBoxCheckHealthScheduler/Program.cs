using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoxCheckHealthScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calling webservice to check the congruency between Kaizen Indicator System and KanbanBox Cards...\n");
            KanbanBoxCheckHealth.KanbanBoxCheckHealthSoapClient kBoxCheck = new KanbanBoxCheckHealth.KanbanBoxCheckHealthSoapClient();
            kBoxCheck.Main();
            Console.WriteLine("Closing... Bye!\n");
        }
    }
}
