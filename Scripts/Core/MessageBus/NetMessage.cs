using System;

namespace Core.MessageBus
{
    [Serializable]
    public struct NetMessage
    {
        public byte Type;
        public bool Broadcast;
        public int NetId;
        public object Data;
    }
}