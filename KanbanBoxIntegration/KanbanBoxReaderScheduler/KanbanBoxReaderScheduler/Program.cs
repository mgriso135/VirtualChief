using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoxReaderScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calling web service to checking the status of Kanban Cards on your KanbanBox account.\n");
            KanbanBoxReader.KanbanBoxReaderSoapClient kBoxRdr = new KanbanBoxReader.KanbanBoxReaderSoapClient();
            kBoxRdr.Main();
            Console.WriteLine("Closing. Bye");
        }
    }
}
