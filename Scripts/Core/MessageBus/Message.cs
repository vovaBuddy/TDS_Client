using Core.Services;
using UnityEngine.Networking;

namespace Core.MessageBus
{
    public abstract class Message
    {
        public string Type;
        public MessageData Data;
        public bool GeneratedChannelMsgType;
        public bool WaitingForSubscriber;

        public Message()
        {
            Type = null;
            Data = null;
            GeneratedChannelMsgType = false;
            WaitingForSubscriber = false;
        }

        public abstract void FreePooledObject();
    }
}