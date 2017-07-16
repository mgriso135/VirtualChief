using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            KISScheduler.RitardiSoapClient Ritardi = new KISScheduler.RitardiSoapClient();
            Ritardi.Main();
            KISSchedulerWarning.WarningSoapClient Warning = new KISSchedulerWarning.WarningSoapClient();
            Warning.Main();
        }
    }
}
