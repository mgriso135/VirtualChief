using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace KIS.Produzione
{
    [ServiceContract(Namespace = "Produzione")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcCadenza
    {
        // Per utilizzare HTTP GET, aggiungere l'attributo [WebGet]. (ResponseFormat predefinito è WebMessageFormat.Json)
        // Per creare un'operazione che restituisca XML,
        //     aggiungere [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     e includere la riga seguente nel corpo dell'operazione:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public String Test()
        {
            // Aggiungere l'implementazione dell'operazione qui
            return "Ciao sono il tuo webservice in ajax";
        }

        // Aggiungere altre operazioni qui e contrassegnarle con [OperationContract]
    }
}
