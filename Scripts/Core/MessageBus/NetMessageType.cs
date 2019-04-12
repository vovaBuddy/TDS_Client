namespace Core.MessageBus
{
    public class NetMessageType
    {
        public string Type;
        public byte NetValue;

        public NetMessageType(string type, byte netValue)
        {
            Type = type;
            NetValue = netValue;
        }
    }
}