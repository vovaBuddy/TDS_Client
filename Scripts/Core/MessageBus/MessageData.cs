using System;

namespace Core.MessageBus
{
    [Serializable]
    public abstract class MessageData
    {
        public abstract void FreeObjectInPool();
        public abstract MessageData GetCopy();
    }
}