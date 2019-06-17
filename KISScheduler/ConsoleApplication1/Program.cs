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
            
            VCSchedulerDelays.RitardiSoapClient Ritardi = new VCSchedulerDelays.RitardiSoapClient();
            Ritardi.Main();
            VCSchedulerWarnings.WarningSoapClient Warning = new VCSchedulerWarnings.WarningSoapClient();
            Warning.Main();
        }
    }
}
