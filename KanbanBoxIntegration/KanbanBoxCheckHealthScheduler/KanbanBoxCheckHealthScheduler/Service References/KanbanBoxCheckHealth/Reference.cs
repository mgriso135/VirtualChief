﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.34209
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap")]
    public interface KanbanBoxCheckHealthSoap {
        
        // CODEGEN: Generazione di un contratto di messaggio perché il nome di elemento MainResult dallo spazio dei nomi http://tempuri.org/ non è contrassegnato come nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Main", ReplyAction="*")]
        KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainResponse Main(KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MainRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Main", Namespace="http://tempuri.org/", Order=0)]
        public KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequestBody Body;
        
        public MainRequest() {
        }
        
        public MainRequest(KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class MainRequestBody {
        
        public MainRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MainResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="MainResponse", Namespace="http://tempuri.org/", Order=0)]
        public KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainResponseBody Body;
        
        public MainResponse() {
        }
        
        public MainResponse(KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class MainResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string MainResult;
        
        public MainResponseBody() {
        }
        
        public MainResponseBody(string MainResult) {
            this.MainResult = MainResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface KanbanBoxCheckHealthSoapChannel : KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KanbanBoxCheckHealthSoapClient : System.ServiceModel.ClientBase<KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap>, KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap {
        
        public KanbanBoxCheckHealthSoapClient() {
        }
        
        public KanbanBoxCheckHealthSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KanbanBoxCheckHealthSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KanbanBoxCheckHealthSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KanbanBoxCheckHealthSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainResponse KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap.Main(KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequest request) {
            return base.Channel.Main(request);
        }
        
        public string Main() {
            KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequest inValue = new KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequest();
            inValue.Body = new KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainRequestBody();
            KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.MainResponse retVal = ((KanbanBoxCheckHealthScheduler.KanbanBoxCheckHealth.KanbanBoxCheckHealthSoap)(this)).Main(inValue);
            return retVal.Body.MainResult;
        }
    }
}
