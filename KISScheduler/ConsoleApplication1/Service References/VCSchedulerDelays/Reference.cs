﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KISScheduler.VCSchedulerDelays {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://kis.org", ConfigurationName="VCSchedulerDelays.RitardiSoap")]
    public interface RitardiSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://kis.org/Main", ReplyAction="*")]
        void Main();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RitardiSoapChannel : KISScheduler.VCSchedulerDelays.RitardiSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RitardiSoapClient : System.ServiceModel.ClientBase<KISScheduler.VCSchedulerDelays.RitardiSoap>, KISScheduler.VCSchedulerDelays.RitardiSoap {
        
        public RitardiSoapClient() {
        }
        
        public RitardiSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RitardiSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RitardiSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RitardiSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Main() {
            base.Channel.Main();
        }
    }
}
