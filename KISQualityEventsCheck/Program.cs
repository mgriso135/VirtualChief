using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISQualityEventsCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            QualityModuleEvents.QualityModuleEventsSoapClient qCheck = new QualityModuleEvents.QualityModuleEventsSoapClient();
            qCheck.NotifyImprovementActionsDelays();
            qCheck.NotifyCorrectiveActionsDelays();
        }
    }
}
