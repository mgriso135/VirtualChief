﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KISScheduler.VCSchedulerWarnings {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://kis.org", ConfigurationName="VCSchedulerWarnings.WarningSoap")]
    public interface WarningSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://kis.org/Main", ReplyAction="*")]
        void Main();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WarningSoapChannel : KISScheduler.VCSchedulerWarnings.WarningSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WarningSoapClient : System.ServiceModel.ClientBase<KISScheduler.VCSchedulerWarnings.WarningSoap>, KISScheduler.VCSchedulerWarnings.WarningSoap {
        
        public WarningSoapClient() {
        }
        
        public WarningSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WarningSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WarningSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WarningSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Main() {
            base.Channel.Main();
        }
    }
}
