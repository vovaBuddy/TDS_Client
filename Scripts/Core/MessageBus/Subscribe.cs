using System;
using System.Linq;

namespace Core.MessageBus
{
    public enum SubscribeType
    {
        Broadcast = 0, 
        Channel = 1, 
        Network = 2,
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class Subscribe : Attribute
    {
        public string[] subscribed_types;
        public SubscribeType Type;

        private Subscribe() { }
        public Subscribe(SubscribeType type, params string[] args)
        {
            subscribed_types = args;
            Type = type;
        }

        public Subscribe(params string[] args)
        {
            subscribed_types = args;
            Type = SubscribeType.Broadcast;
        }
        
        public Subscribe(SubscribeType type, params byte[] args)
        {
            subscribed_types = args.Select(a => a.ToString()).ToArray();
            Type = type;
        }
    }
}