using UnityEngine;
using System.Collections;
using Core.Services;

namespace Core.MessageBus
{
    public class CommonMessage : Message
    {
        private static ObjectPool<CommonMessage> _pool = new ObjectPool<CommonMessage>(10);

        public CommonMessage() : base() { }

        public static CommonMessage Get(string msgType, MessageData data = null)
        {
            var msg = _pool.Get();
            msg.Type = msgType;
            msg.Data = data;
            return msg;
        }

        public override void FreePooledObject()
        {
            Data = null;
            Type = null;
            WaitingForSubscriber = false;
            GeneratedChannelMsgType = false;

            _pool.Release(this);
        }
    }
}