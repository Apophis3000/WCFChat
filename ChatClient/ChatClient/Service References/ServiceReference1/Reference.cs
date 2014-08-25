﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.18444
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatClient.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IChatService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IChatService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Connect", ReplyAction="http://tempuri.org/IChatService/ConnectResponse")]
        bool Connect(string p);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Connect", ReplyAction="http://tempuri.org/IChatService/ConnectResponse")]
        System.Threading.Tasks.Task<bool> ConnectAsync(string p);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Disconnect", ReplyAction="http://tempuri.org/IChatService/DisconnectResponse")]
        void Disconnect(string p);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Disconnect", ReplyAction="http://tempuri.org/IChatService/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(string p);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Say", ReplyAction="http://tempuri.org/IChatService/SayResponse")]
        void Say(string user, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Say", ReplyAction="http://tempuri.org/IChatService/SayResponse")]
        System.Threading.Tasks.Task SayAsync(string user, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Whisper", ReplyAction="http://tempuri.org/IChatService/WhisperResponse")]
        void Whisper(string sender, string recipient, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Whisper", ReplyAction="http://tempuri.org/IChatService/WhisperResponse")]
        System.Threading.Tasks.Task WhisperAsync(string sender, string recipient, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/RetrieveMessages", ReplyAction="http://tempuri.org/IChatService/RetrieveMessagesResponse")]
        Model.Message[] RetrieveMessages(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/RetrieveMessages", ReplyAction="http://tempuri.org/IChatService/RetrieveMessagesResponse")]
        System.Threading.Tasks.Task<Model.Message[]> RetrieveMessagesAsync(string user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/SwitchChannel", ReplyAction="http://tempuri.org/IChatService/SwitchChannelResponse")]
        void SwitchChannel(string person, string channelName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/SwitchChannel", ReplyAction="http://tempuri.org/IChatService/SwitchChannelResponse")]
        System.Threading.Tasks.Task SwitchChannelAsync(string person, string channelName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetChannels", ReplyAction="http://tempuri.org/IChatService/GetChannelsResponse")]
        string[] GetChannels();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetChannels", ReplyAction="http://tempuri.org/IChatService/GetChannelsResponse")]
        System.Threading.Tasks.Task<string[]> GetChannelsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceChannel : ChatClient.ServiceReference1.IChatService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatServiceClient : System.ServiceModel.ClientBase<ChatClient.ServiceReference1.IChatService>, ChatClient.ServiceReference1.IChatService {
        
        public ChatServiceClient() {
        }
        
        public ChatServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ChatServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Connect(string p) {
            return base.Channel.Connect(p);
        }
        
        public System.Threading.Tasks.Task<bool> ConnectAsync(string p) {
            return base.Channel.ConnectAsync(p);
        }
        
        public void Disconnect(string p) {
            base.Channel.Disconnect(p);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(string p) {
            return base.Channel.DisconnectAsync(p);
        }
        
        public void Say(string user, string message) {
            base.Channel.Say(user, message);
        }
        
        public System.Threading.Tasks.Task SayAsync(string user, string message) {
            return base.Channel.SayAsync(user, message);
        }
        
        public void Whisper(string sender, string recipient, string message) {
            base.Channel.Whisper(sender, recipient, message);
        }
        
        public System.Threading.Tasks.Task WhisperAsync(string sender, string recipient, string message) {
            return base.Channel.WhisperAsync(sender, recipient, message);
        }
        
        public Model.Message[] RetrieveMessages(string user) {
            return base.Channel.RetrieveMessages(user);
        }
        
        public System.Threading.Tasks.Task<Model.Message[]> RetrieveMessagesAsync(string user) {
            return base.Channel.RetrieveMessagesAsync(user);
        }
        
        public void SwitchChannel(string person, string channelName) {
            base.Channel.SwitchChannel(person, channelName);
        }
        
        public System.Threading.Tasks.Task SwitchChannelAsync(string person, string channelName) {
            return base.Channel.SwitchChannelAsync(person, channelName);
        }
        
        public string[] GetChannels() {
            return base.Channel.GetChannels();
        }
        
        public System.Threading.Tasks.Task<string[]> GetChannelsAsync() {
            return base.Channel.GetChannelsAsync();
        }
    }
}
